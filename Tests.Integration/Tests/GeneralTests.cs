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
    public async Task VersionReturns200()
    {
        var response = await TestSetup.Client.GetAsync(new ApiVersionUrl().Relative);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}