using Api.Extensions;
using Api.Models.UserModels;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class UpdateUserHandler
{
    public static async Task<IResult> Handle(
        EditUser editUser,
        UserDetails userDetails,
        string userName, 
        [FromBody] UpdateUserPostModel post)
    {
        var updateRequest = new EditUser.Request(userName, post.DisplayName, post.RealName, post.Email);
        var updateResult = await editUser.Execute(updateRequest);
        if (!updateResult.Success)
            return ResultHandler.Error(updateResult.Error);

        var result = await userDetails.Execute(new UserDetails.Request(updateResult.Data!.UserName));
        return ResultHandler.Model(result, () => result.Data is not null ? new FullUserModel(result.Data) : null);
    }
}