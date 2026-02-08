using Api.Extensions;
using Api.Models.JoinRequestModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddJoinRequestHandler
{
    public static async Task<IResult> Handle(
        [FromServices]AddJoinRequest addJoinRequest, 
        [FromServices]IAuth auth, 
        string bunchId)
    {
        var request = new AddJoinRequest.Request(auth, bunchId);
        var result = await addJoinRequest.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        JoinRequestSentModel? CreateModel() => result.Data?.BunchName is not null ? new JoinRequestSentModel(result.Data.BunchName) : null;
    }
}