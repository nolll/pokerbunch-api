namespace Api.Urls;

public abstract class Url
{
    private readonly string _host;
    protected abstract string Input { get; }
    public string Relative => Input != null ? $"/{Input.ToLower()}" : string.Empty;
    public string Absolute() => $"https://{_host}{Relative}";

    protected Url(string host)
    {
        _host = host;
    }
}