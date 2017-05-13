using System.Web.Http;
using Api.Auth;
using Api.Models.EventModels;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class EventController : BaseController
    {
        [Route(ApiRoutes.EventGet)]
        [HttpGet]
        [ApiAuthorize]
        public EventModel Get(int id)
        {
            var result = UseCase.GetEvent.Execute(new EventDetails.Request(CurrentUserName, id));
            return new EventModel(result);
        }

        [Route(ApiRoutes.EventList)]
        [HttpGet]
        [ApiAuthorize]
        public EventListModel List(string slug)
        {
            var eventListResult = UseCase.GetEventList.Execute(new EventList.Request(CurrentUserName, slug));
            return new EventListModel(eventListResult);
        }
    }
}