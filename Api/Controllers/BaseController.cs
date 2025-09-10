using System.Linq;
using Api.Models.CommonModels;
using Api.Settings;
using Core.UseCases;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Api.Auth;
using Api.Models;
using Core;
using Core.Entities;
using Core.Errors;
using Core.Services;
using Environment = Api.Services.Environment;

namespace Api.Controllers;

[UsedImplicitly]
public abstract class BaseController(AppSettings appSettings) : Controller
{
    private static readonly DateTime TokenMinDate = DateTime.Parse("2025-08-31");
    
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

                if (IsTokenTooOld)
                    throw new PokerBunchException("Token too old");

                return User.Identity.Name;
            }

            throw new PokerBunchException("Auth failed: Not authenticated");
        }
    }

    private bool IsTokenTooOld => 
        DateTimeService.FromUnixTimeStamp(int.Parse(GetClaim(CustomClaimTypes.IssuedAt) ?? "0")) < TokenMinDate;

    private bool IsAdmin => GetBoolClaim(CustomClaimTypes.IsAdmin);
    private string CurrentUserId => GetClaim(CustomClaimTypes.UserId) ?? "";
    private string CurrentUserDisplayName => GetClaim(CustomClaimTypes.UserDisplayName) ?? "";
    
    private TokenBunchModel[] UserBunches
    {
        get
        {
            var value = GetClaim(CustomClaimTypes.Bunches);
            if (value is null or "")
                return [];

            if (value.StartsWith('{'))
                return [JsonSerializer.Deserialize<TokenBunchModel>(value)!];
            
            return JsonSerializer.Deserialize<TokenBunchModel[]>(value) ?? [];
        }
    }

    protected IPrincipal Principal => new Principal(CurrentUserId, CurrentUserName, CurrentUserDisplayName, IsAdmin, UserBunches.Select(ToCurrentBunch).ToArray());

    private static CurrentBunch ToCurrentBunch(TokenBunchModel b)
    {
        return new CurrentBunch(b.Id, b.Slug, b.Name, b.PlayerId, b.PlayerName, Enum.Parse<Role>(b.Role, true));
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