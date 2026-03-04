using System.Net;

namespace Tests.Integration;

public class TestClientResult<T>(bool success, HttpStatusCode statusCode, T? model) where T : class
{
    public bool Success { get; } = success;
    public HttpStatusCode StatusCode { get; } = statusCode;
    public T? Model { get; } = model;
}

public class TestClientResult(bool success, HttpStatusCode statusCode)
{
    public bool Success { get; } = success;
    public HttpStatusCode StatusCode { get; } = statusCode;
}