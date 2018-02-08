using System.Web.Http;
using Api.Auth;
using Api.Models.BunchModels;
using Core.UseCases;
using PokerBunch.Common.Routes;

namespace Api.Controllers
{
    public class BunchController : BaseController
    {
        [Route(ApiRoutes.Bunch)]
        [HttpGet]
        [ApiAuthorize]
        public BunchModel Get(string bunchId)
        {
            var request = new GetBunch.Request(CurrentUserName, bunchId);
            var bunchResult = UseCase.GetBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.Bunch)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Update(string bunchId, [FromBody] UpdateBunchPostModel post)
        {
            var request = new EditBunch.Request(CurrentUserName, bunchId, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
            var bunchResult = UseCase.EditBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.Bunches)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel List()
        {
            var request = new GetBunchList.AllBunchesRequest(CurrentUserName);
            var bunchListResult = UseCase.GetBunchList.Execute(request);
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.BunchesByUser)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel Bunches()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.Bunches)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Add([FromBody] AddBunchPostModel post)
        {
            var request = new AddBunch.Request(CurrentUserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
            var bunchResult = UseCase.AddBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}