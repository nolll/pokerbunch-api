using System.Net;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Admin)]
public class Suite05AdminTests
{
    [Test]
    [Order(1)]
    public async Task Test01ClearCacheAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.General.ClearCache(token);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(2)]
    public async Task Test02ClearCacheAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.General.ClearCache(managerToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [Order(3)]
    public async Task Test03SendTestEmailAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.General.TestEmail(token);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(4)]
    public async Task Test04SendTestEmailAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.General.TestEmail(managerToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
    
    [Test]
    [Order(5)]
    public async Task Test05SettingsAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.General.Settings(token);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(6)]
    public async Task Test06SettingsAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.General.Settings(managerToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}