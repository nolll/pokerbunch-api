using Api.Extensions;
using Api.Models;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Core.Errors;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddActionHandler
{
    public static async Task<IResult> Handle(
        Buyin buyin,
        Report report,
        Cashout cashout,
        IAuth auth,
        string cashgameId,
        [FromBody] AddCashgameActionPostModel post)
    {
        return post.Type switch
        {
            ActionType.Buyin => await Buyin(buyin, auth, cashgameId, post),
            ActionType.Report => await Report(report, auth, cashgameId, post),
            ActionType.Cashout => await Cashout(cashout, auth, cashgameId, post),
            _ => ResultHandler.Error(ErrorType.NotFound,
                $"Action type not found. Valid types are [{ActionType.Buyin}], [{ActionType.Report}] and [{ActionType.Cashout}]")
        };
    }
    
    private static async Task<IResult> Buyin(
        Buyin buyin, 
        IAuth auth, 
        string cashgameId, 
        AddCashgameActionPostModel post)
    {
        var result = await buyin.Execute(new Buyin.Request(auth, cashgameId, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
        return ResultHandler.Model(result, () => new OkModel());
    }

    private static async Task<IResult> Report(
        Report report, 
        IAuth auth, 
        string cashgameId, 
        AddCashgameActionPostModel post)
    {
        var result = await report.Execute(new Report.Request(auth, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return ResultHandler.Model(result, () => new OkModel());
    }

    private static async Task<IResult> Cashout(
        Cashout cashout, 
        IAuth auth, 
        string cashgameId, 
        AddCashgameActionPostModel post)
    {
        var result = await cashout.Execute(new Cashout.Request(auth, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return ResultHandler.Model(result, () => new OkModel());
    }
}