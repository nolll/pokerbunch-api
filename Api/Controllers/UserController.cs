using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Api.Auth;
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

public class UserController : BaseController
{
    private readonly UrlProvider _urls;
    private readonly UserDetails _userDetails;
    private readonly UserList _userList;
    private readonly EditUser _editUser;
    private readonly AddUser _addUser;
    private readonly Login _login;
    private readonly ChangePassword _changePassword;
    private readonly ResetPassword _resetPassword;

    public UserController(
        AppSettings appSettings, 
        UrlProvider urls, 
        UserDetails userDetails,
        UserList userList,
        EditUser editUser,
        AddUser addUser,
        Login login,
        ChangePassword changePassword,
        ResetPassword resetPassword) 
        : base(appSettings)
    {
        _urls = urls;
        _userDetails = userDetails;
        _userList = userList;
        _editUser = editUser;
        _addUser = addUser;
        _login = login;
        _changePassword = changePassword;
        _resetPassword = resetPassword;
    }

    /// <summary>
    /// Get user.
    /// </summary>
    [Route(ApiRoutes.User.Get)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult GetUser(string userName)
    {
        var result = _userDetails.Execute(new UserDetails.Request(CurrentUserName, userName));
        return Model(result, () => result.Data.CanViewAll 
            ? new FullUserModel(result.Data) 
            : new UserModel(result.Data)
        );
    }

    /// <summary>
    /// List users.
    /// </summary>
    [Route(ApiRoutes.User.List)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult List()
    {
        var result = _userList.Execute(new UserList.Request(CurrentUserName));
        return Model(result, () => result.Data.Users.Select(o => new UserItemModel(o, _urls)));
    }

    /// <summary>
    /// Update user.
    /// </summary>
    [Route(ApiRoutes.User.Get)]
    [HttpPut]
    [ApiAuthorize]
    public ObjectResult Update(string userName, [FromBody] UpdateUserPostModel post)
    {
        var updateRequest = new EditUser.Request(userName, post.DisplayName, post.RealName, post.Email);
        var updateResult = _editUser.Execute(updateRequest);
        if (!updateResult.Success)
            return Error(updateResult.Error);

        var result = _userDetails.Execute(new UserDetails.Request(updateResult.Data.UserName));
        return Model(result, () => new FullUserModel(result.Data));
    }

    /// <summary>
    /// Change password.
    /// </summary>
    [Route(ApiRoutes.Profile.Password)]
    [HttpPut]
    [ApiAuthorize]
    public ObjectResult ChangePassword([FromBody] ChangePasswordPostModel post)
    {
        var request = new ChangePassword.Request(CurrentUserName, post.NewPassword, post.OldPassword);
        var result = _changePassword.Execute(request);
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Reset password.
    /// </summary>
    [Route(ApiRoutes.Profile.Password)]
    [HttpPost]
    public ObjectResult ResetPassword([FromBody] ResetPasswordPostModel post)
    {
        var request = new ResetPassword.Request(post.Email, _urls.Site.Login);
        var result = _resetPassword.Execute(request);
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Add user.
    /// </summary>
    [Route(ApiRoutes.User.List)]
    [HttpPost]
    public ObjectResult Add([FromBody] AddUserPostModel post)
    {
        var result = _addUser.Execute(new AddUser.Request(post.UserName, post.DisplayName, post.Email, post.Password, _urls.Site.Login));
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Get the current user.
    /// </summary>
    /// <returns>Returns the current user</returns>
    [Route(ApiRoutes.Profile.Get)]
    [HttpGet]
    [ApiAuthorize]
    public ObjectResult Profile()
    {
        var result = _userDetails.Execute(new UserDetails.Request(CurrentUserName));
        return Model(result, () => new FullUserModel(result.Data));
    }

    // https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api
    /// <summary>
    /// Get an auth token by posting form data.
    /// </summary>
    /// <param name="userName">Username</param>
    /// <param name="password" format="password">Password</param>
    /// <returns>A token that can be used for authentication</returns>
    [AllowAnonymous]
    [HttpPost]
    [Obsolete]
    [Route(ApiRoutes.Auth.Token)]
    public ObjectResult Token([FromForm] string userName, [FromForm] string password)
    {
        var post = new LoginPostModel(userName, password);
        var token = GetToken(post);
        return Ok(token);
    }

    /// <summary>
    /// Get an auth token by posting json data
    /// </summary>
    /// <returns>A token that can be used for authentication</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route(ApiRoutes.Auth.Login)]
    public ObjectResult Login([FromBody] LoginPostModel post)
    {
        var token = GetToken(post);
        return Ok(token);
    }

    private string GetToken(LoginPostModel loginPostModel)
    {
        var result = _login.Execute(new Login.Request(loginPostModel.UserName, loginPostModel.Password));
        return CreateToken(result.Data.UserName);
    }
    
    private string CreateToken(string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AppSettings.Auth.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName)
            }),
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}