namespace Api.Urls.ApiUrls;

public class ApiUrlProvider
{
    private readonly string _host;

    public ApiUrlProvider(string host)
    {
        _host = host;
    }

    public string Cashgame(string cashgameId) => new ApiCashgameUrl(cashgameId).Absolute(_host);
    public string User(string userName) => new ApiUserUrl(userName).Absolute(_host);
}