using Api.Extensions;
using Api.Models.CommonModels;
using Api.Settings;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;

namespace Api.Handlers;

public static class SendEmailHandler
{
    public static async Task<IResult> Handle(TestEmail testEmail, IAuth auth, AppSettings appSettings)
    {
        var result = await testEmail.Execute(new TestEmail.Request(auth, appSettings.Email.TestRecipient));
        return ResultHandler.Model(result, () => new MessageModel(result.Data?.Message));
    }
}