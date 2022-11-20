using System.Net;
using Api.Urls.ApiUrls;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.General)]
public class GeneralTests
{
    [Test]
    [Order(1)]
    public async Task Root()
    {
        var response = await TestSetup.Client.GetAsync(new ApiRootUrl().Relative);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(2)]
    public async Task Version()
    {
        var response = await TestSetup.Client.GetAsync(new ApiVersionUrl().Relative);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(3)]
    public async Task Swagger()
    {
        var response = await TestSetup.Client.GetAsync(new ApiSwaggerUrl().Relative);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}