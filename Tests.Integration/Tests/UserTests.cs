using System.Net;
using Api.Models.UserModels;

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

    [Test]
    [Order(5)]
    public async Task Profile()
    {
        var result = await TestClient.User.Profile(TestData.UserToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.UserName, Is.EqualTo(TestData.UserUserName));
        Assert.That(result.Model.DisplayName, Is.EqualTo(TestData.UserDisplayName));
    }

    [Test]
    [Order(6)]
    public async Task ChangePassword()
    {
        var parameters = new ChangePasswordPostModel("new_password", TestData.UserPassword);
        var result = await TestClient.User.PasswordChange(TestData.UserToken, parameters);
        Assert.That(result.Success, Is.True);
    }

    [Test]
    [Order(7)]
    public async Task ResetPassword()
    {
        var parameters = new ResetPasswordPostModel(TestData.UserEmail);
        var result = await TestClient.User.PasswordReset(TestData.UserToken, parameters);
        Assert.That(result.Success, Is.True);
        Assert.That(TestSetup.EmailSender.To, Is.EqualTo(TestData.UserEmail));
    }

    [Test]
    [Order(8)]
    public async Task UpdateUser()
    {
        const string displayName = "New Display Name";
        const string realName = "Real Name";
        var parameters = new UpdateUserPostModel(displayName, TestData.UserEmail, realName);
        var result = await TestClient.User.Update(TestData.AdminToken, TestData.UserUserName, parameters);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Model.DisplayName, Is.EqualTo(displayName));
        Assert.That(result.Model.RealName, Is.EqualTo(realName));

        var changeBackParameters = new UpdateUserPostModel(TestData.UserDisplayName, TestData.UserEmail, null);
        var changeBackResult = await TestClient.User.Update(TestData.AdminToken, TestData.UserUserName, changeBackParameters);
    }
}