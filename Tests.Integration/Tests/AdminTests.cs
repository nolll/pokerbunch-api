using System.Net;
using Api.Urls.ApiUrls;

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
        var url = new ApiAdminClearCacheUrl().Relative;
        var response = await TestClient.LegacyPost(TestData.AdminToken, url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(2)]
    public async Task ClearCacheAsManager()
    {
        var url = new ApiAdminClearCacheUrl().Relative;
        var response = await TestClient.LegacyPost(TestData.ManagerToken, url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [Order(3)]
    public async Task SendTestEmailAsAdmin()
    {
        var url = new ApiAdminSendEmailUrl().Relative;
        var response = await TestClient.LegacyPost(TestData.AdminToken, url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(4)]
    public async Task SendTestEmailAsManager()
    {
        var url = new ApiAdminSendEmailUrl().Relative;
        var response = await TestClient.LegacyPost(TestData.ManagerToken, url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
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