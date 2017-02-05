using System.Web.Http;
using Api.Auth;
using Api.Models;
using Api.Routes;
using Core.UseCases;

namespace Api.Controllers
{
    public class EventController : BaseApiController
    {
        [Route(ApiRoutes.EventList)]
        [HttpGet]
        [ApiAuthorize]
        public EventListModel GetList(string slug)
        {
            var eventListResult = UseCase.GetEventList.Execute(new EventList.Request(CurrentUserName, slug));
            return new EventListModel(eventListResult);
        }
    }
}