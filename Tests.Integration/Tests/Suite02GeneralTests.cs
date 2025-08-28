using System.Net;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.General)]
public class Suite02GeneralTests
{
    [Test]
    [Order(1)]
    public async Task Test01Root()
    {
        var result = await TestClient.General.Root();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(2)]
    public async Task Test02Version()
    {
        var result = await TestClient.General.Version();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [Order(3)]
    public async Task Test03Swagger()
    {
        var result = await TestClient.General.Swagger();
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}