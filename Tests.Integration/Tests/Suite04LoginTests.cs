using System.Net;

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
}