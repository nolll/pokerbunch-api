using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Models.BunchModels;
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
        public BunchModel Add([FromBody] AddBunchPostModel b)
        {
            var request = new AddBunch.Request(CurrentUserName, b.Name, b.Description, b.CurrencySymbol, b.CurrencyLayout, b.Timezone);
            var bunchResult = UseCase.AddBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

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