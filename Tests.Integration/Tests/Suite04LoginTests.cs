using System.Net;
using Api.Models.UserModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Login, 1)]
    public async Task Suite04Login_01LoginAdminReturns200()
    {
        var result = await fixture.LoginHelper.LoginAdmin();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model!.AccessToken.Should().NotBeEmpty();
    }

    [Fact]
    [Order(TestSuite.Login, 2)]
    public async Task Suite04Login_02LoginManagerReturns200()
    {
        var result = await fixture.LoginHelper.LoginManager();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model!.AccessToken.Should().NotBeEmpty();
    }

    [Fact]
    [Order(TestSuite.Login, 3)]
    public async Task Suite04Login_03LoginRegularUserReturns200()
    {
        var result = await fixture.LoginHelper.LoginUser();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model!.AccessToken.Should().NotBeEmpty();
    }
    
    [Fact]
    [Order(TestSuite.Login, 4)]
    public async Task Suite04Login_03RefreshUserReturns200()
    {
        var loginResult = await fixture.LoginHelper.LoginUser();
        var refreshResult = await fixture.ApiClient.Auth.Refresh(new RefreshPostModel(loginResult.Model!.RefreshToken));
        refreshResult.StatusCode.Should().Be(HttpStatusCode.OK);
        refreshResult.Model!.AccessToken.Should().NotBe(loginResult.Model.AccessToken);
    }
}