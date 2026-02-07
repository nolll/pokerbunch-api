using Api.Extensions;
using Api.Models.CommonModels;
using Api.Models.JoinRequestModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class AcceptJoinRequestHandler
{
    public static async Task<IResult> Handle([FromServices]AcceptJoinRequest acceptJoinRequest, [FromServices]IAuth auth, string joinRequestId)
    {
        var request = new AcceptJoinRequest.Request(auth, joinRequestId);
        var result = await acceptJoinRequest.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        MessageModel? CreateModel() => new("Join request accepted");
    }
}