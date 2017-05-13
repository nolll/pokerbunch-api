using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Models.AppModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class AppController : BaseController
    {
        [Route(ApiRoutes.AppGet)]
        [HttpGet]
        [ApiAuthorize]
        public AppModel Get(int id)
        {
            var result = UseCase.GetApp.Execute(new GetApp.Request(CurrentUserName, id));
            return new AppModel(result);
        }

        [Route(ApiRoutes.AppList)]
        [HttpGet]
        [ApiAuthorize]
        public AppListModel GetList()
        {
            var request = new AppList.AllAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }

        [Route(ApiRoutes.AppAdd)]
        [HttpGet]
        [ApiAuthorize]
        public AppModel Add([FromBody] AppModel app)
        {
            var result = UseCase.AddApp.Execute(new AddApp.Request(CurrentUserName, app.Name));
            return new AppModel(result);
        }
    }
}