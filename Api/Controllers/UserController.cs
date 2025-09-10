using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Api.Auth;
using Api.Models;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

public class UserController(
    AppSettings appSettings,
    UrlProvider urls,
    UserDetails userDetails,
    UserList userList,
    EditUser editUser,
    AddUser addUser,
    Login login,
    ChangePassword changePassword,
    ResetPassword resetPassword)
    : BaseController(appSettings)
{
    [Route(ApiRoutes.User.Get)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get user")]
    public async Task<ObjectResult> GetUser(string userName)
    {
        var result = await userDetails.Execute(new UserDetails.Request(CurrentUserName, userName));

        if (result.Data is null)
            return Success(null);

        var canViewAll = result.Data?.CanViewAll ?? false;

        return Model(result, () => canViewAll 
            ? new FullUserModel(result.Data!) 
            : new UserModel(result.Data!)
        );
    }
    
    [Route(ApiRoutes.User.List)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List users")]
    public async Task<ObjectResult> List()
    {
        var result = await userList.Execute(new UserList.Request(Principal));
        return Model(result, () => result.Data?.Users.Select(o => new UserItemModel(o, urls)));
    }
    
    [Route(ApiRoutes.User.Update)]
    [HttpPut]
    [Authorize]
    [EndpointSummary("Update user")]
    public async Task<ObjectResult> Update(string userName, [FromBody] UpdateUserPostModel post)
    {
        var updateRequest = new EditUser.Request(userName, post.DisplayName, post.RealName, post.Email);
        var updateResult = await editUser.Execute(updateRequest);
        if (!updateResult.Success)
            return Error(updateResult.Error);

        var result = await userDetails.Execute(new UserDetails.Request(updateResult.Data!.UserName));
        return Model(result, () => result.Data is not null ? new FullUserModel(result.Data) : null);
    }
    
    [Route(ApiRoutes.Profile.ChangePassword)]
    [HttpPut]
    [Authorize]
    [EndpointSummary("Change password")]
    public async Task<ObjectResult> ChangePassword([FromBody] ChangePasswordPostModel post)
    {
        var request = new ChangePassword.Request(CurrentUserName, post.NewPassword, post.OldPassword);
        var result = await changePassword.Execute(request);
        return Model(result, () => new OkModel());
    }
    
    [Route(ApiRoutes.Profile.ResetPassword)]
    [HttpPost]
    [EndpointSummary("Reset password")]
    public async Task<ObjectResult> ResetPassword([FromBody] ResetPasswordPostModel post)
    {
        var request = new ResetPassword.Request(post.Email, urls.Site.Login);
        var result = await resetPassword.Execute(request);
        return Model(result, () => new OkModel());
    }
    
    [Route(ApiRoutes.User.Add)]
    [HttpPost]
    [EndpointSummary("Add user")]
    public async Task<ObjectResult> Add([FromBody] AddUserPostModel post)
    {
        var result = await addUser.Execute(new AddUser.Request(post.UserName, post.DisplayName, post.Email, post.Password, urls.Site.Login));
        return Model(result, () => new OkModel());
    }
    
    [Route(ApiRoutes.Profile.Get)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get authenticated user")]
    public async Task<ObjectResult> Profile()
    {
        var result = await userDetails.Execute(new UserDetails.Request(CurrentUserName));
        return Model(result, () => result.Data is not null ? new FullUserModel(result.Data) : null);
    }
    
    [AllowAnonymous]
    [HttpPost]
    [Route(ApiRoutes.Auth.Login)]
    [EndpointSummary("Get an auth token")]
    [EndpointDescription("Get a token that can bu used for authentication")]
    public async Task<ObjectResult> Login([FromBody] LoginPostModel post)
    {
        var result = await login.Execute(new Login.Request(post.UserName, post.Password));
        
        return result is { Success: true, Data: not null }
            ? new ObjectResult(CreateToken(result.Data)) 
            : Error(result.Error);
    }

    private string CreateToken(Login.Result data)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSecretProvider.GetSecret(AppSettings.Auth.Secret));
        var symmetricKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, data.UserName),
            new Claim(CustomClaimTypes.Version, "2"),
            new Claim(CustomClaimTypes.UserId, data.UserId),
            new Claim(CustomClaimTypes.UserDisplayName, data.DisplayName),
            new Claim(CustomClaimTypes.IsAdmin, data.IsAdmin.ToString().ToLower()),
            new Claim(CustomClaimTypes.Bunches, ToJson(data.BunchResults), JsonClaimValueTypes.JsonArray)
        ]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = credentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string ToJson(List<Login.ResultBunch> bunchResults)
    {
        var tokenBunches = bunchResults.Select(ToTokenBunch).ToArray();
        return JsonSerializer.Serialize(tokenBunches);
    }
    
    private static TokenBunchModel ToTokenBunch(Login.ResultBunch b) => new(b.BunchId, b.BunchSlug, b.BunchName, b.PlayerId, b.PlayerName, b.Role.ToString().ToLower());
}