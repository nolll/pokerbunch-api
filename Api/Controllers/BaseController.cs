﻿using Api.Models.CommonModels;
using Api.Settings;
using Core.UseCases;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Core;
using Core.Errors;
using Environment = Api.Services.Environment;

namespace Api.Controllers;

[UsedImplicitly]
public abstract class BaseController(AppSettings appSettings) : Controller
{
    protected AppSettings AppSettings { get; } = appSettings;

    protected string CurrentUserName
    {
        get
        {
            if (User.Identity is null)
                throw new PokerBunchException("Auth failed: No identity");

            if (User.Identity.IsAuthenticated)
            {
                if(User.Identity.Name is null)
                    throw new PokerBunchException("Auth failed: No identity");

                return User.Identity.Name;
            }
                
            var env = new Environment(Request.Host.Host);
            if (AppSettings.Auth.Override.Enabled && env.IsDevModeAdmin)
                return AppSettings.Auth.Override.AdminUserName;

            if (AppSettings.Auth.Override.Enabled && env.IsDevModePlayer)
                return AppSettings.Auth.Override.PlayerUserName;

            throw new PokerBunchException("Auth failed: Not authenticated");
        }
    }

    protected ObjectResult Model<T>(UseCaseResult<T> result, Func<object?> create)
    {
        return result.Success
            ? Success(create())
            : Error(result.Error);
    }

    protected ObjectResult Success(object? model)
    {
        return Ok(model);
    }

    protected ObjectResult Error(UseCaseError? error)
    {
        return error is not null
            ? Error(error.Type, error.Message)
            : Error(ErrorType.Unknown, "Unknown error");
    }

    protected ObjectResult Error(ErrorType errorType, string errorMessage)
    {
        var statusCode = GetStatusCode(errorType);
        var messageModel = new ErrorModel(errorMessage);

        return StatusCode((int)statusCode, messageModel);
    }

    private HttpStatusCode GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Auth => HttpStatusCode.Unauthorized,
            ErrorType.AccessDenied => HttpStatusCode.Forbidden,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };
    }
}