using System;
using System.IdentityModel.Tokens.Jwt;
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

namespace Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly UrlProvider _urls;
        private readonly UserDetails _userDetails;
        private readonly UserList _userList;
        private readonly EditUser _editUser;
        private readonly AddUser _addUser;
        private readonly Login _login;
        private readonly ChangePassword _changePassword;

        public UserController(
            AppSettings appSettings, 
            UrlProvider urls, 
            UserDetails userDetails,
            UserList userList,
            EditUser editUser,
            AddUser addUser,
            Login login,
            ChangePassword changePassword) 
            : base(appSettings)
        {
            _urls = urls;
            _userDetails = userDetails;
            _userList = userList;
            _editUser = editUser;
            _addUser = addUser;
            _login = login;
            _changePassword = changePassword;
        }

        [Route(ApiRoutes.User.Get)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel GetUser(string userName)
        {
            var userDetails = _userDetails.Execute(new UserDetails.Request(CurrentUserName, userName));
            return userDetails.CanViewAll ? new FullUserModel(userDetails) : new UserModel(userDetails);
        }

        [Route(ApiRoutes.User.List)]
        [HttpGet]
        [ApiAuthorize]
        public UserListModel List()
        {
            var userListResult = _userList.Execute(new UserList.Request(CurrentUserName));
            return new UserListModel(userListResult, _urls);
        }

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

        [Route(ApiRoutes.Profile.PasswordChange)]
        [HttpPut]
        [ApiAuthorize]
        public OkModel ChangePassword([FromBody] ChangePasswordPostModel post)
        {
            var request = new ChangePassword.Request(CurrentUserName, post.NewPassword, post.OldPassword);
            _changePassword.Execute(request);
            return new OkModel();
        }

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
        [HttpPost("token")]
        public IActionResult Authenticate([FromForm]string userName, [FromForm] string password)
        {
            var result = _login.Execute(new Login.Request(userName, password));
            var token = CreateToken(result.UserName);
            return Ok(token);
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
}