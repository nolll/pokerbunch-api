using System.Net;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Admin)]
public class AdminTests
{
    [Test]
    [Order(1)]
    public async Task ClearCacheAsAdmin()
    {
        var result = await TestClient.General.ClearCache(TestData.AdminToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(2)]
    public async Task ClearCacheAsManager()
    {
        var result = await TestClient.General.ClearCache(TestData.ManagerToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [Order(3)]
    public async Task SendTestEmailAsAdmin()
    {
        var result = await TestClient.General.TestEmail(TestData.AdminToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(4)]
    public async Task SendTestEmailAsManager()
    {
        var result = await TestClient.General.TestEmail(TestData.ManagerToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
    
    [Test]
    [Order(5)]
    public async Task SettingsAsAdmin()
    {
        var result = await TestClient.General.Settings(TestData.AdminToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(6)]
    public async Task SettingsAsManager()
    {
        var result = await TestClient.General.Settings(TestData.ManagerToken);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}