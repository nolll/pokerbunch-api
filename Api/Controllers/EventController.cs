using System.Collections.Generic;
using System.Linq;
using Api.Auth;
using Api.Models.EventModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EventController : BaseController
{
    private readonly EventDetails _eventDetails;
    private readonly EventList _eventList;
    private readonly AddEvent _addEvent;

    public EventController(
        AppSettings appSettings,
        EventDetails eventDetails,
        EventList eventList,
        AddEvent addEvent) : base(appSettings)
    {
        _eventDetails = eventDetails;
        _eventList = eventList;
        _addEvent = addEvent;
    }

    [Route(ApiRoutes.Event.Get)]
    [HttpGet]
    [ApiAuthorize]
    public EventModel Get(int eventId)
    {
        var result = _eventDetails.Execute(new EventDetails.Request(CurrentUserName, eventId));
        return new EventModel(result);
    }

    [Route(ApiRoutes.Event.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public IEnumerable<EventModel> List(string bunchId)
    {
        var eventListResult = _eventList.Execute(new EventList.Request(CurrentUserName, bunchId));
        return eventListResult.Events.Select(o => new EventModel(o));
    }

    [Route(ApiRoutes.Event.ListByBunch)]
    [HttpPost]
    [ApiAuthorize]
    public EventModel Add(string bunchId, [FromBody] EventAddPostModel post)
    {
        var result = _addEvent.Execute(new AddEvent.Request(CurrentUserName, bunchId, post.Name));
        return Get(result.Id);
    }
}