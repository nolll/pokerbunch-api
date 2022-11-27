using System.Text.Json.Serialization;
using Api.Urls.ApiUrls;

namespace Api.Models.HomeModels;

public class HomeModel
{
    [JsonPropertyName("userProfileUrl")]
    public string UserProfileUrl { get; }

    [JsonPropertyName("docsUrl")]
    public string DocsUrl { get; }

    [JsonPropertyName("swaggerUrl")]
    public string SwaggerUrl { get; }

    public HomeModel(UrlProvider urls)
    {
        UserProfileUrl = urls.Api.UserProfile;
        DocsUrl = urls.Site.ApiDocs;
        SwaggerUrl = urls.Api.Swagger;
    }

    [JsonConstructor]
    public HomeModel(string userProfileUrl, string docsUrl, string swaggerUrl)
    {
        UserProfileUrl = userProfileUrl;
        DocsUrl = docsUrl;
        SwaggerUrl = swaggerUrl;
    }
}