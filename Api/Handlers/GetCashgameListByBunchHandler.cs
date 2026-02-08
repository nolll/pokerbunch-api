using System.Collections.Generic;
using System.Linq;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetCashgameListByBunchHandler
{
    public static async Task<IResult> Handle(CashgameList cashgameList, IAuth auth, string bunchId)
    {
        var result = await cashgameList.Execute(new CashgameList.Request(auth, bunchId, null));
        return ResultHandler.Model(result, CreateModel);
        IEnumerable<CashgameListItemModel>? CreateModel() => result.Data?.Items.Select(o => new CashgameListItemModel(o));
    }
}