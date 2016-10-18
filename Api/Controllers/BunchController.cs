using System.Web.Http;
using Api.Auth;
using Api.Models;
using Core.UseCases;
using Web.Common.Routes;

namespace Api.Controllers
{
    public class BunchController : BaseApiController
    {
        [Route(ApiRoutes.BunchList)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public BunchListModel List()
        {
            var request = new GetBunchList.AllBunchesRequest(CurrentUserName);
            var bunchListResult = UseCase.GetBunchList.Execute(request);
            return new BunchListModel(bunchListResult);
        }

        [Route(ApiRoutes.BunchGet)]
        [AcceptVerbs("GET")]
        [ApiAuthorize]
        public BunchModel Get(string slug)
        {
            var request = new GetBunch.Request(CurrentUserName, slug);
            var bunchResult = UseCase.GetBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.BunchList)]
        [AcceptVerbs("POST")]
        [ApiAuthorize]
        public BunchModel Add([FromBody] BunchModel b)
        {
            var request = new AddBunch.Request(CurrentUserName, b.Name, b.Description, b.CurrencySymbol, b.CurrencyLayout, b.Timezone);
            var bunchResult = UseCase.AddBunch.Execute(request);
            return new BunchModel(bunchResult);
        }

        [Route(ApiRoutes.BunchGet)]
        [AcceptVerbs("POST")]
        [ApiAuthorize]
        public BunchModel Save(string slug, [FromBody] BunchModel b)
        {
            var request = new EditBunch.Request(CurrentUserName, slug, b.Description, b.CurrencySymbol, b.CurrencyLayout, b.Timezone, b.HouseRules, b.DefaultBuyin);
            var bunchResult = UseCase.EditBunch.Execute(request);
            return new BunchModel(bunchResult);
        }
    }
}
