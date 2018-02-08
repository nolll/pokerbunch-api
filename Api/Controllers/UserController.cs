using System.Web.Http;
using Api.Auth;
using Api.Extensions;
using Api.Models.AppModels;
using Api.Models.BunchModels;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Core.UseCases;
using PokerBunch.Common.Routes;
using PokerBunch.Common.Urls.ApiUrls;
using PokerBunch.Common.Urls.SiteUrls;

namespace Api.Controllers
{
    public class UserController : BaseController
    {
        [Route(ApiRoutes.User)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel GetUser(string userName)
        {
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName, userName));
            return userDetails.CanViewAll ? new FullUserModel(userDetails) : new UserModel(userDetails);
        }

        [Route(ApiRoutes.Users)]
        [HttpGet]
        [ApiAuthorize]
        public UserListModel List()
        {
            var userListResult = UseCase.UserList.Execute(new UserList.Request(CurrentUserName));
            return new UserListModel(userListResult);
        }

        [Route(ApiRoutes.User)]
        [HttpPost]
        [ApiAuthorize]
        public UserModel Update(string userName, [FromBody] UpdateUserPostModel post)
        {
            var request = new EditUser.Request(CurrentUserName, post.DisplayName, post.RealName, post.Email);
            var editUserResult = UseCase.EditUser.Execute(request);
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(editUserResult.UserName));
            return new FullUserModel(userDetails);
        }

        [Route(ApiRoutes.User)]
        [HttpPost]
        public OkModel Add([FromBody] AddUserPostModel post)
        {
            UseCase.AddUser.Execute(new AddUser.Request(post.UserName, post.DisplayName, post.Email, post.Password, new LoginUrl().Absolute()));
            return new OkModel();
        }

        [Route(ApiRoutes.Profile)]
        [HttpGet]
        [ApiAuthorize]
        public UserModel Profile()
        {
            var userDetails = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName));
            return new FullUserModel(userDetails);
        }

        [Route(ApiRoutes.Apps)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel Apps()
        {
            var request = new AppList.UserAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }

        [Route(ApiRoutes.Bunches)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel Bunches()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }
    }
}