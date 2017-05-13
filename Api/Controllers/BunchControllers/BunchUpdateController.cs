using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers.BunchControllers
{
    public class BunchUpdateController : BaseController
    {
        [Route(ApiRoutes.BunchUpdate)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Update(string slug, [FromBody] UpdateBunchPostModel b)
        {
            var request = new EditBunch.Request(CurrentUserName, slug, b.Description, b.CurrencySymbol, b.CurrencyLayout, b.Timezone, b.HouseRules, b.DefaultBuyin);
            var bunchResult = UseCase.EditBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}