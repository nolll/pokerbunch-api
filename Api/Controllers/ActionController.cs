using System;
using Api.Auth;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Api.Routes;
using Api.Settings;
using Core.Exceptions;
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

    [Route(ApiRoutes.Action.List)]
    [HttpPost]
    [ApiAuthorize]
    public OkModel Add(int cashgameId, [FromBody] AddCashgameActionPostModel post)
    {
        if(post.Type == ActionType.Buyin)
            return Buyin(cashgameId, post);
        if (post.Type == ActionType.Report)
            return Report(cashgameId, post);
        if(post.Type == ActionType.Cashout)
            return Cashout(cashgameId, post);

        throw new NotFoundException($"Action type not found. Valid types are [{ActionType.Buyin}], [{ActionType.Report}] and [{ActionType.Cashout}]");
    }

    private OkModel Buyin(int cashgameId, AddCashgameActionPostModel post)
    {
        _buyin.Execute(new Buyin.Request(CurrentUserName, cashgameId, post.PlayerId, post.Added, post.Stack, DateTime.UtcNow));
        return new OkModel();
    }

    private OkModel Report(int cashgameId, AddCashgameActionPostModel post)
    {
        _report.Execute(new Report.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return new OkModel();
    }

    private OkModel Cashout(int cashgameId, AddCashgameActionPostModel post)
    {
        _cashout.Execute(new Cashout.Request(CurrentUserName, cashgameId, post.PlayerId, post.Stack, DateTime.UtcNow));
        return new OkModel();
    }

    [Route(ApiRoutes.Action.Get)]
    [HttpPut]
    [ApiAuthorize]
    public OkModel UpdateAction(int cashgameId, int actionId, [FromBody] UpdateActionPostModel post)
    {
        _editCheckpoint.Execute(new EditCheckpoint.Request(CurrentUserName, actionId, post.Timestamp, post.Stack, post.Added));
        return new OkModel();
    }

    [Route(ApiRoutes.Action.Get)]
    [HttpDelete]
    [ApiAuthorize]
    public OkModel DeleteAction(int cashgameId, int actionId)
    {
        _deleteCheckpoint.Execute(new DeleteCheckpoint.Request(CurrentUserName, actionId));
        return new OkModel();
    }

    private static class ActionType
    {
        public const string Buyin = "buyin";
        public const string Report = "report";
        public const string Cashout = "cashout";
    }
}