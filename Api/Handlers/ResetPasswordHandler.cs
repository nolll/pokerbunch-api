using Api.Extensions;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class ResetPasswordHandler
{
    public static async Task<IResult> Handle(
        ResetPassword resetPassword,
        UrlProvider urls,
        [FromBody] ResetPasswordPostModel post)
    {
        var request = new ResetPassword.Request(post.Email, urls.Site.Login);
        var result = await resetPassword.Execute(request);
        return ResultHandler.Model(result, () => new OkModel());
    }
}