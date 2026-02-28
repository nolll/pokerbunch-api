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
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    [Order(2)]
    public async Task Test02ClearCacheAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.General.ClearCache(managerToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Test]
    [Order(3)]
    public async Task Test03SendTestEmailAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await TestClient.General.TestEmail(token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    [Order(4)]
    public async Task Test04SendTestEmailAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.General.TestEmail(managerToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}