using Api.Auth;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Routes;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BunchController : BaseController
    {
        public BunchController(Settings settings) : base(settings)
        {
        }

        [Route(ApiRoutes.Bunch.Get)]
        [HttpGet]
        [ApiAuthorize]
        public BunchModel Get(string bunchId)
        {
            var request = new GetBunch.Request(CurrentUserName, bunchId);
            var bunchResult = UseCase.GetBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.Bunch.Get)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Update(string bunchId, [FromBody] UpdateBunchPostModel post)
        {
            var request = new EditBunch.Request(CurrentUserName, bunchId, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
            var bunchResult = UseCase.EditBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.Bunch.List)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel List()
        {
            var request = new GetBunchList.AllBunchesRequest(CurrentUserName);
            var bunchListResult = UseCase.GetBunchList.Execute(request);
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.Bunch.ListForCurrentUser)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel Bunches()
        {
            var bunchListResult = UseCase.GetBunchList.Execute(new GetBunchList.UserBunchesRequest(CurrentUserName));
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.Bunch.List)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Add([FromBody] AddBunchPostModel post)
        {
            var request = new AddBunch.Request(CurrentUserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
            var bunchResult = UseCase.AddBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.Bunch.Join)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerJoinedModel Join(string bunchId, [FromBody] JoinBunchPostModel post)
        {
            var request = new JoinBunch.Request(CurrentUserName, bunchId, post.Code);
            var result = UseCase.JoinBunch.Execute(request);
            return new PlayerJoinedModel(result.PlayerId);
        }
    }
}