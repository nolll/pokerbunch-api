using System.Text.Json;
using Api.Models.BunchModels;
using Api.Models.EventModels;
using Api.Models.LocationModels;
using Api.Models.PlayerModels;
using Api.Urls.ApiUrls;

namespace Tests.Integration;

public static class TestClient
{
    public static class Bunch
    {
        public static async Task<TestClientResult<BunchModel>> Add(string token, AddBunchPostModel parameters)
        {
            return await Post<BunchModel>(token, new ApiBunchAddUrl(), parameters);
        }

        public static async Task<TestClientResult<BunchModel>> Get(string token, string bunchId)
        {
            return await Get<BunchModel>(token, new ApiBunchUrl(bunchId));
        }
    }

    public static class Event
    {
        public static async Task<TestClientResult<EventModel>> Add(string token, string bunchId, EventAddPostModel parameters)
        {
            return await Post<EventModel>(token, new ApiEventAddUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<EventModel>> Get(string token, string eventId)
        {
            return await Get<EventModel>(token, new ApiEventUrl(eventId));
        }

        public static async Task<TestClientResult<List<EventModel>>> List(string token, string bunchId)
        {
            return await Get<List<EventModel>>(token, new ApiEventListUrl(bunchId));
        }
    }

    public static class Location
    {
        public static async Task<TestClientResult<LocationModel>> Add(string token, string bunchId, LocationAddPostModel parameters)
        {
            return await Post<LocationModel>(token, new ApiLocationAddUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<LocationModel>> Get(string token, string locationId)
        {
            return await Get<LocationModel>(token, new ApiLocationUrl(locationId));
        }

        public static async Task<TestClientResult<List<LocationModel>>> List(string token, string bunchId)
        {
            return await Get<List<LocationModel>>(token, new ApiLocationListUrl(bunchId));
        }
    }

    public static class Player
    {
        public static async Task<TestClientResult<PlayerModel>> Add(string token, string bunchId, PlayerAddPostModel parameters)
        {
            return await Post<PlayerModel>(token, new ApiPlayerAddUrl(bunchId), parameters);
        }
    }

    public static async Task<HttpResponseMessage> Get(string url)
    {
        return await Get(null, url);
    }

    public static async Task<HttpResponseMessage> Get(string token, string url)
    {
        return await GetClient(token).GetAsync(url);
    }

    public static async Task<string> GetString(string url)
    {
        return await GetString(null, url);
    }

    public static async Task<string> GetString(string token, string url)
    {
        var response = await GetClient(token).GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<HttpResponseMessage> Post(string url, object parameters)
    {
        return await Post(null, url, parameters);
    }

    public static async Task<HttpResponseMessage> Post(string token, string url, object parameters = null)
    {
        return await GetClient(token).PostAsJsonAsync(url, parameters);
    }

    public static async Task<HttpResponseMessage> Delete(string token, string url)
    {
        return await GetClient(token).DeleteAsync(url);
    }

    private static async Task<TestClientResult<T>> Get<T>(string token, ApiUrl url) where T : class
    {
        var response = await GetClient(token).GetAsync(url.Relative);
        return await HandleResponse<T>(response);
    }

    private static async Task<TestClientResult<T>> Post<T>(string token, ApiUrl url, object parameters) where T : class
    {
        var response = await GetClient(token).PostAsJsonAsync(url.Relative, parameters);
        return await HandleResponse<T>(response);
    }

    private static HttpClient GetClient(string token = null)
    {
        return TestSetup.GetClient(token);
    }

    private static async Task<TestClientResult<T>> HandleResponse<T>(HttpResponseMessage response) where T : class
    {
        if (!response.IsSuccessStatusCode)
            return new TestClientResult<T>(false, response.StatusCode, null);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(content);

        return new TestClientResult<T>(true, response.StatusCode, result);
    }
}