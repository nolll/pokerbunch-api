using System.Text.Json;
using Api.Models.BunchModels;
using Api.Models.CashgameModels;
using Api.Models.EventModels;
using Api.Models.HomeModels;
using Api.Models.LocationModels;
using Api.Models.PlayerModels;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;

namespace Tests.Integration;

public static class TestClient
{
    public static class Action
    {
        public static async Task<TestClientResult> Add(string token, string cashgameId, AddCashgameActionPostModel parameters)
        {
            return await Post(token, new ApiActionAddUrl(cashgameId), parameters);
        }

        public static async Task<TestClientResult> Delete(string token, string cashgameId, string actionId)
        {
            return await Del(token, new ApiActionUpdateUrl(cashgameId, actionId));
        }

        public static async Task<TestClientResult> Update(string token, string cashgameId, string actionId, UpdateActionPostModel parameters)
        {
            return await Put(token, new ApiActionUpdateUrl(cashgameId, actionId), parameters);
        }
    }

    public static class Auth
    {
        public static async Task<TestClientResult<string>> Login(LoginPostModel parameters)
        {
            var response = await GetClient().PostAsJsonAsync(new ApiLoginUrl().Relative, parameters);
            return await HandleStringResponse(response);
        }
    }

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

        public static async Task<TestClientResult> Join(string token, string bunchId, JoinBunchPostModel parameters)
        {
            return await Post(token, new ApiBunchJoinUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<BunchModel>> Update(string token, string bunchId, UpdateBunchPostModel parameters)
        {
            return await Put<BunchModel>(token, new ApiBunchUpdateUrl(bunchId), parameters);
        }
    }

    public static class Cashgame
    {
        public static async Task<TestClientResult<CashgameDetailsModel>> Add(string token, string bunchId, AddCashgamePostModel parameters)
        {
            return await Post<CashgameDetailsModel>(token, new ApiCashgameAddUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<IEnumerable<ApiCurrentGame>>> Current(string token, string bunchId)
        {
            return await Get<IEnumerable<ApiCurrentGame>>(token, new ApiBunchCashgamesCurrentUrl(bunchId));
        }

        public static async Task<TestClientResult<CashgameDetailsModel>> Get(string token, string cashgameId)
        {
            return await Get<CashgameDetailsModel>(token, new ApiCashgameUrl(cashgameId));
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

    public static class General
    {
        public static async Task<TestClientResult> ClearCache(string token)
        {
            return await Post(token, new ApiAdminClearCacheUrl());
        }

        public static async Task<TestClientResult<HomeModel>> Root()
        {
            return await Get<HomeModel>(new ApiRootUrl());
        }

        public static async Task<TestClientResult> Settings(string token)
        {
            return await Get(token, new ApiSettingsUrl());
        }

        public static async Task<TestClientResult> Swagger()
        {
            return await Get(new ApiSwaggerUrl());
        }

        public static async Task<TestClientResult> TestEmail(string token)
        {
            return await Post(token, new ApiAdminSendEmailUrl());
        }

        public static async Task<TestClientResult<VersionModel>> Version()
        {
            return await Get<VersionModel>(new ApiVersionUrl());
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

        public static async Task<TestClientResult> Delete(string token, string playerId)
        {
            return await Del(token, new ApiPlayerDeleteUrl(playerId));
        }

        public static async Task<TestClientResult<PlayerModel>> Get(string token, string playerId)
        {
            return await Get<PlayerModel>(token, new ApiPlayerUrl(playerId));
        }

        public static async Task<TestClientResult> Invite(string token, string playerId, PlayerInvitePostModel parameters)
        {
            return await Post(token, new ApiPlayerInviteUrl(TestData.UserPlayerId), parameters);
        }

        public static async Task<TestClientResult<List<PlayerListItemModel>>> List(string token, string bunchId)
        {
            return await Get<List<PlayerListItemModel>>(token, new ApiPlayerListUrl(bunchId));
        }
    }

    public static class User
    {
        public static async Task<TestClientResult> Add(AddUserPostModel parameters)
        {
            return await Post(new ApiUserAddUrl(), parameters);
        }

        public static async Task<TestClientResult<FullUserModel>> GetAsAdmin(string userName)
        {
            return await Get<FullUserModel>(TestData.AdminToken, new ApiUserUrl(userName));
        }

        public static async Task<TestClientResult<UserModel>> GetAsUser(string userName)
        {
            return await Get<UserModel>(TestData.UserToken, new ApiUserUrl(userName));
        }

        public static async Task<TestClientResult<List<UserModel>>> List(string token)
        {
            return await Get<List<UserModel>>(token, new ApiUserListUrl());
        }

        public static async Task<TestClientResult<FullUserModel>> Profile(string token)
        {
            return await Get<FullUserModel>(token, new ApiUserProfileUrl());
        }
    }
    
    private static async Task<TestClientResult> Get(ApiUrl url)
    {
        return await Get(null, url);
    }

    private static async Task<TestClientResult> Get(string token, ApiUrl url)
    {
        var response = await GetClient(token).GetAsync(url.Relative);
        return HandleEmptyResponse(response);
    }

    private static async Task<TestClientResult<T>> Get<T>(ApiUrl url) where T : class
    {
        return await Get<T>(null, url);
    }

    private static async Task<TestClientResult<T>> Get<T>(string token, ApiUrl url) where T : class
    {
        var response = await GetClient(token).GetAsync(url.Relative);
        return await HandleJsonResponse<T>(response);
    }

    private static async Task<TestClientResult> Post(ApiUrl url, object parameters)
    {
        return await Post(null, url, parameters);
    }

    private static async Task<TestClientResult> Post(string token, ApiUrl url, object parameters = null)
    {
        var response = await GetClient(token).PostAsJsonAsync(url.Relative, parameters);
        return HandleEmptyResponse(response);
    }

    private static async Task<TestClientResult<T>> Post<T>(ApiUrl url, object parameters) where T : class
    {
        return await Post<T>(null, url, parameters);
    }

    private static async Task<TestClientResult<T>> Post<T>(string token, ApiUrl url, object parameters) where T : class
    {
        var response = await GetClient(token).PostAsJsonAsync(url.Relative, parameters);
        return await HandleJsonResponse<T>(response);
    }

    private static async Task<TestClientResult> Put(string token, ApiUrl url, object parameters = null)
    {
        var response = await GetClient(token).PutAsJsonAsync(url.Relative, parameters);
        return HandleEmptyResponse(response);
    }

    private static async Task<TestClientResult<T>> Put<T>(string token, ApiUrl url, object parameters) where T : class
    {
        var response = await GetClient(token).PutAsJsonAsync(url.Relative, parameters);
        return await HandleJsonResponse<T>(response);
    }

    private static async Task<TestClientResult> Del(string token, ApiUrl url)
    {
        var response = await GetClient(token).DeleteAsync(url.Relative);
        return HandleEmptyResponse(response);
    }

    private static HttpClient GetClient(string token = null)
    {
        return TestSetup.GetClient(token);
    }

    private static async Task<TestClientResult<T>> HandleJsonResponse<T>(HttpResponseMessage response) where T : class
    {
        if (!response.IsSuccessStatusCode)
            return new TestClientResult<T>(false, response.StatusCode, null);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(content);

        return new TestClientResult<T>(true, response.StatusCode, result);
    }

    private static async Task<TestClientResult<string>> HandleStringResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return new TestClientResult<string>(false, response.StatusCode, null);

        var content = await response.Content.ReadAsStringAsync();

        return new TestClientResult<string>(true, response.StatusCode, content);
    }

    private static TestClientResult HandleEmptyResponse(HttpResponseMessage response)
    {
        return new TestClientResult(response.IsSuccessStatusCode, response.StatusCode);
    }
}