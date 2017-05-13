using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.UserControllers
{
    public class UserGetController : BaseController
    {
        [Route(ApiRoutes.UserGet)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel GetUser(string userName)
        {
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName, userName));
            return userDetails.CanViewAll ? new FullUserModel(userDetails) : new UserModel(userDetails);
        }
    }
}