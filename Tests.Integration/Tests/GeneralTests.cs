using System.Net;

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
        var result = await TestClient.General.Root();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Redirect));
    }

    [Test]
    [Order(2)]
    public async Task Version()
    {
        var result = await TestClient.General.Version();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(3)]
    public async Task Swagger()
    {
        var result = await TestClient.General.Swagger();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}