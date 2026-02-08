using Api.Extensions;
using Api.Models.UserModels;
using Core.Services;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class UpdateUserHandler
{
    public static async Task<IResult> Handle(
        EditUser editUser,
        UserDetails userDetails,
        IAuth auth,
        string userName, 
        [FromBody] UpdateUserPostModel post)
    {
        var updateRequest = new EditUser.Request(userName, post.DisplayName, post.RealName, post.Email);
        var updateResult = await editUser.Execute(updateRequest);
        if (!updateResult.Success)
            return ResultHandler.Error(updateResult.Error);

        var result = await userDetails.Execute(new UserDetails.Request(auth, updateResult.Data!.UserName));
        return ResultHandler.Model(result, () => result.Data is not null ? new FullUserModel(result.Data) : null);
    }
}