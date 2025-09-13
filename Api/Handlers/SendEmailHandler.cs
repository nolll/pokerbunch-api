using Api.Extensions;
using Api.Models.CommonModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class SendEmailHandler
{
    public static async Task<IResult> Handle(TestEmail testEmail, IAuth auth)
    {
        var result = await testEmail.Execute(new TestEmail.Request(auth));
        return ResultHandler.Model(result, () => new MessageModel(result.Data?.Message));
    }
}