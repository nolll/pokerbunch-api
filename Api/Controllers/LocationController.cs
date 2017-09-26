using System.Web.Http;
using Api.Auth;
using Api.Models.LocationModels;
using Core.UseCases;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Controllers
{
    public class LocationController : BaseController
    {
        [Route(ApiLocationUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public LocationModel Get(int id)
        {
            var result = UseCase.GetLocation.Execute(new GetLocation.Request(CurrentUserName, id));
            return new LocationModel(result);
        }

        [Route(ApiBunchLocationsUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public LocationListModel GetList(string slug)
        {
            var locationListResult = UseCase.GetLocationList.Execute(new GetLocationList.Request(CurrentUserName, slug));
            return new LocationListModel(locationListResult);
        }

        [Route(ApiBunchLocationsUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public LocationModel Add(string slug, [FromBody] LocationAddPostModel post)
        {
            var result = UseCase.AddLocation.Execute(new AddLocation.Request(CurrentUserName, slug, post.Name));
            return new LocationModel(result);
        }
    }
}