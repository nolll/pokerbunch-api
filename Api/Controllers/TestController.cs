using Api.Settings;
using Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class TestController(AppSettings appSettings) : BaseController(appSettings)
{
    [Route("test/exception")]
    [HttpGet]
    public ObjectResult Exception() => throw new Exception("exception");

    [Route("test/unexpected")]
    [HttpGet]
    public ObjectResult Unexpected() => Error(ErrorType.Unknown, "unknown");

    [Route("test/notfound")]
    [HttpGet]
    public ObjectResult NotFoundError() => Error(ErrorType.NotFound, "not found");

    [Route("test/accessdenied")]
    [HttpGet]
    public ObjectResult AccessDeniedError() => Error(ErrorType.AccessDenied, "access denied");

    [Route("test/auth")]
    [HttpGet]
    public ObjectResult AuthError() => Error(ErrorType.Auth, "auth error");

    [Route("test/validation")]
    [HttpGet]
    public ObjectResult ValidationError() => Error(ErrorType.Validation, "validation error");

    [Route("test/conflict")]
    [HttpGet]
    public ObjectResult ConflictException() => Error(ErrorType.Conflict, "conflict");
}