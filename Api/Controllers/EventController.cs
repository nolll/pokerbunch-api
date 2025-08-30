using System.Linq;
using Api.Models.EventModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EventController(
    AppSettings appSettings,
    EventDetails eventDetails,
    EventList eventList,
    AddEvent addEvent)
    : BaseController(appSettings)
{
    /// <summary>
    /// Get an event
    /// </summary>
    [Route(ApiRoutes.Event.Get)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> Get(string eventId)
    {
        var result = await eventDetails.Execute(new EventDetails.Request(CurrentUserName, eventId));
        return Model(result, () => result.Data is not null ? new EventModel(result.Data) : null);
    }

    /// <summary>
    /// List events
    /// </summary>
    [Route(ApiRoutes.Event.ListByBunch)]
    [HttpGet]
    [Authorize]
    public async Task<ObjectResult> List(string bunchId)
    {
        var result = await eventList.Execute(new EventList.Request(AccessControl, bunchId));
        return Model(result, () => result.Data?.Events.Select(o => new EventModel(o)));
    }

    /// <summary>
    /// Add an event
    /// </summary>
    [Route(ApiRoutes.Event.Add)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] EventAddPostModel post)
    {
        var result = await addEvent.Execute(new AddEvent.Request(AccessControl, bunchId, post.Name));
        return result.Success 
            ? await Get(result.Data?.Id ?? "") 
            : Error(result.Error);
    }
}