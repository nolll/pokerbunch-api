using System.Collections.Generic;
using System.Linq;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class GetCashgameListByBunchAndYearHandler
{
    public static async Task<IResult> Handle(CashgameList cashgameList, IAuth auth, string bunchId, int year)
    {
        var result = await cashgameList.Execute(new CashgameList.Request(auth, bunchId, year));
        return ResultHandler.Model(result, CreateModel);
        IEnumerable<CashgameListItemModel>? CreateModel() => result.Data?.Items.Select(o => new CashgameListItemModel(o));
    }
}