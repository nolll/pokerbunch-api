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
    public async Task<ObjectResult> Get(int eventId)
    {
        var result = await _eventDetails.Execute(new EventDetails.Request(CurrentUserName, eventId));
        return Model(result, () => new EventModel(result.Data));
    }

    [Route(ApiRoutes.Event.ListByBunch)]
    [HttpGet]
    [ApiAuthorize]
    public async Task<ObjectResult> List(string bunchId)
    {
        var result = await _eventList.Execute(new EventList.Request(CurrentUserName, bunchId));
        return Model(result, () => result.Data.Events.Select(o => new EventModel(o)));
    }

    [Route(ApiRoutes.Event.Add)]
    [HttpPost]
    [ApiAuthorize]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] EventAddPostModel post)
    {
        var result = await _addEvent.Execute(new AddEvent.Request(CurrentUserName, bunchId, post.Name));
        return result.Success 
            ? await Get(result.Data.Id) 
            : Error(result.Error);
    }
}