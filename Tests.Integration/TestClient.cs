using System.Text.Json;
using Api.Models.EventModels;
using Api.Urls.ApiUrls;
using Tests.Integration.Tests;

namespace Tests.Integration;

public static class TestClient
{
    public static async Task<TestClientResult<EventModel>> GetEvent(string token, string eventId)
    {
        return await Get<EventModel>(token, new ApiEventUrl(eventId));
    }

    private static async Task<TestClientResult<T>> Get<T>(string token, ApiUrl url) where T : class
    {
        var response = await TestSetup.AuthorizedClient(token).GetAsync(url.Relative);
        if (!response.IsSuccessStatusCode)
            return new TestClientResult<T>(false, response.StatusCode, null);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(content);

        return new TestClientResult<T>(true, response.StatusCode, result);
    }
}