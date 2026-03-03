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
        var token = await LoginHelper.GetAdminToken();
        var result = await ApiClient.User.GetAsAdmin(token, Data.AdminUserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(Data.AdminUserName);
        result.Model.DisplayName.Should().Be(Data.AdminDisplayName);
    }

    [Fact]
    [Order(TestSuite.User, 2)]
    public async Task Suite11User_02GetUserAsUser()
    {
        var token = await LoginHelper.GetUserToken();
        var result = await ApiClient.User.GetAsUser(token, Data.AdminUserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(Data.AdminUserName);
        result.Model.DisplayName.Should().Be(Data.AdminDisplayName);
    }

    [Fact]
    [Order(TestSuite.User, 3)]
    public async Task Suite11User_03ListUsersAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await ApiClient.User.List(token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(3);
        result.Model[0].UserName.Should().Be(Data.AdminUserName);
        result.Model[0].DisplayName.Should().Be(Data.AdminDisplayName);
        result.Model[1].UserName.Should().Be(Data.ManagerUserName);
        result.Model[1].DisplayName.Should().Be(Data.ManagerDisplayName);
        result.Model[2].UserName.Should().Be(Data.UserUserName);
        result.Model[2].DisplayName.Should().Be(Data.UserDisplayName);
    }

    [Fact]
    [Order(TestSuite.User, 4)]
    public async Task Suite11User_04ListUsersAsUser()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.User.List(userToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    [Order(TestSuite.User, 5)]
    public async Task Suite11User_05Profile()
    {
        var userToken = await LoginHelper.GetUserToken();
        var result = await ApiClient.User.Profile(userToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(Data.UserUserName);
        result.Model.DisplayName.Should().Be(Data.UserDisplayName);
    }

    [Fact]
    [Order(TestSuite.User, 6)]
    public async Task Suite11User_06ChangePassword()
    {
        var userToken = await LoginHelper.GetUserToken();
        var parameters = new ChangePasswordPostModel("new_password", Data.UserPassword);
        var result = await ApiClient.User.PasswordChange(userToken, parameters);
        result.Success.Should().BeTrue();
    }

    [Fact]
    [Order(TestSuite.User, 7)]
    public async Task Suite11User_07ResetPassword()
    {
        var parameters = new ResetPasswordPostModel(Data.UserEmail);
        var result = await ApiClient.User.PasswordReset(parameters);
        result.Success.Should().BeTrue();
        EmailSender!.LastSentTo.Should().Be(Data.UserEmail);
    }

    [Fact]
    [Order(TestSuite.User, 8)]
    public async Task Suite11User_08UpdateUser()
    {
        var displayName = DataFactory.String();
        var realName = DataFactory.String();
        var token = await LoginHelper.GetAdminToken();
        var parameters = new UpdateUserPostModel(displayName, Data.UserEmail, realName);
        var result = await ApiClient.User.Update(token, Data.UserUserName, parameters);
        result.Success.Should().BeTrue();
        result.Model!.DisplayName.Should().Be(displayName);
        result.Model!.RealName.Should().Be(realName);

        var changeBackParameters = new UpdateUserPostModel(Data.UserDisplayName, Data.UserEmail, null);
        var changeBackResult = await ApiClient.User.Update(token, Data.UserUserName, changeBackParameters);
    }
}