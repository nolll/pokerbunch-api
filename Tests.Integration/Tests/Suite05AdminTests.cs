using System.Net;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Admin, 1)]
    public async Task Suite05Admin_01ClearCacheAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await ApiClient.General.ClearCache(token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.Admin, 2)]
    public async Task Suite05Admin_02ClearCacheAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.General.ClearCache(managerToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    [Order(TestSuite.Admin, 3)]
    public async Task Suite05Admin_03SendTestEmailAsAdmin()
    {
        var token = await LoginHelper.GetAdminToken();
        var result = await ApiClient.General.TestEmail(token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.Admin, 4)]
    public async Task Suite05Admin_04SendTestEmailAsManager()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.General.TestEmail(managerToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}