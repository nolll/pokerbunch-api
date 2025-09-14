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
        public static async Task<TestClientResult> Add(string? token, string cashgameId, AddCashgameActionPostModel parameters)
        {
            return await PostAsync(token, new ApiActionAddUrl(cashgameId), parameters);
        }

        public static async Task<TestClientResult> Delete(string? token, string cashgameId, string actionId)
        {
            return await DeleteAsync(token, new ApiActionUpdateUrl(cashgameId, actionId));
        }

        public static async Task<TestClientResult> Update(string? token, string cashgameId, string actionId, UpdateActionPostModel parameters)
        {
            return await PutAsync(token, new ApiActionUpdateUrl(cashgameId, actionId), parameters);
        }
    }

    public static class Auth
    {
        public static async Task<TestClientResult<LoginModel>> Login(LoginPostModel parameters)
        {
            var response = await GetClient().PostAsJsonAsync(new ApiLoginUrl().Relative, parameters);
            return await HandleJsonResponse<LoginModel>(response);
        }
        
        public static async Task<TestClientResult<LoginModel>> Refresh(RefreshPostModel parameters)
        {
            var response = await GetClient().PostAsJsonAsync(new ApiRefreshUrl().Relative, parameters);
            return await HandleJsonResponse<LoginModel>(response);
        }
    }

    public static class Bunch
    {
        public static async Task<TestClientResult<BunchModel>> Add(string? token, AddBunchPostModel parameters)
        {
            return await PostAsync<BunchModel>(token, new ApiBunchAddUrl(), parameters);
        }

        public static async Task<TestClientResult<BunchModel>> Get(string? token, string bunchId)
        {
            return await GetAsync<BunchModel>(token, new ApiBunchUrl(bunchId));
        }

        public static async Task<TestClientResult> Join(string? token, string bunchId, JoinBunchPostModel parameters)
        {
            return await PostAsync(token, new ApiBunchJoinUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<IEnumerable<BunchModel>>> List(string? token)
        {
            return await GetAsync<IEnumerable<BunchModel>>(token, new ApiBunchesUrl());
        }

        public static async Task<TestClientResult<IEnumerable<BunchModel>>> ListForUser(string? token)
        {
            return await GetAsync<IEnumerable<BunchModel>>(token, new ApiUserBunchesUrl());
        }

        public static async Task<TestClientResult<BunchModel>> Update(string? token, string bunchId, UpdateBunchPostModel parameters)
        {
            return await PutAsync<BunchModel>(token, new ApiBunchUpdateUrl(bunchId), parameters);
        }
    }

    public static class Cashgame
    {
        public static async Task<TestClientResult<CashgameDetailsModel>> Add(string? token, string bunchId, AddCashgamePostModel parameters)
        {
            return await PostAsync<CashgameDetailsModel>(token, new ApiCashgameAddUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<IEnumerable<ApiCurrentGame>>> Current(string? token, string bunchId)
        {
            return await GetAsync<IEnumerable<ApiCurrentGame>>(token, new ApiBunchCashgamesCurrentUrl(bunchId));
        }

        public static async Task<TestClientResult> Delete(string? token, string cashgameId)
        {
            return await DeleteAsync(token, new ApiCashgameDeleteUrl(cashgameId));
        }

        public static async Task<TestClientResult<CashgameDetailsModel>> Get(string? token, string cashgameId)
        {
            return await GetAsync<CashgameDetailsModel>(token, new ApiCashgameUrl(cashgameId));
        }

        public static async Task<TestClientResult<IEnumerable<CashgameListItemModel>>> ListByBunch(string? token, string bunchId, int? year = null)
        {
            return await GetAsync<IEnumerable<CashgameListItemModel>>(token, new ApiBunchCashgamesUrl(bunchId, year));
        }

        public static async Task<TestClientResult<IEnumerable<CashgameListItemModel>>> ListByEvent(string? token, string eventId)
        {
            return await GetAsync<IEnumerable<CashgameListItemModel>>(token, new ApiEventCashgamesUrl(eventId));
        }

        public static async Task<TestClientResult<IEnumerable<CashgameListItemModel>>> ListByPlayer(string? token, string playerId)
        {
            return await GetAsync<IEnumerable<CashgameListItemModel>>(token, new ApiPlayerCashgamesUrl(playerId));
        }

        public static async Task<TestClientResult<CashgameDetailsModel>> Update(string? token, string cashgameId, UpdateCashgamePostModel parameters)
        {
            return await PutAsync<CashgameDetailsModel>(token, new ApiCashgameUpdateUrl(cashgameId), parameters);
        }
    }

    public static class Event
    {
        public static async Task<TestClientResult<EventModel>> Add(string? token, string bunchId, EventAddPostModel parameters)
        {
            return await PostAsync<EventModel>(token, new ApiEventAddUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<EventModel>> Get(string? token, string eventId)
        {
            return await GetAsync<EventModel>(token, new ApiEventUrl(eventId));
        }

        public static async Task<TestClientResult<List<EventModel>>> List(string? token, string bunchId)
        {
            return await GetAsync<List<EventModel>>(token, new ApiEventListUrl(bunchId));
        }
    }

    public static class General
    {
        public static async Task<TestClientResult> ClearCache(string? token)
        {
            return await PostAsync(token, new ApiAdminClearCacheUrl());
        }

        public static async Task<TestClientResult> Root()
        {
            return await GetAsync(new ApiRootUrl(), false);
        }

        public static async Task<TestClientResult> Settings(string? token)
        {
            return await GetAsync(token, new ApiSettingsUrl());
        }

        public static async Task<TestClientResult> Swagger()
        {
            return await GetAsync(new ApiSwaggerUrl());
        }

        public static async Task<TestClientResult> TestEmail(string? token)
        {
            return await PostAsync(token, new ApiAdminSendEmailUrl());
        }

        public static async Task<TestClientResult<VersionModel>> Version()
        {
            return await GetAsync<VersionModel>(new ApiVersionUrl());
        }
    }

    public static class Location
    {
        public static async Task<TestClientResult<LocationModel>> Add(string? token, string bunchId, LocationAddPostModel parameters)
        {
            return await PostAsync<LocationModel>(token, new ApiLocationAddUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult<LocationModel>> Get(string? token, string locationId)
        {
            return await GetAsync<LocationModel>(token, new ApiLocationUrl(locationId));
        }

        public static async Task<TestClientResult<List<LocationModel>>> List(string? token, string bunchId)
        {
            return await GetAsync<List<LocationModel>>(token, new ApiLocationListUrl(bunchId));
        }
    }

    public static class Player
    {
        public static async Task<TestClientResult<PlayerModel>> Add(string? token, string bunchId, PlayerAddPostModel parameters)
        {
            return await PostAsync<PlayerModel>(token, new ApiPlayerAddUrl(bunchId), parameters);
        }

        public static async Task<TestClientResult> Delete(string? token, string playerId)
        {
            return await DeleteAsync(token, new ApiPlayerDeleteUrl(playerId));
        }

        public static async Task<TestClientResult<PlayerModel>> Get(string? token, string playerId)
        {
            return await GetAsync<PlayerModel>(token, new ApiPlayerUrl(playerId));
        }

        public static async Task<TestClientResult> Invite(string? token, string playerId, PlayerInvitePostModel parameters)
        {
            return await PostAsync(token, new ApiPlayerInviteUrl(TestData.UserPlayerId), parameters);
        }

        public static async Task<TestClientResult<List<PlayerListItemModel>>> List(string? token, string bunchId)
        {
            return await GetAsync<List<PlayerListItemModel>>(token, new ApiPlayerListUrl(bunchId));
        }
    }

    public static class User
    {
        public static async Task<TestClientResult> Add(AddUserPostModel parameters)
        {
            return await PostAsync(new ApiUserAddUrl(), parameters);
        }

        public static async Task<TestClientResult<FullUserModel>> GetAsAdmin(string userName)
        {
            var token = await LoginHelper.GetAdminToken();
            return await GetAsync<FullUserModel>(token, new ApiUserUrl(userName));
        }

        public static async Task<TestClientResult<UserModel>> GetAsUser(string userName)
        {
            var userToken = await LoginHelper.GetUserToken();
            return await GetAsync<UserModel>(userToken, new ApiUserUrl(userName));
        }

        public static async Task<TestClientResult<List<UserModel>>> List(string? token)
        {
            return await GetAsync<List<UserModel>>(token, new ApiUserListUrl());
        }

        public static async Task<TestClientResult> PasswordChange(string? token, ChangePasswordPostModel parameters)
        {
            return await PutAsync(token, new ApiUserChangePasswordUrl(), parameters);
        }

        public static async Task<TestClientResult> PasswordReset(ResetPasswordPostModel parameters)
        {
            return await PostAsync(new ApiUserResetPasswordUrl(), parameters);
        }

        public static async Task<TestClientResult<FullUserModel>> Profile(string? token)
        {
            return await GetAsync<FullUserModel>(token, new ApiUserProfileUrl());
        }

        public static async Task<TestClientResult<FullUserModel>> Update(string? token, string userName, UpdateUserPostModel parameters)
        {
            return await PutAsync<FullUserModel>(token, new ApiUserUpdateUrl(userName), parameters);
        }
    }
    
    private static async Task<TestClientResult> GetAsync(ApiUrl url, bool followRedirect = true)
    {
        return await GetAsync(null, url, followRedirect);
    }

    private static async Task<TestClientResult> GetAsync(string? token, ApiUrl url, bool followRedirect = true)
    {
        var response = await GetClient(token, followRedirect).GetAsync(url.Relative);
        return HandleEmptyResponse(response);
    }

    private static async Task<TestClientResult<T>> GetAsync<T>(ApiUrl url) where T : class
    {
        return await GetAsync<T>(null, url);
    }

    private static async Task<TestClientResult<T>> GetAsync<T>(string? token, ApiUrl url) where T : class
    {
        var response = await GetClient(token).GetAsync(url.Relative);
        return await HandleJsonResponse<T>(response);
    }

    private static async Task<TestClientResult> PostAsync(ApiUrl url, object parameters)
    {
        return await PostAsync(null, url, parameters);
    }

    private static async Task<TestClientResult> PostAsync(string? token, ApiUrl url, object? parameters = null)
    {
        var response = await GetClient(token).PostAsJsonAsync(url.Relative, parameters);
        return HandleEmptyResponse(response);
    }

    private static async Task<TestClientResult<T>> PostAsync<T>(ApiUrl url, object parameters) where T : class
    {
        return await PostAsync<T>(null, url, parameters);
    }

    private static async Task<TestClientResult<T>> PostAsync<T>(string? token, ApiUrl url, object parameters) where T : class
    {
        var response = await GetClient(token).PostAsJsonAsync(url.Relative, parameters);
        return await HandleJsonResponse<T>(response);
    }

    private static async Task<TestClientResult> PutAsync(string? token, ApiUrl url, object? parameters = null)
    {
        var response = await GetClient(token).PutAsJsonAsync(url.Relative, parameters);
        return HandleEmptyResponse(response);
    }

    private static async Task<TestClientResult<T>> PutAsync<T>(string? token, ApiUrl url, object parameters) where T : class
    {
        var response = await GetClient(token).PutAsJsonAsync(url.Relative, parameters);
        return await HandleJsonResponse<T>(response);
    }

    private static async Task<TestClientResult> DeleteAsync(string? token, ApiUrl url)
    {
        var response = await GetClient(token).DeleteAsync(url.Relative);
        return HandleEmptyResponse(response);
    }

    private static HttpClient GetClient(string? token = null, bool followRedirect = true)
    {
        return TestSetup.GetClient(token, followRedirect);
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