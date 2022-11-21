using System.Net;
using Api.Models.UserModels;

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
        var result = await Login(TestData.AdminUserName, TestData.AdminPassword);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Empty);
        TestData.AdminToken = result.Model;
    }

    [Test]
    [Order(2)]
    public async Task LoginManagerReturns200()
    {
        var result = await Login(TestData.ManagerUserName, TestData.ManagerPassword);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Empty);

        TestData.ManagerToken = result.Model;
    }

    [Test]
    [Order(3)]
    public async Task LoginRegularUserReturns200()
    {
        var result = await Login(TestData.UserUserName, TestData.UserPassword);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Empty);
        TestData.UserToken = result.Model;
    }

    private async Task<TestClientResult<string>> Login(string userName, string password)
    {
        var parameters = new LoginPostModel(userName, password);
        return await TestClient.Auth.Login(parameters);
    }
}