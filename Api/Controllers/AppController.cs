using System.Web.Http;
using Api.Auth;
using Api.Models;
using Core.UseCases;
using Web.Common.Routes;

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

        //[Route(ApiRoutes.AppAdd)]
        //[AcceptVerbs(HttpVerb.Post)]
        //[ApiAuthorize]
        //public LocationModel Add([FromBody] LocationModel location)
        //{
        //    var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, location.Bunch, location.Name));
        //    return new LocationModel(result);
        //}

        //[Route(ApiRoutes.AppAdd)]
        //[AcceptVerbs(HttpVerb.Post)]
        //[ApiAuthorize]
        //public LocationModel Save([FromBody] LocationModel location)
        //{
        //    var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, location.Bunch, location.Name));
        //    return new LocationModel(result);
        //}
    }
}