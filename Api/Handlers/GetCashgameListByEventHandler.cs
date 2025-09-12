using System.Linq;
using Api.Auth;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetCashgameListByEventHandler
{
    public static async Task<IResult> Handle(EventCashgameList eventCashgameList, IAuth auth, string eventId)
    {
        var result = await eventCashgameList.Execute(new EventCashgameList.Request(auth.Principal, eventId));
        return ResultHandler.Model(result, () => result.Data?.Items.Select(o => new CashgameListItemModel(o)));
    }
}