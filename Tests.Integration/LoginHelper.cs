using Api.Models.UserModels;

namespace Tests.Integration;

public static class LoginHelper
{
    public static async Task<string> GetAdminToken() => (await GetTokens(TestData.AdminUserName, TestData.AdminPassword)).AccessToken;
    public static async Task<string> GetManagerToken() => (await GetTokens(TestData.ManagerUserName, TestData.ManagerPassword)).AccessToken;
    public static async Task<string> GetUserToken() => (await GetTokens(TestData.UserUserName, TestData.UserPassword)).AccessToken;

    private static async Task<LoginModel> GetTokens(string userName, string password) => 
        (await Login(userName, password)).Model!;

    public static Task<TestClientResult<LoginModel>> LoginAdmin() => Login(TestData.AdminUserName, TestData.AdminPassword);
    public static Task<TestClientResult<LoginModel>> LoginManager() => Login(TestData.ManagerUserName, TestData.ManagerPassword);
    public static Task<TestClientResult<LoginModel>> LoginUser() => Login(TestData.UserUserName, TestData.UserPassword);

    private static async Task<TestClientResult<LoginModel>> Login(string userName, string password) => 
        await TestClient.Auth.Login(new LoginPostModel(userName, password));
}