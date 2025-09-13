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
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model?.AccessToken, Is.Not.Empty);
    }

    [Test]
    [Order(2)]
    public async Task Test02LoginManagerReturns200()
    {
        var result = await LoginHelper.LoginManager();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model?.AccessToken, Is.Not.Empty);
    }

    [Test]
    [Order(3)]
    public async Task Test03LoginRegularUserReturns200()
    {
        var result = await LoginHelper.LoginUser();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model?.AccessToken, Is.Not.Empty);
    }
}