using System.Web.Http;
using Api.Auth;
using Api.Models.EventModels;
using Core.UseCases;
using PokerBunch.Common.Urls.ApiUrls;

namespace Api.Controllers
{
    public class EventController : BaseController
    {
        [Route(ApiEventUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public EventModel Get(int id)
        {
            var result = UseCase.GetEvent.Execute(new EventDetails.Request(CurrentUserName, id));
            return new EventModel(result);
        }

        [Route(ApiBunchEventsUrl.Route)]
        [HttpGet]
        [ApiAuthorize]
        public EventListModel List(string bunchId)
        {
            var eventListResult = UseCase.GetEventList.Execute(new EventList.Request(CurrentUserName, bunchId));
            return new EventListModel(eventListResult);
        }

        [Route(ApiBunchEventsUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public EventModel Add(string bunchId, [FromBody] EventAddPostModel post)
        {
            var result = UseCase.AddEvent.Execute(new AddEvent.Request(CurrentUserName, bunchId, post.Name));
            return Get(result.Id);
        }
    }
}