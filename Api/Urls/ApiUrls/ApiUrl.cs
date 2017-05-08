namespace Api.Urls.ApiUrls
{
    public abstract class ApiUrl : Url
    {
        protected override UrlType Type => UrlType.Api;
    }
}