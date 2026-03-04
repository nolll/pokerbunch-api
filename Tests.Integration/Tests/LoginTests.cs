using System.Net;
using Api.Models.UserModels;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class LoginTests(TestFixture fixture) : IntegrationTests2(fixture)
{
    [Fact]
    public async Task Login()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.Auth.Login(new(user.UserName, user.Password));
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model?.AccessToken.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Refresh()
    {
        var user = await Fixture.CreateUser();
        var loginResult = await ApiClient.Auth.Login(new(user.UserName, user.Password));
        var refreshResult = await ApiClient.Auth.Refresh(new RefreshPostModel(loginResult.Model!.RefreshToken));
        refreshResult.StatusCode.Should().Be(HttpStatusCode.OK);
        refreshResult.Model!.AccessToken.Should().NotBe(loginResult.Model.AccessToken);
    }
}