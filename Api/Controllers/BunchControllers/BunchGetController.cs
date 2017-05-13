using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.BunchControllers
{
    public class BunchGetController : BaseController
    {
        [Route(ApiRoutes.BunchGet)]
        [HttpGet]
        [ApiAuthorize]
        public BunchModel Get(string slug)
        {
            var request = new GetBunch.Request(CurrentUserName, slug);
            var bunchResult = UseCase.GetBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}