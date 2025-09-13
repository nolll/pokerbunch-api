using Api.Extensions;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class ChangePasswordHandler
{
    public static async Task<IResult> Handle(
        ChangePassword changePassword,
        IAuth auth,
        [FromBody] ChangePasswordPostModel post)
    {
        var request = new ChangePassword.Request(auth.UserName, post.NewPassword, post.OldPassword);
        var result = await changePassword.Execute(request);
        return ResultHandler.Model(result, () => new OkModel());
    }
}