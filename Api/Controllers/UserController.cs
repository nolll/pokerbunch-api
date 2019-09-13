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
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly UrlProvider _urls;

        public UserController(AppSettings appSettings, UrlProvider urls) : base(appSettings)
        {
            _urls = urls;
        }

        [Route(ApiRoutes.User.Get)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel GetUser(string userName)
        {
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName, userName));
            return userDetails.CanViewAll ? new FullUserModel(userDetails) : new UserModel(userDetails);
        }

        [Route(ApiRoutes.User.List)]
        [HttpGet]
        [ApiAuthorize]
        public UserListModel List()
        {
            var userListResult = UseCase.UserList.Execute(new UserList.Request(CurrentUserName));
            return new UserListModel(userListResult, _urls);
        }

        [Route(ApiRoutes.User.Get)]
        [HttpPost]
        [ApiAuthorize]
        public UserModel Update(string userName, [FromBody] UpdateUserPostModel post)
        {
            var request = new EditUser.Request(CurrentUserName, post.DisplayName, post.RealName, post.Email);
            var editUserResult = UseCase.EditUser.Execute(request);
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(editUserResult.UserName));
            return new FullUserModel(userDetails);
        }

        [Route(ApiRoutes.User.List)]
        [HttpPost]
        public OkModel Add([FromBody] AddUserPostModel post)
        {
            UseCase.AddUser.Execute(new AddUser.Request(post.UserName, post.DisplayName, post.Email, post.Password, _urls.Site.Login.Absolute()));
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
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName));
            return new FullUserModel(userDetails);
        }

        // https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api
        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult Authenticate([FromForm]LoginPostModel postModel)
        {
            var result = UseCase.Login.Execute(new Login.Request(postModel.UserName, postModel.Password));
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

    public class LoginPostModel
    {
        public string UserName { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }
    }
}