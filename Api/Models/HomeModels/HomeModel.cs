using System.Text.Json.Serialization;
using Api.Urls.ApiUrls;

namespace Api.Models.HomeModels;

public class HomeModel
{
    [JsonPropertyName("userProfileUrl")]
    public string UserProfileUrl { get; }

    [JsonPropertyName("docs")]
    public string DocsUrl { get; }

    [JsonPropertyName("swagger")]
    public string SwaggerUrl { get; }

    public HomeModel(UrlProvider urls)
    {
        UserProfileUrl = urls.Api.UserProfile.Absolute();
        DocsUrl = urls.Site.ApiDocs.Absolute();
        SwaggerUrl = urls.Api.Swagger.Absolute();
    }

    [JsonConstructor]
    public HomeModel(string userProfileUrl, string docsUrl, string swaggerUrl)
    {
        UserProfileUrl = userProfileUrl;
        DocsUrl = docsUrl;
        SwaggerUrl = swaggerUrl;
    }
}