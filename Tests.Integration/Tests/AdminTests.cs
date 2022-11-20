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
        var response = await TestSetup.AuthorizedClient(TestData.AdminToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(2)]
    public async Task ClearCacheAsManager()
    {
        var url = new ApiAdminClearCacheUrl().Relative;
        var response = await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    [Order(3)]
    public async Task SendTestEmailAsAdmin()
    {
        var url = new ApiAdminSendEmailUrl().Relative;
        var response = await TestSetup.AuthorizedClient(TestData.AdminToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(4)]
    public async Task SendTestEmailAsManager()
    {
        var url = new ApiAdminSendEmailUrl().Relative;
        var response = await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsync(url, null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }
}