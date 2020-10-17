namespace Api.Urls.ApiUrls
{
    public class ApiUrlProvider
    {
        private readonly string _host;

        public ApiUrlProvider(string host)
        {
            _host = host;
        }

        public Url BunchCashgames(string bunchId, int? year) => new ApiBunchCashgamesUrl(_host, bunchId, year);
        public Url Cashgame(string cashgameId) => new ApiCashgameUrl(_host, cashgameId);
        public Url User(string userName) => new ApiUserUrl(_host, userName);
        public Url UserProfile => new ApiUserProfileUrl(_host);
        public Url Swagger => new ApiSwaggerUrl(_host);
    }
}