using System.Net;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Login)]
public class LoginTests
{
    [Test]
    [Order(1)]
    public async Task LoginAdminReturns200()
    {
        var response = await Login(TestData.AdminUserName, TestData.AdminPassword);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var token = await GetToken(response);
        Assert.That(token, Is.Not.Empty);

        TestData.AdminToken = token;
    }

    [Test]
    [Order(2)]
    public async Task LoginManagerReturns200()
    {
        var response = await Login(TestData.ManagerUserName, TestData.ManagerPassword);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var token = await GetToken(response);
        Assert.That(token, Is.Not.Empty);

        TestData.ManagerToken = token;
    }

    [Test]
    [Order(3)]
    public async Task LoginRegularUserReturns200()
    {
        var response = await Login(TestData.UserUserName, TestData.UserPassword);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var token = await GetToken(response);
        Assert.That(token, Is.Not.Empty);

        TestData.UserToken = token;
    }

    private async Task<HttpResponseMessage> Login(string userName, string password)
    {
        var parameters = new LoginPostModel(userName, password);
        return await TestClient.Post(new ApiLoginUrl().Relative, parameters);
    }

    private async Task<string> GetToken(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStringAsync();
    }
}