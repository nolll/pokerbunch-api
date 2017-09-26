using System.Web.Http;
using Api.Auth;
using Api.Models.EventModels;
using Api.Routes;
using Core.UseCases;

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
        public EventListModel List(string slug)
        {
            var eventListResult = UseCase.GetEventList.Execute(new EventList.Request(CurrentUserName, slug));
            return new EventListModel(eventListResult);
        }

        [Route(ApiBunchEventsUrl.Route)]
        [HttpPost]
        [ApiAuthorize]
        public EventModel Add(string slug, [FromBody] EventAddPostModel post)
        {
            var result = UseCase.AddEvent.Execute(new AddEvent.Request(CurrentUserName, slug, post.Name));
            return Get(result.Id);
        }
    }
}