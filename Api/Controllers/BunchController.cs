using System.Web.Http;
using Api.Auth;
using Api.Models.BunchModels;
using Core.UseCases;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Controllers
{
    [Route(ApiBunchUrl.Route)]
    [ApiAuthorize]
    public class BunchController : BaseController
    {
        [HttpGet]
        public BunchModel Get(string bunchId)
        {
            var request = new GetBunch.Request(CurrentUserName, bunchId);
            var bunchResult = UseCase.GetBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [HttpPost]
        public BunchModel Update(string bunchId, [FromBody] UpdateBunchPostModel post)
        {
            var request = new EditBunch.Request(CurrentUserName, bunchId, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
            var bunchResult = UseCase.EditBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}