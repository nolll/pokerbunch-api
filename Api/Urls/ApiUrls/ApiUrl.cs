namespace Api.Urls.ApiUrls
{
    public abstract class ApiUrl : Url
    {
        protected override string Host => Settings.ApiHost;
    }
}