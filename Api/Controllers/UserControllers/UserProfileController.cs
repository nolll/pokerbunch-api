using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.UserControllers
{
    public class UserProfileController : BaseController
    {
        [Route(ApiRoutes.UserProfile)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel Profile()
        {
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName));
            return new FullUserModel(userDetails);
        }
    }
}