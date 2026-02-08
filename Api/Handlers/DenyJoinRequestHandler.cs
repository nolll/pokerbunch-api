using Api.Extensions;
using Api.Models.CommonModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class DenyJoinRequestHandler
{
    public static async Task<IResult> Handle([FromServices]DenyJoinRequest denyJoinRequest, [FromServices]IAuth auth, string joinRequestId)
    {
        var request = new DenyJoinRequest.Request(auth, joinRequestId);
        var result = await denyJoinRequest.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        MessageModel? CreateModel() => new("Join request denied");
    }
}