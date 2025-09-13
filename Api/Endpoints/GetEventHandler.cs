using Api.Extensions;
using Api.Models.EventModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class GetEventHandler
{
    public static async Task<IResult> Handle(EventDetails eventDetails, IAuth auth, string eventId)
    {
        var result = await eventDetails.Execute(new EventDetails.Request(auth, eventId));
        return ResultHandler.Model(result, () => result.Data is not null ? new EventModel(result.Data) : null);
    }
}