using Api.Auth;
using Api.Models.AppModels;
using Api.Models.CommonModels;
using Api.Routes;
using Api.Settings;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AppController : BaseController
    {
        private readonly UrlProvider _urls;
        private readonly GetApp _getApp;
        private readonly AppList _appList;
        private readonly AddApp _addApp;
        private readonly DeleteApp _deleteApp;

        public AppController(
            AppSettings appSettings,
            UrlProvider urls,
            GetApp getApp,
            AppList appList,
            AddApp addApp,
            DeleteApp deleteApp) : base(appSettings)
        {
            _urls = urls;
            _getApp = getApp;
            _appList = appList;
            _addApp = addApp;
            _deleteApp = deleteApp;
        }

        [Route(ApiRoutes.App.Get)]
        [HttpGet]
        [ApiAuthorize]
        public AppModel Get(int appId)
        {
            var result = _getApp.Execute(new GetApp.Request(CurrentUserName, appId));
            return new AppModel(result, _urls);
        }

        [Route(ApiRoutes.App.List)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel GetList()
        {
            var request = new AppList.AllAppsRequest(CurrentUserName);
            var appListResult = _appList.Execute(request);
            return new AppListModel(appListResult, _urls);
        }

        [Route(ApiRoutes.App.ListForCurrentUser)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel Apps()
        {
            var request = new AppList.UserAppsRequest(CurrentUserName);
            var appListResult = _appList.Execute(request);
            return new AppListModel(appListResult, _urls);
        }

        [Route(ApiRoutes.App.List)]
        [HttpPost]
        [ApiAuthorize]
        public AppModel Add([FromBody] AddAppPostModel post)
        {
            var result = _addApp.Execute(new AddApp.Request(CurrentUserName, post.Name));
            return new AppModel(result, _urls);
        }

        [Route(ApiRoutes.App.Get)]
        [HttpDelete]
        [ApiAuthorize]
        public OkModel Delete(int appId)
        {
            _deleteApp.Execute(new DeleteApp.Request(CurrentUserName, appId));
            return new OkModel();
        }
    }
}