using System.Net;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.User)]
public class UserTests
{
    [Test]
    [Order(1)]
    public async Task GetUserAsAdmin()
    {
        var result = await TestClient.User.GetAsAdmin(TestData.AdminUserName);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.UserName, Is.EqualTo(TestData.AdminUserName));
        Assert.That(result.Model.DisplayName, Is.EqualTo(TestData.AdminDisplayName));
    }

    [Test]
    [Order(2)]
    public async Task GetUserAsUser()
    {
        var result = await TestClient.User.GetAsUser(TestData.AdminUserName);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.UserName, Is.EqualTo(TestData.AdminUserName));
        Assert.That(result.Model.DisplayName, Is.EqualTo(TestData.AdminDisplayName));
    }

    [Test]
    [Order(3)]
    public async Task ListUsersAsAdmin()
    {
        var result = await TestClient.User.List(TestData.AdminToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Count, Is.EqualTo(3));
        Assert.That(result.Model[0].UserName, Is.EqualTo(TestData.AdminUserName));
        Assert.That(result.Model[0].DisplayName, Is.EqualTo(TestData.AdminDisplayName));
        Assert.That(result.Model[1].UserName, Is.EqualTo(TestData.ManagerUserName));
        Assert.That(result.Model[1].DisplayName, Is.EqualTo(TestData.ManagerDisplayName));
        Assert.That(result.Model[2].UserName, Is.EqualTo(TestData.UserUserName));
        Assert.That(result.Model[2].DisplayName, Is.EqualTo(TestData.UserDisplayName));
    }

    [Test]
    [Order(4)]
    public async Task ListUsersAsUser()
    {
        var result = await TestClient.User.List(TestData.UserToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}