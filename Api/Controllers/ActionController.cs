using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Api.Routes;
using Api.Settings;
using Core.Errors;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ActionController(
    AppSettings appSettings,
    Buyin buyin,
    Report report,
    Cashout cashout,
    EditCheckpoint editCheckpoint,
    DeleteCheckpoint deleteCheckpoint)
    : BaseController(appSettings)
{
    /// <summary>
    /// Add an action to a cashgame
    /// </summary>
    [Route(ApiRoutes.Action.Add)]
    [HttpPost]
    [Authorize]
    public async Task<ObjectResult> Add(string cashgameId, [FromBody] AddCashgameActionPostModel post)
    {
        return post.Type switch
        {
            ActionType.Buyin => await Buyin(cashgameId, post),
            ActionType.Report => await Report(cashgameId, post),
            ActionType.Cashout => await Cashout(cashgameId, post),
            _ => Error(ErrorType.NotFound,
                $"Action type not found. Valid types are [{ActionType.Buyin}], [{ActionType.Report}] and [{ActionType.Cashout}]")
        };
    }

    private async Task<ObjectResult> Buyin(string cashgameId, AddCashgameActionPostModel post)
    {
        var result = await buyin.Execute(new Buyin.Request(CurrentUserName, cashgameId, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
        return Model(result, () => new OkModel());
    }

    private async Task<ObjectResult> Report(string cashgameId, AddCashgameActionPostModel post)
    {
        var result = await report.Execute(new Report.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return Model(result, () => new OkModel());
    }

    private async Task<ObjectResult> Cashout(string cashgameId, AddCashgameActionPostModel post)
    {
        var result = await cashout.Execute(new Cashout.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Update an action
    /// </summary>
    [Route(ApiRoutes.Action.Update)]
    [HttpPut]
    [Authorize]
    public async Task<ObjectResult> UpdateAction(string cashgameId, string actionId, [FromBody] UpdateActionPostModel post)
    {
        var result = await editCheckpoint.Execute(new EditCheckpoint.Request(CurrentUserName, actionId, post.Timestamp, post.Stack, post.Added));
        return Model(result, () => new OkModel());
    }

    /// <summary>
    /// Delete an action
    /// </summary>
    [Route(ApiRoutes.Action.Delete)]
    [HttpDelete]
    [Authorize]
    public async Task<ObjectResult> DeleteAction(string cashgameId, string actionId)
    {
        var result = await deleteCheckpoint.Execute(new DeleteCheckpoint.Request(CurrentUserName, actionId));
        return Model(result, () => new OkModel());
    }

    private static class ActionType
    {
        public const string Buyin = "buyin";
        public const string Report = "report";
        public const string Cashout = "cashout";
    }
}