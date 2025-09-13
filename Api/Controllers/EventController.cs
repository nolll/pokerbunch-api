using System.Linq;
using Api.Models.EventModels;
using Api.Routes;
using Api.Settings;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EventController(
    AppSettings appSettings,
    EventDetails eventDetails,
    EventList eventList,
    AddEvent addEvent,
    IAuth auth)
    : BaseController(appSettings)
{
    [Route(ApiRoutes.Event.Get)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("Get event")]
    public async Task<ObjectResult> Get(string eventId)
    {
        var result = await eventDetails.Execute(new EventDetails.Request(auth, eventId));
        return Model(result, () => result.Data is not null ? new EventModel(result.Data) : null);
    }
    
    [Route(ApiRoutes.Event.ListByBunch)]
    [HttpGet]
    [Authorize]
    [EndpointSummary("List events")]
    public async Task<ObjectResult> List(string bunchId)
    {
        var result = await eventList.Execute(new EventList.Request(auth, bunchId));
        return Model(result, () => result.Data?.Events.Select(o => new EventModel(o)));
    }
    
    [Route(ApiRoutes.Event.Add)]
    [HttpPost]
    [Authorize]
    [EndpointSummary("Add event")]
    public async Task<ObjectResult> Add(string bunchId, [FromBody] EventAddPostModel post)
    {
        var result = await addEvent.Execute(new AddEvent.Request(auth, bunchId, post.Name));
        return result.Success 
            ? await Get(result.Data?.Id ?? "") 
            : Error(result.Error);
    }
}