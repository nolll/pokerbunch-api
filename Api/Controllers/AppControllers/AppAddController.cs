using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.AppControllers
{
    public class AppAddController : BaseController
    {
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