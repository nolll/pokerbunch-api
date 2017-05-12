using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class UserController : BaseApiController
    {
        [Route(ApiRoutes.UserGet)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel GetUser(string userName)
        {
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName, userName));
            return userDetails.CanViewAll ? new FullUserModel(userDetails) : new UserModel(userDetails);
        }

        [Route(ApiRoutes.UserProfile)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel Profile()
        {
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName));
            return new FullUserModel(userDetails);
        }

        [Route(ApiRoutes.UserList)]
        [HttpGet]
        [ApiAuthorize]
        public UserListModel List()
        {
            var userListResult = UseCase.UserList.Execute(new UserList.Request(CurrentUserName));
            return new UserListModel(userListResult);
        }

        [Route(ApiRoutes.UserBunchList)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel Bunches()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }
        
        [Route(ApiRoutes.UserAppList)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel Apps()
        {
            var request = new AppList.UserAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }

        [Route(ApiRoutes.UserSave)]
        [HttpPost]
        [ApiAuthorize]
        public UserModel Save(string userName, [FromBody] EditUserPostModel u)
        {
            var request = new EditUser.Request(CurrentUserName, u.DisplayName, u.RealName, u.Email);
            var editUserResult = UseCase.EditUser.Execute(request);
            return GetUser(editUserResult.UserName);
        }
    }
}