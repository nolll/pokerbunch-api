using Api.Extensions;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Core.Services.Interfaces;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class ResetPasswordHandler
{
    public static async Task<IResult> Handle(
        ResetPassword resetPassword,
        ISiteUrlProvider urls,
        [FromBody] ResetPasswordPostModel post)
    {
        var request = new ResetPassword.Request(post.Email);
        var result = await resetPassword.Execute(request);
        return ResultHandler.Model(result, () => new OkModel());
    }
}