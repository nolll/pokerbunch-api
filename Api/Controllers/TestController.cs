using System;
using Api.Settings;
using Core.Errors;
using Core.Exceptions;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TestController : BaseController
{
    public TestController(AppSettings appSettings) : base(appSettings)
    {
    }

    [Route("test/unexpected")]
    [HttpGet]
    public void Unexpected()
    {
        throw new Exception("unexpected");
    }

    [Route("test/notfound")]
    [HttpGet]
    public ObjectResult NotFoundException()
    {
        return Error(ErrorType.NotFound, "not found");
    }

    [Route("test/accessdenied")]
    [HttpGet]
    public void AccessDeniedException()
    {
        throw new AccessDeniedException("access denied");
    }

    [Route("test/auth")]
    [HttpGet]
    public void AuthException()
    {
        throw new AuthException("auth error");
    }

    [Route("test/validation")]
    [HttpGet]
    public void ValidationException()
    {
        throw new ValidationException("validation");
    }

    [Route("test/conflict")]
    [HttpGet]
    public void ConflictException()
    {
        throw new ConflictException("conflict");
    }
}