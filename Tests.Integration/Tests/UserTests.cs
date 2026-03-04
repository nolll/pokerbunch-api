using System.Net;
using Api.Models.UserModels;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class UserTests(TestFixture fixture) : IntegrationTests2(fixture)
{
    [Fact]
    public async Task GetUserAsAdmin()
    {
        var admin = await Fixture.CreateUser();
        await admin.AsAdmin();
        
        var result = await ApiClient.User.GetAsAdmin(admin.Token, admin.UserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(admin.UserName);
        result.Model.DisplayName.Should().Be(admin.DisplayName);
    }

    [Fact]
    public async Task GetUserAsUser()
    {
        var user = await Fixture.CreateUser();
        
        var result = await ApiClient.User.GetAsAdmin(user.Token, user.UserName);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(user.UserName);
        result.Model.DisplayName.Should().Be(user.DisplayName);
    }

    [Fact]
    public async Task ListUsersAsAdmin()
    {
        var user = await Fixture.CreateUser();
        var admin = await Fixture.CreateUser();
        await admin.AsAdmin();
        
        var result = await ApiClient.User.List(admin.Token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(2);
        
        var adminUser = result.Model.First(o => o.UserName == admin.UserName);
        adminUser.UserName.Should().Be(admin.UserName);
        adminUser.DisplayName.Should().Be(admin.DisplayName);
        
        var other = result.Model.First(o => o.UserName == user.UserName);
        other.UserName.Should().Be(user.UserName);
        other.DisplayName.Should().Be(user.DisplayName);
        
        List<string> orderedNames =
        [
            user.DisplayName,
            admin.DisplayName
        ];
        orderedNames.Sort();
        
        result.Model.Select(o => o.DisplayName).Should().BeEquivalentTo(orderedNames);
    }

    [Fact]
    public async Task ListUsersAsUser()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.User.List(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Profile()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.User.Profile(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.UserName.Should().Be(user.UserName);
        result.Model.DisplayName.Should().Be(user.DisplayName);
    }

    [Fact]
    public async Task ChangePassword()
    {
        var user = await Fixture.CreateUser();
        var parameters = new ChangePasswordPostModel(Data.String(), user.Password);
        var result = await ApiClient.User.PasswordChange(user.Token, parameters);
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task ResetPassword()
    {
        var user = await Fixture.CreateUser();
        var parameters = new ResetPasswordPostModel(user.Email);
        var result = await ApiClient.User.PasswordReset(parameters);
        result.Success.Should().BeTrue();
        EmailSender.LastSentTo.Should().Be(user.Email);
    }

    [Fact]
    public async Task UpdateUser()
    {
        var user = await Fixture.CreateUser();
        var displayName = Data.String();
        var realName = Data.String();
        var email = Data.EmailAddress();
        var parameters = new UpdateUserPostModel(displayName, email, realName);
        var result = await ApiClient.User.Update(user.Token, user.UserName, parameters);
        result.Success.Should().BeTrue();
        result.Model!.DisplayName.Should().Be(displayName);
        result.Model!.RealName.Should().Be(realName);
        result.Model!.Email.Should().Be(email);
    }
}