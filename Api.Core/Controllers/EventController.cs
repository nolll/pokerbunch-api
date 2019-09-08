using Api.Auth;
using Api.Models.EventModels;
using Api.Routes;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class EventController : BaseController
    {
        public EventController(Settings settings) : base(settings)
        {
        }

        [Route(ApiRoutes.Event.Get)]
        [HttpGet]
        [ApiAuthorize]
        public EventModel Get(int eventId)
        {
            var result = UseCase.GetEvent.Execute(new EventDetails.Request(CurrentUserName, eventId));
            return new EventModel(result);
        }

        [Route(ApiRoutes.Event.ListByBunch)]
        [HttpGet]
        [ApiAuthorize]
        public EventListModel List(string bunchId)
        {
            var eventListResult = UseCase.GetEventList.Execute(new EventList.Request(CurrentUserName, bunchId));
            return new EventListModel(eventListResult);
        }

        [Route(ApiRoutes.Event.ListByBunch)]
        [HttpPost]
        [ApiAuthorize]
        public EventModel Add(string bunchId, [FromBody] EventAddPostModel post)
        {
            var result = UseCase.AddEvent.Execute(new AddEvent.Request(CurrentUserName, bunchId, post.Name));
            return Get(result.Id);
        }
    }
}