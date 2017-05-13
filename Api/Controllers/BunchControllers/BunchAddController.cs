using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.BunchControllers
{
    public class BunchAddController : BaseController
    {
        [Route(ApiRoutes.BunchAdd)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Add([FromBody] AddBunchPostModel b)
        {
            var request = new AddBunch.Request(CurrentUserName, b.Name, b.Description, b.CurrencySymbol, b.CurrencyLayout, b.Timezone);
            var bunchResult = UseCase.AddBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}