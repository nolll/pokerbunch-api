using System.Net;
using Api.Models.UserModels;
using Api.Urls.ApiUrls;
using Infrastructure.Sql;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.UserRegistration)]
public class UserRegistrationTests
{
    [Test]
    [Order(1)]
    public async Task RegisterAdminReturns200()
    {
        var response = await RegisterUser(TestData.AdminUserName, TestData.AdminDisplayName, TestData.AdminEmail, TestData.AdminPassword);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var db = new PostgresStorageProvider(TestSetup.ConnectionString);
        await db.ExecuteAsync("UPDATE pb_user SET role_id = 3 WHERE user_id = 1");
    }

    [Test]
    [Order(2)]
    public async Task RegisterManagerReturns200()
    {
        var response = await RegisterUser(TestData.ManagerUserName, TestData.ManagerDisplayName, TestData.ManagerEmail, TestData.ManagerPassword);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(3)]
    public async Task RegisterRegularUserReturns200()
    {
        var response = await RegisterUser(TestData.UserUserName, TestData.UserDisplayName, TestData.UserEmail, TestData.UserPassword);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task<HttpResponseMessage> RegisterUser(string userName, string displayName, string email, string password)
    {
        var parameters = new AddUserPostModel(userName, displayName, email, password);
        return await TestClient.LegacyPost(new ApiUserAddUrl().Relative, parameters);
    }
}