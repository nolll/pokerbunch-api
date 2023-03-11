using Api.Settings;
using Core.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class TestController : BaseController
{
    public TestController(AppSettings appSettings) : base(appSettings)
    {
    }

    [Route("test/exception")]
    [HttpGet]
    public ObjectResult Exception()
    {
        throw new Exception("exception");
    }

    [Route("test/unexpected")]
    [HttpGet]
    public ObjectResult Unexpected()
    {
        return Error(ErrorType.Unknown, "unknown");
    }

    [Route("test/notfound")]
    [HttpGet]
    public ObjectResult NotFoundError()
    {
        return Error(ErrorType.NotFound, "not found");
    }

    [Route("test/accessdenied")]
    [HttpGet]
    public ObjectResult AccessDeniedError()
    {
        return Error(ErrorType.AccessDenied, "access denied");
    }

    [Route("test/auth")]
    [HttpGet]
    public ObjectResult AuthError()
    {
        return Error(ErrorType.Auth, "auth error");
    }

    [Route("test/validation")]
    [HttpGet]
    public ObjectResult ValidationError()
    {
        return Error(ErrorType.NotFound, "validation error");
    }

    [Route("test/conflict")]
    [HttpGet]
    public ObjectResult ConflictException()
    {
        return Error(ErrorType.Conflict, "conflict");
    }
}