using System.Text.Json;
using Api.Models.BunchModels;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Api.Models.EventModels;
using Api.Models.HomeModels;
using Api.Models.JoinRequestModels;
using Api.Models.LocationModels;
using Api.Models.PlayerModels;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;
using Core;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tests.Integration;

public class ApiClientForTest(HttpClientForTest httpClient)
{
    public ActionClient Action { get; } = new (httpClient);
    public AuthClient Auth { get; } = new (httpClient);
    public BunchClient Bunch { get; } = new (httpClient);
    public JoinRequestClient JoinRequest { get; } = new (httpClient);
    public CashgameClient Cashgame { get; } = new (httpClient);
    public EventClient Event { get; } = new (httpClient);
    public GeneralClient General { get; } = new (httpClient);
    public LocationClient Location { get; } = new (httpClient);
    public PlayerClient Player { get; } = new (httpClient);
    public UserClient User { get; } = new (httpClient);
}

public class ActionClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult> Add(string? token, string cashgameId, AddCashgameActionPostModel parameters) => 
        await httpClient.PostAsync(token, new ApiActionAddUrl(cashgameId), parameters);

    public async Task<TestClientResult> Delete(string? token, string cashgameId, string actionId) => 
        await httpClient.DeleteAsync(token, new ApiActionUpdateUrl(cashgameId, actionId));

    public async Task<TestClientResult> Update(string? token, string cashgameId, string actionId, UpdateActionPostModel parameters) => 
        await httpClient.PutAsync(token, new ApiActionUpdateUrl(cashgameId, actionId), parameters);
}

public class AuthClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult<LoginModel>> Login(LoginPostModel parameters)
    {
        var response = await httpClient.GetClient().PostAsJsonAsync(new ApiLoginUrl().Relative, parameters);
        return await httpClient.HandleJsonResponse<LoginModel>(response);
    }
        
    public async Task<TestClientResult<LoginModel>> Refresh(RefreshPostModel parameters)
    {
        var response = await httpClient.GetClient().PostAsJsonAsync(new ApiRefreshUrl().Relative, parameters);
        return await httpClient.HandleJsonResponse<LoginModel>(response);
    }
}

public class BunchClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult<BunchModel>> Add(string? token, AddBunchPostModel parameters) => 
        await httpClient.PostAsync<BunchModel>(token, new ApiBunchAddUrl(), parameters);

    public async Task<TestClientResult<BunchModel>> Get(string? token, string bunchId) => 
        await httpClient.GetAsync<BunchModel>(token, new ApiBunchUrl(bunchId));

    public async Task<TestClientResult<IEnumerable<BunchModel>>> List(string? token) => 
        await httpClient.GetAsync<IEnumerable<BunchModel>>(token, new ApiBunchesUrl());

    public async Task<TestClientResult<IEnumerable<BunchModel>>> ListForUser(string? token) => 
        await httpClient.GetAsync<IEnumerable<BunchModel>>(token, new ApiUserBunchesUrl());

    public async Task<TestClientResult<BunchModel>> Update(string? token, string bunchId, UpdateBunchPostModel parameters) => 
        await httpClient.PutAsync<BunchModel>(token, new ApiBunchUpdateUrl(bunchId), parameters);
}

public class JoinRequestClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult<MessageModel>> Add(string? token, string bunchId) => 
        await httpClient.PostAsync<MessageModel>(token, new ApiJoinRequestAddUrl(bunchId), null);
        
    public async Task<TestClientResult<IEnumerable<JoinRequestModel>>> ListByBunch(string? token, string bunchId) => 
        await httpClient.GetAsync<IEnumerable<JoinRequestModel>>(token, new ApiJoinRequestListUrl(bunchId));
        
    public async Task<TestClientResult<MessageModel>> Accept(string? token, string joinRequestId) => 
        await httpClient.PostAsync<MessageModel>(token, new ApiJoinRequestAcceptUrl(joinRequestId), null);
}

public class CashgameClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult<CashgameDetailsModel>> Add(string? token, string bunchId, AddCashgamePostModel parameters) => 
        await httpClient.PostAsync<CashgameDetailsModel>(token, new ApiCashgameAddUrl(bunchId), parameters);

    public async Task<TestClientResult<IEnumerable<ApiCurrentGame>>> Current(string? token, string bunchId) => 
        await httpClient.GetAsync<IEnumerable<ApiCurrentGame>>(token, new ApiBunchCashgamesCurrentUrl(bunchId));

    public async Task<TestClientResult> Delete(string? token, string cashgameId) => 
        await httpClient.DeleteAsync(token, new ApiCashgameDeleteUrl(cashgameId));

    public async Task<TestClientResult<CashgameDetailsModel>> Get(string? token, string cashgameId) => 
        await httpClient.GetAsync<CashgameDetailsModel>(token, new ApiCashgameUrl(cashgameId));

    public async Task<TestClientResult<IEnumerable<CashgameListItemModel>>> ListByBunch(string? token, string bunchId, int? year = null) => 
        await httpClient.GetAsync<IEnumerable<CashgameListItemModel>>(token, new ApiBunchCashgamesUrl(bunchId, year));

    public async Task<TestClientResult<IEnumerable<CashgameListItemModel>>> ListByEvent(string? token, string eventId) => 
        await httpClient.GetAsync<IEnumerable<CashgameListItemModel>>(token, new ApiEventCashgamesUrl(eventId));

    public async Task<TestClientResult<IEnumerable<CashgameListItemModel>>> ListByPlayer(string? token, string playerId) => 
        await httpClient.GetAsync<IEnumerable<CashgameListItemModel>>(token, new ApiPlayerCashgamesUrl(playerId));

    public async Task<TestClientResult<CashgameDetailsModel>> Update(string? token, string cashgameId, UpdateCashgamePostModel parameters) => 
        await httpClient.PutAsync<CashgameDetailsModel>(token, new ApiCashgameUpdateUrl(cashgameId), parameters);
}

public class EventClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult<EventModel>> Add(string? token, string bunchId, EventAddPostModel parameters) => 
        await httpClient.PostAsync<EventModel>(token, new ApiEventAddUrl(bunchId), parameters);

    public async Task<TestClientResult<EventModel>> Get(string? token, string eventId) => 
        await httpClient.GetAsync<EventModel>(token, new ApiEventUrl(eventId));

    public async Task<TestClientResult<List<EventModel>>> List(string? token, string bunchId) => 
        await httpClient.GetAsync<List<EventModel>>(token, new ApiEventListUrl(bunchId));
}

public class GeneralClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult> ClearCache(string? token) => await httpClient.PostAsync(token, new ApiAdminClearCacheUrl());
    public async Task<TestClientResult> Root() => await httpClient.GetAsync(new ApiRootUrl(), false);
    public async Task<TestClientResult> Swagger() => await httpClient.GetAsync(new ApiSwaggerUrl());
    public async Task<TestClientResult> TestEmail(string? token) => await httpClient.PostAsync(token, new ApiAdminSendEmailUrl());
    public async Task<TestClientResult<VersionModel>> Version() => await httpClient.GetAsync<VersionModel>(new ApiVersionUrl());
}

public class LocationClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult<LocationModel>> Add(string? token, string bunchId, LocationAddPostModel parameters) => 
        await httpClient.PostAsync<LocationModel>(token, new ApiLocationAddUrl(bunchId), parameters);

    public async Task<TestClientResult<LocationModel>> Get(string? token, string locationId) => 
        await httpClient.GetAsync<LocationModel>(token, new ApiLocationUrl(locationId));

    public async Task<TestClientResult<List<LocationModel>>> List(string? token, string bunchId) => 
        await httpClient.GetAsync<List<LocationModel>>(token, new ApiLocationListUrl(bunchId));
}

public class PlayerClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult<PlayerModel>> Add(string? token, string bunchId, PlayerAddPostModel parameters) =>
        await httpClient.PostAsync<PlayerModel>(token, new ApiPlayerAddUrl(bunchId), parameters);

    public async Task<TestClientResult> Delete(string? token, string playerId) =>
        await httpClient.DeleteAsync(token, new ApiPlayerDeleteUrl(playerId));

    public async Task<TestClientResult<PlayerModel>> Get(string? token, string playerId) =>
        await httpClient.GetAsync<PlayerModel>(token, new ApiPlayerUrl(playerId));

    public async Task<TestClientResult<List<PlayerListItemModel>>> List(string? token, string bunchId) =>
        await httpClient.GetAsync<List<PlayerListItemModel>>(token, new ApiPlayerListUrl(bunchId));
}

public class UserClient(HttpClientForTest httpClient)
{
    public async Task<TestClientResult> Add(AddUserPostModel parameters) => await httpClient.PostAsync(new ApiUserAddUrl(), parameters);

    public async Task<TestClientResult<FullUserModel>> GetAsAdmin(string token, string userName)
    {
        return await httpClient.GetAsync<FullUserModel>(token, new ApiUserUrl(userName));
    }

    public async Task<TestClientResult<UserModel>> GetAsUser(string token, string userName)
    {
        return await httpClient.GetAsync<UserModel>(token, new ApiUserUrl(userName));
    }

    public async Task<TestClientResult<List<UserModel>>> List(string? token) =>
        await httpClient.GetAsync<List<UserModel>>(token, new ApiUserListUrl());

    public async Task<TestClientResult> PasswordChange(string? token, ChangePasswordPostModel parameters) =>
        await httpClient.PutAsync(token, new ApiUserChangePasswordUrl(), parameters);

    public async Task<TestClientResult> PasswordReset(ResetPasswordPostModel parameters) =>
        await httpClient.PostAsync(new ApiUserResetPasswordUrl(), parameters);

    public async Task<TestClientResult<FullUserModel>> Profile(string? token) =>
        await httpClient.GetAsync<FullUserModel>(token, new ApiUserProfileUrl());

    public async Task<TestClientResult<FullUserModel>> Update(string? token, string userName, UpdateUserPostModel parameters) =>
        await httpClient.PutAsync<FullUserModel>(token, new ApiUserUpdateUrl(userName), parameters);
}

public class HttpClientForTest(WebApplicationFactory<Program> webApplicationFactory)
{
    public async Task<TestClientResult> GetAsync(ApiUrl url, bool followRedirect = true) => await GetAsync(null, url, followRedirect);

    public async Task<TestClientResult> GetAsync(string? token, ApiUrl url, bool followRedirect = true)
    {
        var response = await GetClient(token, followRedirect).GetAsync(url.Relative);
        return HandleEmptyResponse(response);
    }

    public async Task<TestClientResult<T>> GetAsync<T>(ApiUrl url) where T : class => await GetAsync<T>(null, url);

    public async Task<TestClientResult<T>> GetAsync<T>(string? token, ApiUrl url) where T : class
    {
        var response = await GetClient(token).GetAsync(url.Relative);
        return await HandleJsonResponse<T>(response);
    }

    public async Task<TestClientResult> PostAsync(ApiUrl url, object parameters) => await PostAsync(null, url, parameters);

    public async Task<TestClientResult> PostAsync(string? token, ApiUrl url, object? parameters = null)
    {
        var response = await GetClient(token).PostAsJsonAsync(url.Relative, parameters);
        return HandleEmptyResponse(response);
    }

    public async Task<TestClientResult<T>> PostAsync<T>(ApiUrl url, object? parameters) where T : class => 
        await PostAsync<T>(null, url, parameters);

    public async Task<TestClientResult<T>> PostAsync<T>(string? token, ApiUrl url, object? parameters) where T : class
    {
        var response = await GetClient(token).PostAsJsonAsync(url.Relative, parameters);
        return await HandleJsonResponse<T>(response);
    }

    public async Task<TestClientResult> PutAsync(string? token, ApiUrl url, object? parameters = null)
    {
        var response = await GetClient(token).PutAsJsonAsync(url.Relative, parameters);
        return HandleEmptyResponse(response);
    }

    public async Task<TestClientResult<T>> PutAsync<T>(string? token, ApiUrl url, object parameters) where T : class
    {
        var response = await GetClient(token).PutAsJsonAsync(url.Relative, parameters);
        return await HandleJsonResponse<T>(response);
    }

    public async Task<TestClientResult> DeleteAsync(string? token, ApiUrl url)
    {
        var response = await GetClient(token).DeleteAsync(url.Relative);
        return HandleEmptyResponse(response);
    }

    public async Task<TestClientResult<T>> HandleJsonResponse<T>(HttpResponseMessage response) where T : class
    {
        if (!response.IsSuccessStatusCode)
            return new TestClientResult<T>(false, response.StatusCode, null);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(content);

        return new TestClientResult<T>(true, response.StatusCode, result);
    }
    
    private TestClientResult HandleEmptyResponse(HttpResponseMessage response) => 
        new(response.IsSuccessStatusCode, response.StatusCode);

    public HttpClient GetClient(string? token = null, bool followRedirect = true)
    {
        if (webApplicationFactory == null)
            throw new PokerBunchException("WebApplicationFactory was not initialized.");

        var options = new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = !followRedirect
        };
        
        var client = webApplicationFactory.CreateClient(options);

        if(token != null)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }
}