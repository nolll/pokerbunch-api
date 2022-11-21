using System.Net;

namespace Tests.Integration;

public class TestClientResult<T> where T : class
{
    public bool Success { get; }
    public HttpStatusCode StatusCode { get; }
    public T Model { get; }

    public TestClientResult(bool success, HttpStatusCode statusCode, T model)
    {
        Success = success;
        StatusCode = statusCode;
        Model = model;
    }
}

public class TestClientResult
{
    public bool Success { get; }
    public HttpStatusCode StatusCode { get; }

    public TestClientResult(bool success, HttpStatusCode statusCode)
    {
        Success = success;
        StatusCode = statusCode;
    }
}