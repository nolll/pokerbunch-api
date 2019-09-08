using Api.Auth;
using Api.Models.AppModels;
using Api.Models.CommonModels;
using Api.Routes;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AppController : BaseController
    {
        private readonly UrlProvider _urls;

        public AppController(Settings settings, UrlProvider urls) : base(settings)
        {
            _urls = urls;
        }

        [Route(ApiRoutes.App.Get)]
        [HttpGet]
        [ApiAuthorize]
        public AppModel Get(int appId)
        {
            var result = UseCase.GetApp.Execute(new GetApp.Request(CurrentUserName, appId));
            return new AppModel(result, _urls);
        }

        [Route(ApiRoutes.App.List)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel GetList()
        {
            var request = new AppList.AllAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult, _urls);
        }

        [Route(ApiRoutes.App.ListForCurrentUser)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel Apps()
        {
            var request = new AppList.UserAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult, _urls);
        }

        [Route(ApiRoutes.App.List)]
        [HttpPost]
        [ApiAuthorize]
        public AppModel Add([FromBody] AddAppPostModel post)
        {
            var result = UseCase.AddApp.Execute(new AddApp.Request(CurrentUserName, post.Name));
            return new AppModel(result, _urls);
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