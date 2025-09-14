using System.Net;
using Api.Models.UserModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Login)]
public class Suite04LoginTests
{
    [Test]
    [Order(1)]
    public async Task Test01LoginAdminReturns200()
    {
        var result = await LoginHelper.LoginAdmin();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model?.AccessToken.Should().NotBeEmpty();
    }

    [Test]
    [Order(2)]
    public async Task Test02LoginManagerReturns200()
    {
        var result = await LoginHelper.LoginManager();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model?.AccessToken.Should().NotBeEmpty();
    }

    [Test]
    [Order(3)]
    public async Task Test03LoginRegularUserReturns200()
    {
        var result = await LoginHelper.LoginUser();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model?.AccessToken.Should().NotBeEmpty();
    }
    
    [Test]
    [Order(4)]
    public async Task Test03RefreshUserReturns200()
    {
        var loginResult = await LoginHelper.LoginUser();
        var refreshResult = await TestClient.Auth.Refresh(new RefreshPostModel(loginResult.Model!.RefreshToken));
        refreshResult.StatusCode.Should().Be(HttpStatusCode.OK);
        refreshResult.Model?.AccessToken.Should().NotBe(loginResult.Model.AccessToken);
    }
}