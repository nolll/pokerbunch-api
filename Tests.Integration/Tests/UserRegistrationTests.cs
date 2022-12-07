using System.Net;
using Api.Models.UserModels;

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
        var parameters = new AddUserPostModel(TestData.AdminUserName, TestData.AdminDisplayName, TestData.AdminEmail, TestData.AdminPassword);
        var result = await TestClient.User.Add(parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        await TestSetup.Db.Execute("UPDATE pb_user SET role_id = 3 WHERE user_id = 1");
    }
    
    [Test]
    [Order(2)]
    public async Task RegisterManagerReturns200()
    {
        var parameters = new AddUserPostModel(TestData.ManagerUserName, TestData.ManagerDisplayName, TestData.ManagerEmail, TestData.ManagerPassword);
        var result = await TestClient.User.Add(parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(3)]
    public async Task RegisterRegularUserReturns200()
    {
        var parameters = new AddUserPostModel(TestData.UserUserName, TestData.UserDisplayName, TestData.UserEmail, TestData.UserPassword);
        var result = await TestClient.User.Add(parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    }