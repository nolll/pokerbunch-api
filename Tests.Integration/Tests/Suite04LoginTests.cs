using System.Net;
using Api.Models.UserModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Login, 1)]
    public async Task Suite04Login_01LoginReturns200()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.Auth.Login(new(user.UserName, user.Password));
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model?.AccessToken.Should().NotBeEmpty();
    }
    
    [Fact]
    [Order(TestSuite.Login, 2)]
    public async Task Suite04Login_02RefreshUserReturns200()
    {
        var user = await Fixture.CreateUser();
        var loginResult = await ApiClient.Auth.Login(new(user.UserName, user.Password));
        var refreshResult = await ApiClient.Auth.Refresh(new RefreshPostModel(loginResult.Model!.RefreshToken));
        refreshResult.StatusCode.Should().Be(HttpStatusCode.OK);
        refreshResult.Model!.AccessToken.Should().NotBe(loginResult.Model.AccessToken);
    }
}