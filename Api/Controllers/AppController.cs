using System.Web.Http;
using Api.Auth;
using Api.Models.AppModels;
using Api.Models.CommonModels;
using Core.UseCases;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    public class AppController : BaseController
    {
        [Route(ApiRoutes.App.Get)]
        [HttpGet]
        [ApiAuthorize]
        public AppModel Get(int appId)
        {
            var result = UseCase.GetApp.Execute(new GetApp.Request(CurrentUserName, appId));
            return new AppModel(result);
        }

        [Route(ApiRoutes.App.List)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel GetList()
        {
            var request = new AppList.AllAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }

        [Route(ApiRoutes.App.ListForCurrentUser)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel Apps()
        {
            var request = new AppList.UserAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }

        [Route(ApiRoutes.App.List)]
        [HttpPost]
        [ApiAuthorize]
        public AppModel Add([FromBody] AddAppPostModel post)
        {
            var result = UseCase.AddApp.Execute(new AddApp.Request(CurrentUserName, post.Name));
            return new AppModel(result);
        }

        [Route(ApiRoutes.App.Get)]
        [HttpDelete]
        [ApiAuthorize]
        public OkModel Delete(int appId)
        {
            UseCase.DeleteApp.Execute(new DeleteApp.Request(CurrentUserName, appId));
            return new OkModel();
        }
    }
}