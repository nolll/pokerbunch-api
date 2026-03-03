using System.Net;
using Api.Models.UserModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.User, 1)]
    public async Task Suite11User_01GetUserAsAdmin()
    {
        var token = await fixture.LoginHelper.GetAdminToken();
        var result = await fixture.ApiClient.User.GetAsAdmin(token, TestData.AdminUserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(TestData.AdminUserName);
        result.Model.DisplayName.Should().Be(TestData.AdminDisplayName);
    }

    [Fact]
    [Order(TestSuite.User, 2)]
    public async Task Suite11User_02GetUserAsUser()
    {
        var token = await fixture.LoginHelper.GetUserToken();
        var result = await fixture.ApiClient.User.GetAsUser(token, TestData.AdminUserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(TestData.AdminUserName);
        result.Model.DisplayName.Should().Be(TestData.AdminDisplayName);
    }

    [Fact]
    [Order(TestSuite.User, 3)]
    public async Task Suite11User_03ListUsersAsAdmin()
    {
        var token = await fixture.LoginHelper.GetAdminToken();
        var result = await fixture.ApiClient.User.List(token);
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

    [Fact]
    [Order(TestSuite.User, 4)]
    public async Task Suite11User_04ListUsersAsUser()
    {
        var userToken = await fixture.LoginHelper.GetUserToken();
        var result = await fixture.ApiClient.User.List(userToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    [Order(TestSuite.User, 5)]
    public async Task Suite11User_05Profile()
    {
        var userToken = await fixture.LoginHelper.GetUserToken();
        var result = await fixture.ApiClient.User.Profile(userToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(TestData.UserUserName);
        result.Model.DisplayName.Should().Be(TestData.UserDisplayName);
    }

    [Fact]
    [Order(TestSuite.User, 6)]
    public async Task Suite11User_06ChangePassword()
    {
        var userToken = await fixture.LoginHelper.GetUserToken();
        var parameters = new ChangePasswordPostModel("new_password", TestData.UserPassword);
        var result = await fixture.ApiClient.User.PasswordChange(userToken, parameters);
        result.Success.Should().BeTrue();
    }

    [Fact]
    [Order(TestSuite.User, 7)]
    public async Task Suite11User_07ResetPassword()
    {
        var parameters = new ResetPasswordPostModel(TestData.UserEmail);
        var result = await fixture.ApiClient.User.PasswordReset(parameters);
        result.Success.Should().BeTrue();
        fixture.EmailSender!.LastSentTo.Should().Be(TestData.UserEmail);
    }

    [Fact]
    [Order(TestSuite.User, 8)]
    public async Task Suite11User_08UpdateUser()
    {
        const string displayName = "New Display Name";
        const string realName = "Real Name";
        var token = await fixture.LoginHelper.GetAdminToken();
        var parameters = new UpdateUserPostModel(displayName, TestData.UserEmail, realName);
        var result = await fixture.ApiClient.User.Update(token, TestData.UserUserName, parameters);
        result.Success.Should().BeTrue();
        result.Model!.DisplayName.Should().Be(displayName);
        result.Model!.RealName.Should().Be(realName);

        var changeBackParameters = new UpdateUserPostModel(TestData.UserDisplayName, TestData.UserEmail, null);
        var changeBackResult = await fixture.ApiClient.User.Update(token, TestData.UserUserName, changeBackParameters);
    }
}