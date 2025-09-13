using System.Linq;
using Api.Extensions;
using Api.Models.EventModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetEventListHandler
{
    public static async Task<IResult> Handle(EventList eventList, IAuth auth, string bunchId)
    {
        var result = await eventList.Execute(new EventList.Request(auth, bunchId));
        return ResultHandler.Model(result, () => result.Data?.Events.Select(o => new EventModel(o)));
    }
}