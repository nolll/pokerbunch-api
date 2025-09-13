using Api.Extensions;
using Api.Models.EventModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class AddEventHandler
{
    public static async Task<IResult> Handle(
        AddEvent addEvent,
        EventDetails eventDetails,
        IAuth auth,
        string bunchId,
        [FromBody] EventAddPostModel post)
    {
        var result = await addEvent.Execute(new AddEvent.Request(auth, bunchId, post.Name));
        return result.Success 
            ? await GetEventHandler.Handle(eventDetails, auth, result.Data?.Id ?? "") 
            : ResultHandler.Error(result.Error);
    }
}