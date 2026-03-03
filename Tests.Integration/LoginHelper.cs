using Api.Models.UserModels;

namespace Tests.Integration;

public class LoginHelper(ApiClientForTest apiClient, TestData data)
{
    public async Task<string> GetAdminToken() => (await GetTokens(data.AdminUserName, data.AdminPassword)).AccessToken;
    public async Task<string> GetManagerToken() => (await GetTokens(data.ManagerUserName, data.ManagerPassword)).AccessToken;
    public async Task<string> GetUserToken() => (await GetTokens(data.UserUserName, data.UserPassword)).AccessToken;

    private async Task<LoginModel> GetTokens(string userName, string password) => 
        (await Login(userName, password)).Model!;

    public Task<TestClientResult<LoginModel>> LoginAdmin() => Login(data.AdminUserName, data.AdminPassword);
    public Task<TestClientResult<LoginModel>> LoginManager() => Login(data.ManagerUserName, data.ManagerPassword);
    public Task<TestClientResult<LoginModel>> LoginUser() => Login(data.UserUserName, data.UserPassword);

    private async Task<TestClientResult<LoginModel>> Login(string userName, string password) => 
        await apiClient.Auth.Login(new LoginPostModel(userName, password));
}