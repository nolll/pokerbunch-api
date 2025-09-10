using System.Security.Claims;
using Api.Auth;
using Api.Extensions;
using Api.Models.CommonModels;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class SendEmailHandler
{
    public static async Task<IResult> Handle(TestEmail testEmail, ClaimsPrincipal user)
    {
        var result = await testEmail.Execute(new TestEmail.Request(new AuthWrapper(user).Principal));
        return ResultHandler.Model(result, () => new MessageModel(result.Data?.Message));
    }
}