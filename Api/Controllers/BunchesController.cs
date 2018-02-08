using System.Web.Http;
using Api.Auth;
using Api.Models.BunchModels;
using Core.UseCases;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    [Route(ApiRoutes.Bunches)]
    [ApiAuthorize]
    public class BunchesController : BaseController
    {
        [HttpGet]
        public BunchListModel List()
        {
            var request = new GetBunchList.AllBunchesRequest(CurrentUserName);
            var bunchListResult = UseCase.GetBunchList.Execute(request);
            return new BunchListModel(bunchListResult);
        }

        [HttpPost]
        public BunchModel Add([FromBody] AddBunchPostModel post)
        {
            var request = new AddBunch.Request(CurrentUserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
            var bunchResult = UseCase.AddBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}