using System.Web.Http;
using Api.Auth;
using Api.Models.AppModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class AppController : BaseController
    {
        [Route(ApiAppUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public AppModel Get(int id)
        {
            var result = UseCase.GetApp.Execute(new GetApp.Request(CurrentUserName, id));
            return new AppModel(result);
        }

        [Route(ApiAppsUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel GetList()
        {
            var request = new AppList.AllAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }

        [Route(ApiAppsUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public AppModel Add([FromBody] AddAppPostModel post)
        {
            var result = UseCase.AddApp.Execute(new AddApp.Request(CurrentUserName, post.Name));
            return new AppModel(result);
        }
    }
}