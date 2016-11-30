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
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public UserModel GetUser(string name)
        {
            var getUserResult = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName, name));
            return new UserModel(getUserResult);
        }

        [Route(ApiRoutes.UserProfile)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public UserModel Profile()
        {
            var getUserResult = UseCase.UserDetails.Execute(new UserDetails.Request(CurrentUserName));
            return new UserModel(getUserResult);
        }

        [Route(ApiRoutes.UserBunchList)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public BunchListModel Bunches()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.UserAppList)]
        [AcceptVerbs(HttpVerb.Get)]
        [ApiAuthorize]
        public AppListModel Apps()
        {
            var request = new AppList.UserAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }
    }
}