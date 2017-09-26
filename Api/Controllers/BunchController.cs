using System.Web.Http;
using Api.Auth;
using Api.Models.BunchModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    [Route(ApiBunchUrl.Route)]
    [ApiAuthorize]
    public class BunchController : BaseController
    {
        [HttpGet]
        public BunchModel Get(string slug)
        {
            var request = new GetBunch.Request(CurrentUserName, slug);
            var bunchResult = UseCase.GetBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [HttpPost]
        public BunchModel Update(string slug, [FromBody] UpdateBunchPostModel post)
        {
            var request = new EditBunch.Request(CurrentUserName, slug, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
            var bunchResult = UseCase.EditBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}