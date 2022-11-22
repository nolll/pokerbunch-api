using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Api.Routes;
using Api.Settings;
using Core.Errors;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ActionController : BaseController
{
    private readonly Buyin _buyin;
    private readonly Report _report;
    private readonly Cashout _cashout;
    private readonly EditCheckpoint _editCheckpoint;
    private readonly DeleteCheckpoint _deleteCheckpoint;

    public ActionController(
        AppSettings appSettings,
        Buyin buyin,
        Report report,
        Cashout cashout,
        EditCheckpoint editCheckpoint,
        DeleteCheckpoint deleteCheckpoint) 
        : base(appSettings)
    {
        _buyin = buyin;
        _report = report;
        _cashout = cashout;
        _editCheckpoint = editCheckpoint;
        _deleteCheckpoint = deleteCheckpoint;
    }

    [Route(ApiRoutes.Action.Add)]
    [HttpPost]
    [ApiAuthorize]
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
        var result = await _buyin.Execute(new Buyin.Request(CurrentUserName, cashgameId, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
        return Model(result, () => new OkModel());
    }

    private async Task<ObjectResult> Report(string cashgameId, AddCashgameActionPostModel post)
    {
        var result = await _report.Execute(new Report.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return Model(result, () => new OkModel());
    }

    private async Task<ObjectResult> Cashout(string cashgameId, AddCashgameActionPostModel post)
    {
        var result = await _cashout.Execute(new Cashout.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return Model(result, () => new OkModel());
    }

    [Route(ApiRoutes.Action.Update)]
    [HttpPut]
    [ApiAuthorize]
    public async Task<ObjectResult> UpdateAction(string cashgameId, string actionId, [FromBody] UpdateActionPostModel post)
    {
        var result = await _editCheckpoint.Execute(new EditCheckpoint.Request(CurrentUserName, actionId, post.Timestamp, post.Stack, post.Added));
        return Model(result, () => new OkModel());
    }

    [Route(ApiRoutes.Action.Delete)]
    [HttpDelete]
    [ApiAuthorize]
    public async Task<ObjectResult> DeleteAction(string cashgameId, string actionId)
    {
        var result = await _deleteCheckpoint.Execute(new DeleteCheckpoint.Request(CurrentUserName, actionId));
        return Model(result, () => new OkModel());
    }

    private static class ActionType
    {
        public const string Buyin = "buyin";
        public const string Report = "report";
        public const string Cashout = "cashout";
    }
}