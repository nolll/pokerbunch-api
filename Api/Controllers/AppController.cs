using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class AppController : BaseApiController
    {
        [Route(ApiRoutes.AppList)]
        [AcceptVerbs(HttpVerb.Get)]
        [ApiAuthorize]
        public AppListModel GetList()
        {
            var request = new AppList.AllAppsRequest(CurrentUserName);
            var appListResult = UseCase.GetAppList.Execute(request);
            return new AppListModel(appListResult);
        }

        [Route(ApiRoutes.AppGet)]
        [AcceptVerbs(HttpVerb.Get)]
        [ApiAuthorize]
        public AppModel Get(int id)
        {
            var result = UseCase.GetApp.Execute(new GetApp.Request(id));
            return new AppModel(result);
        }

        [Route(ApiRoutes.AppAdd)]
        [AcceptVerbs(HttpVerb.Post)]
        [ApiAuthorize]
        public AppModel Add([FromBody] AppModel app)
        {
            var result = UseCase.AddApp.Execute(new AddApp.Request(CurrentUserName, app.Name));
            return new AppModel(result);
        }
    }
}