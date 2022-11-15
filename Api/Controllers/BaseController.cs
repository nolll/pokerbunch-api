using Api.Models.CommonModels;
using Api.Services;
using Api.Settings;
using Core.UseCases;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Core.Errors;
using System;
using Environment = Api.Services.Environment;

namespace Api.Controllers;

[UsedImplicitly]
public abstract class BaseController : Controller
{
    protected AppSettings AppSettings { get; }

    protected BaseController(AppSettings appSettings)
    {
        AppSettings = appSettings;
    }

    protected string CurrentUserName
    {
        get
        {
            if (User?.Identity == null)
                return null;
            if (User.Identity.IsAuthenticated)
                return User.Identity.Name;
            var env = new Environment(Request.Host.Host);
            if (AppSettings.Auth.Override.Enabled && env.IsDevModeAdmin)
                return AppSettings.Auth.Override.AdminUserName;
            if (AppSettings.Auth.Override.Enabled && env.IsDevModePlayer)
                return AppSettings.Auth.Override.PlayerUserName;
            return null;
        }
    }

    protected ObjectResult Model<T>(UseCaseResult<T> result, Func<object> create)
    {
        return result.Success
            ? Success(create())
            : Error(result.Error);
    }

    protected ObjectResult Success(object model)
    {
        return Ok(model);
    }

    protected ObjectResult Error(UseCaseError error)
    {
        return Error(error.Type, error.Message);
    }

    protected ObjectResult Error(ErrorType errorType, string errorMessage)
    {
        var statusCode = GetStatusCode(errorType);
        var messageModel = new ErrorModel(errorMessage);

        return StatusCode((int)statusCode, messageModel);
    }

    protected HttpStatusCode GetStatusCode(ErrorType errorType)
    {
        if (errorType == ErrorType.NotFound)
            return HttpStatusCode.NotFound;

        if (errorType == ErrorType.AccessDenied)
            return HttpStatusCode.Forbidden;

        return HttpStatusCode.InternalServerError;
    }
}