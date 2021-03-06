using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Auth;
using Api.Extensions;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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
    public UserModel GetUser(string userName)
    {
        var userDetails = _userDetails.Execute(new UserDetails.Request(CurrentUserName, userName));
        return userDetails.CanViewAll ? new FullUserModel(userDetails) : new UserModel(userDetails);
    }

    /// <summary>
    /// List users.
    /// </summary>
    [Route(ApiRoutes.User.List)]
    [HttpGet]
    [ApiAuthorize]
    public UserListModel List()
    {
        var userListResult = _userList.Execute(new UserList.Request(CurrentUserName));
        return new UserListModel(userListResult, _urls);
    }

    /// <summary>
    /// Update user.
    /// </summary>
    [Route(ApiRoutes.User.Get)]
    [HttpPut]
    [ApiAuthorize]
    public UserModel Update(string userName, [FromBody] UpdateUserPostModel post)
    {
        var request = new EditUser.Request(userName, post.DisplayName, post.RealName, post.Email);
        var editUserResult = _editUser.Execute(request);
        var userDetails = _userDetails.Execute(new UserDetails.Request(editUserResult.UserName));
        return new FullUserModel(userDetails);
    }

    /// <summary>
    /// Change password.
    /// </summary>
    [Route(ApiRoutes.Profile.Password)]
    [HttpPut]
    [ApiAuthorize]
    public OkModel ChangePassword([FromBody] ChangePasswordPostModel post)
    {
        var request = new ChangePassword.Request(CurrentUserName, post.NewPassword, post.OldPassword);
        _changePassword.Execute(request);
        return new OkModel();
    }

    /// <summary>
    /// Reset password.
    /// </summary>
    [Route(ApiRoutes.Profile.Password)]
    [HttpPost]
    public OkModel ResetPassword([FromBody] ResetPasswordPostModel post)
    {
        var request = new ResetPassword.Request(post.Email, _urls.Site.Login.Absolute());
        _resetPassword.Execute(request);
        return new OkModel();
    }

    /// <summary>
    /// Add user.
    /// </summary>
    [Route(ApiRoutes.User.List)]
    [HttpPost]
    public OkModel Add([FromBody] AddUserPostModel post)
    {
        _addUser.Execute(new AddUser.Request(post.UserName, post.DisplayName, post.Email, post.Password, _urls.Site.Login.Absolute()));
        return new OkModel();
    }

    /// <summary>
    /// Get the current user.
    /// </summary>
    /// <returns>Returns the current user</returns>
    [Route(ApiRoutes.Profile.Get)]
    [HttpGet]
    [ApiAuthorize]
    public UserModel Profile()
    {
        var userDetails = _userDetails.Execute(new UserDetails.Request(CurrentUserName));
        return new FullUserModel(userDetails);
    }

    // https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api
    /// <summary>
    /// Get an auth token
    /// </summary>
    /// <param name="userName">Username</param>
    /// <param name="password" format="password">Password</param>
    /// <returns>A token that can be used for authentication</returns>
    [AllowAnonymous]
    [HttpPost]
    [Route(ApiRoutes.Token.Get)]
    public async Task<IActionResult> Authenticate([FromForm] string userName, [FromForm] string password)
    {
        var post = await GetLoginPostModel(userName, password);
        var result = _login.Execute(new Login.Request(post.UserName, post.Password));
        var token = CreateToken(result.UserName);
        return Ok(token);
    }

    private async Task<LoginPostModel> GetLoginPostModel(string userName, string password)
    {
        var fromForm = GetLoginPostModelFromForm(userName, password);
        if (fromForm != null)
            return fromForm;

        var fromBody = await GetLoginPostModelFromJsonBody();
        if (fromBody != null)
            return fromBody;

        return new LoginPostModel();
    }

    private LoginPostModel GetLoginPostModelFromForm(string userName, string password)
    {
        if (userName == null || password == null)
            return null;

        return new LoginPostModel
        {
            UserName = userName,
            Password = password
        };
    }

    private async Task<LoginPostModel> GetLoginPostModelFromJsonBody()
    {
        var body = await Request.BodyAsString();
        return !string.IsNullOrEmpty(body)
            ? JsonConvert.DeserializeObject<LoginPostModel>(body) 
            : null;
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