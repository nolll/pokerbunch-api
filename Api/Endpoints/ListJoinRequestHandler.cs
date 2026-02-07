using System.Collections.Generic;
using System.Linq;
using Api.Extensions;
using Api.Models.JoinRequestModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class ListJoinRequestHandler
{
    public static async Task<IResult> Handle([FromServices]ListJoinRequests listJoinRequest, [FromServices]IAuth auth, string bunchId)
    {
        var request = new ListJoinRequests.Request(auth, bunchId);
        var result = await listJoinRequest.Execute(request);
        return ResultHandler.Model(result, CreateModel);
        IEnumerable<JoinRequestModel>? CreateModel() => 
            result.Data?.JoinRequests.Select(o => new JoinRequestModel(o.Id, bunchId, o.UserId, o.UserName));
    }
}