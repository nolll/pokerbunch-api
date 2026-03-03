using Api.Models.UserModels;

namespace Tests.Integration;

public class LoginHelper(ApiClientForTest apiClient)
{
    public async Task<string> GetAdminToken() => (await GetTokens(TestData.AdminUserName, TestData.AdminPassword)).AccessToken;
    public async Task<string> GetManagerToken() => (await GetTokens(TestData.ManagerUserName, TestData.ManagerPassword)).AccessToken;
    public async Task<string> GetUserToken() => (await GetTokens(TestData.UserUserName, TestData.UserPassword)).AccessToken;

    private async Task<LoginModel> GetTokens(string userName, string password) => 
        (await Login(userName, password)).Model!;

    public Task<TestClientResult<LoginModel>> LoginAdmin() => Login(TestData.AdminUserName, TestData.AdminPassword);
    public Task<TestClientResult<LoginModel>> LoginManager() => Login(TestData.ManagerUserName, TestData.ManagerPassword);
    public Task<TestClientResult<LoginModel>> LoginUser() => Login(TestData.UserUserName, TestData.UserPassword);

    private async Task<TestClientResult<LoginModel>> Login(string userName, string password) => 
        await apiClient.Auth.Login(new LoginPostModel(userName, password));
}