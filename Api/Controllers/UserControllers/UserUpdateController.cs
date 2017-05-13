using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;
using JetBrains.Annotations;

namespace Api.Controllers.UserControllers
{
    public class UserUpdateController : BaseController
    {
        [Route(ApiRoutes.UserUpdate)]
        [HttpPost]
        [ApiAuthorize]
        public UserModel Update(string userName, [FromBody] UpdateUserPostModel u)
        {
            var request = new EditUser.Request(CurrentUserName, u.DisplayName, u.RealName, u.Email);
            var editUserResult = UseCase.EditUser.Execute(request);
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(editUserResult.UserName));
            return new FullUserModel(userDetails);
        }
    }

    public class UpdateUserPostModel
    {
        public string DisplayName { get; [UsedImplicitly] set; }
        public string Email { get; [UsedImplicitly] set; }
        public string RealName { get; [UsedImplicitly] set; }
    }
}