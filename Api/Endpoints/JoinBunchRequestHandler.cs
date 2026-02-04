using Api.Extensions;
using Api.Models.BunchModels;
using Api.Models.PlayerModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class JoinBunchRequestHandler
{
    public static async Task<IResult> Handle(JoinBunchRequest joinBunchRequest, IAuth auth, string bunchId)
    {
        var request = new JoinBunchRequest.Request(auth, bunchId);
        var result = await joinBunchRequest.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        PlayerJoinedModel? CreateModel() => result.Data?.PlayerId is not null ? new PlayerJoinedModel(result.Data.PlayerId) : null;
    }
}