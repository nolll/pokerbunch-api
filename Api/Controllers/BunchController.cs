using System.Web.Http;
using Api.Auth;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class BunchController : BaseController
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

        [Route(ApiRoutes.BunchList)]
        [HttpGet]
        [ApiAuthorize]
        public BunchListModel List()
        {
            var request = new GetBunchList.AllBunchesRequest(CurrentUserName);
            var bunchListResult = UseCase.GetBunchList.Execute(request);
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.BunchAdd)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Add([FromBody] AddBunchPostModel post)
        {
            var request = new AddBunch.Request(CurrentUserName, post.Name, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone);
            var bunchResult = UseCase.AddBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.BunchUpdate)]
        [HttpPost]
        [ApiAuthorize]
        public BunchModel Update(string slug, [FromBody] UpdateBunchPostModel post)
        {
            var request = new EditBunch.Request(CurrentUserName, slug, post.Description, post.CurrencySymbol, post.CurrencyLayout, post.Timezone, post.HouseRules, post.DefaultBuyin);
            var bunchResult = UseCase.EditBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.BunchJoin)]
        [HttpPost]
        [ApiAuthorize]
        public PlayerJoinedModel Join(string slug, [FromBody] JoinBunchPostModel post)
        {
            var request = new JoinBunch.Request(CurrentUserName, slug, post.Code);
            var result = UseCase.JoinBunch.Execute(request);
            return new PlayerJoinedModel(result.PlayerId);
        }
    }
}