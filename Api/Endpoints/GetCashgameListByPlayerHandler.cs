using System.Linq;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints;

public static class GetCashgameListByPlayerHandler
{
    public static async Task<IResult> Handle(PlayerCashgameList playerCashgameList, IAuth auth, string playerId)
    {
        var result = await playerCashgameList.Execute(new PlayerCashgameList.Request(auth, playerId));
        return ResultHandler.Model(result, () => result.Data?.Items.Select(o => new CashgameListItemModel(o)));
    }
}