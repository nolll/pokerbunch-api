using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Api.Bootstrapping;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
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
    /// <summary>
    /// Get a user
    /// </summary>
    [Route(ApiRoutes.User.Get)]
    [HttpGet]
    [Authorize]
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

    /// <summary>
    /// List users
    /// </summary>
    [Route(ApiRoutes.User.List)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> List()
    {
        var result = await userList.Execute(new UserList.Request(CurrentUserName));
        return Model(result, () => result.Data?.Users.Select(o => new UserItemModel(o, urls)));
    }

    /// <summary>
    /// Update a user
    /// </summary>
    [Route(ApiRoutes.User.Update)]
    [HttpPut]
    [Authorize]
    public async Task<ObjectResult> Update(string userName, [FromBody] UpdateUserPostModel post)
    {
        var updateRequest = new EditUser.Request(userName, post.DisplayName, post.RealName, post.Email);
        var updateResult = await editUser.Execute(updateRequest);
        if (!updateResult.Success)
            return Error(updateResult.Error);

        var result = await userDetails.Execute(new UserDetails.Request(updateResult.Data!.UserName));
        return Model(result, () => result.Data is not null ? new FullUserModel(result.Data) : null);
    }

    /// <summary>
    /// Change password
    /// </summary>
    [Route(ApiRoutes.Profile.ChangePassword)]
    [HttpPut]
    [Authorize]
    public async Task<ObjectResult> ChangePassword([FromBody] ChangePasswordPostModel post)
    {
        var request = new ChangePassword.Request(CurrentUserName, post.NewPassword, post.OldPassword);
        var result = await changePassword.Execute(request);
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Reset password
    /// </summary>
    [Route(ApiRoutes.Profile.ResetPassword)]
    [HttpPost]
    public async Task<ObjectResult> ResetPassword([FromBody] ResetPasswordPostModel post)
    {
        var request = new ResetPassword.Request(post.Email, urls.Site.Login);
        var result = await resetPassword.Execute(request);
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Add a user.
    /// </summary>
    [Route(ApiRoutes.User.Add)]
    [HttpPost]
    public async Task<ObjectResult> Add([FromBody] AddUserPostModel post)
    {
        var result = await addUser.Execute(new AddUser.Request(post.UserName, post.DisplayName, post.Email, post.Password, urls.Site.Login));
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Get the current user
    /// </summary>
    /// <returns>Returns the current user</returns>
    [Route(ApiRoutes.Profile.Get)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Profile()
    {
        var result = await userDetails.Execute(new UserDetails.Request(CurrentUserName));
        return Model(result, () => result.Data is not null ? new FullUserModel(result.Data) : null);
    }
    
    /// <summary>
    /// Get an auth token
    /// </summary>
    /// <returns>A token that can be used for authentication</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route(ApiRoutes.Auth.Login)]
    public async Task<ObjectResult> Login([FromBody] LoginPostModel post)
    {
        var result = await login.Execute(new Login.Request(post.UserName, post.Password));
        return result.Success
            ? new ObjectResult(CreateToken(result.Data?.UserName ?? "")) 
            : Error(result.Error);
    }

    private string CreateToken(string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSecretProvider.GetSecret(AppSettings.Auth.Secret));
        var symmetricKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName)
        });

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = credentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}