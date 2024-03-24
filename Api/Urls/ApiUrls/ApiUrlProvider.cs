namespace Api.Urls.ApiUrls;

public class ApiUrlProvider(string host)
{
    public string Cashgame(string cashgameId) => new ApiCashgameUrl(cashgameId).Absolute(host);
    public string User(string userName) => new ApiUserUrl(userName).Absolute(host);
}