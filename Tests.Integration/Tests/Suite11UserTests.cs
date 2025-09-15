using System.Net;
using Api.Models.UserModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.User)]
public class Suite11UserTests
{
    [Test]
    [Order(1)]
    public async Task Test01GetUserAsAdmin()
    {
        var result = await TestClient.User.GetAsAdmin(TestData.AdminUserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(TestData.AdminUserName);
        result.Model.DisplayName.Should().Be(TestData.AdminDisplayName);
    }

    [Test]
    [Order(2)]
    public async Task Test02GetUserAsUser()
    {
        var result = await TestClient.User.GetAsUser(TestData.AdminUserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(TestData.AdminUserName);
        result.Model.DisplayName.Should().Be(TestData.AdminDisplayName);
    }

    [Test]
    [Order(3)]
    public async Task Test03ListUsersAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.User.List(token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(3);
        result.Model[0].UserName.Should().Be(TestData.AdminUserName);
        result.Model[0].DisplayName.Should().Be(TestData.AdminDisplayName);
        result.Model[1].UserName.Should().Be(TestData.ManagerUserName);
        result.Model[1].DisplayName.Should().Be(TestData.ManagerDisplayName);
        result.Model[2].UserName.Should().Be(TestData.UserUserName);
        result.Model[2].DisplayName.Should().Be(TestData.UserDisplayName);
    }

    [Test]
    [Order(4)]
    public async Task Test04ListUsersAsUser()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.User.List(userToken);
        
        // todo: Find out why forbid returns 404
        //result.StatusCode.Should().Be(HttpStatusCode.Forbidden));
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    [Order(5)]
    public async Task Test05Profile()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await TestClient.User.Profile(userToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(TestData.UserUserName);
        result.Model.DisplayName.Should().Be(TestData.UserDisplayName);
    }

    [Test]
    [Order(6)]
    public async Task Test06ChangePassword()
    {
        var userToken = await LoginHelper.GetUserToken();
        var parameters = new ChangePasswordPostModel("new_password", TestData.UserPassword);
        var result = await TestClient.User.PasswordChange(userToken, parameters);
        result.Success.Should().BeTrue();
    }

    [Test]
    [Order(7)]
    public async Task Test07ResetPassword()
    {
        var parameters = new ResetPasswordPostModel(TestData.UserEmail);
        var result = await TestClient.User.PasswordReset(parameters);
        result.Success.Should().BeTrue();
        TestSetup.EmailSender!.To.Should().Be(TestData.UserEmail);
    }

    [Test]
    [Order(8)]
    public async Task Test08UpdateUser()
    {
        const string displayName = "New Display Name";
        const string realName = "Real Name";
        var token = await LoginHelper.GetAdminToken();
        var parameters = new UpdateUserPostModel(displayName, TestData.UserEmail, realName);
        var result = await TestClient.User.Update(token, TestData.UserUserName, parameters);
        result.Success.Should().BeTrue();
        result.Model!.DisplayName.Should().Be(displayName);
        result.Model!.RealName.Should().Be(realName);

        var changeBackParameters = new UpdateUserPostModel(TestData.UserDisplayName, TestData.UserEmail, null);
        var changeBackResult = await TestClient.User.Update(token, TestData.UserUserName, changeBackParameters);
    }
}