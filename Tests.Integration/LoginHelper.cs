using Api.Models.UserModels;

namespace Tests.Integration;

public static class LoginHelper
{
    public static Task<string> GetAdminToken() => GetToken(TestData.AdminUserName, TestData.AdminPassword);
    public static Task<string> GetManagerToken() => GetToken(TestData.ManagerUserName, TestData.ManagerPassword);
    public static Task<string> GetUserToken() => GetToken(TestData.UserUserName, TestData.UserPassword);

    private static async Task<string> GetToken(string userName, string password) => 
        (await Login(userName, password)).Model ?? "";

    public static Task<TestClientResult<string>> LoginAdmin() => Login(TestData.AdminUserName, TestData.AdminPassword);
    public static Task<TestClientResult<string>> LoginManager() => Login(TestData.ManagerUserName, TestData.ManagerPassword);
    public static Task<TestClientResult<string>> LoginUser() => Login(TestData.UserUserName, TestData.UserPassword);

    private static async Task<TestClientResult<string>> Login(string userName, string password) => 
        await TestClient.Auth.Login(new LoginPostModel(userName, password));
}