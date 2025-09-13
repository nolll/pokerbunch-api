using Api.Auth;
using Api.Extensions;
using Api.Models.CashgameModels;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class DeleteCashgameHandler
{
    public static async Task<IResult> Handle(DeleteCashgame deleteCashgame, IAuth auth, string cashgameId)
    {
        var request = new DeleteCashgame.Request(auth.Principal, cashgameId);
        var result = await deleteCashgame.Execute(request);
        return ResultHandler.Model(result, () => new CashgameDeletedModel(cashgameId));
    }
}