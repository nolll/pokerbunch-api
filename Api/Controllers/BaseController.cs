using System.Linq;
using Api.Models.CommonModels;
using Api.Settings;
using Core.UseCases;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Api.Auth;
using Core;
using Core.Entities;
using Core.Errors;
using Newtonsoft.Json;
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
            return AppSettings.Auth.Override.Enabled switch
            {
                true when env.IsDevModeAdmin => AppSettings.Auth.Override.AdminUserName,
                true when env.IsDevModePlayer => AppSettings.Auth.Override.PlayerUserName,
                _ => throw new PokerBunchException("Auth failed: Not authenticated")
            };
        }
    }

    protected CurrentUser CurrentUser => new(CurrentUserId, CurrentUserName, CurrentUserDisplayName, IsAdmin);

    private bool IsAdmin => GetBoolClaim(CustomClaimTypes.IsAdmin);
    private string CurrentUserId => GetClaim(CustomClaimTypes.UserId) ?? "";
    private string CurrentUserDisplayName => GetClaim(CustomClaimTypes.UserDisplayName) ?? "";
    
    private TokenBunch[] UserBunches => JsonConvert.DeserializeObject<TokenBunch[]>(GetClaim(CustomClaimTypes.Bunches) ?? "") ?? [];

    protected CurrentBunch CurrentBunch(string id)
    {
        var b = UserBunches.First(o => o.Id == id);
        return new CurrentBunch(b.Id, b.Name, b.PlayerId, b.PlayerName, b.Role);
    }
    
    private string? GetClaim(string type) => User.Claims.FirstOrDefault(o => o.Type == type)?.Value;

    private bool GetBoolClaim(string type)
    {
        var claim = GetClaim(type);
        return claim is not null && bool.Parse(claim);
    }

    protected ObjectResult Model<T>(UseCaseResult<T> result, Func<object?> create) => result.Success
        ? Success(create())
        : Error(result.Error);

    protected ObjectResult Success(object? model) => Ok(model);

    protected ObjectResult Error(UseCaseError? error) => error is not null
        ? Error(error.Type, error.Message)
        : Error(ErrorType.Unknown, "Unknown error");

    protected ObjectResult Error(ErrorType errorType, string errorMessage)
    {
        var statusCode = GetStatusCode(errorType);
        var messageModel = new ErrorModel(errorMessage);

        return StatusCode((int)statusCode, messageModel);
    }

    private HttpStatusCode GetStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => HttpStatusCode.BadRequest,
        ErrorType.NotFound => HttpStatusCode.NotFound,
        ErrorType.Auth => HttpStatusCode.Unauthorized,
        ErrorType.AccessDenied => HttpStatusCode.Forbidden,
        ErrorType.Conflict => HttpStatusCode.Conflict,
        _ => HttpStatusCode.InternalServerError
    };
}