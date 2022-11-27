namespace Api.Urls.ApiUrls;

public class ApiUrlProvider
{
    private readonly string _host;

    public ApiUrlProvider(string host)
    {
        _host = host;
    }

    public string BunchCashgames(string bunchId, int? year) => new ApiBunchCashgamesUrl(bunchId, year).Absolute(_host);
    public string Cashgame(string cashgameId) => new ApiCashgameUrl(cashgameId).Absolute(_host);
    public string User(string userName) => new ApiUserUrl(userName).Absolute(_host);
    public string UserProfile => new ApiUserProfileUrl().Absolute(_host);
    public string Swagger => new ApiSwaggerUrl().Absolute(_host);
}