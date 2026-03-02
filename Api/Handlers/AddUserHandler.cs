using Api.Extensions;
using Api.Models.CommonModels;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;
using Core.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public static class AddUserHandler
{
    public static async Task<IResult> Handle(AddUser addUser, [FromBody] AddUserPostModel post)
    {
        var result = await addUser.Execute(new AddUser.Request(post.UserName, post.DisplayName, post.Email, post.Password));
        return ResultHandler.Model(result, () => new OkModel());    
    }
}