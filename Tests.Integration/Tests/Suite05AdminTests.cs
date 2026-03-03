using System.Net;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Admin, 1)]
    public async Task Suite05Admin_01ClearCacheAsAdmin()
    {
        var token = await fixture.LoginHelper.GetAdminToken();
        var result = await fixture.ApiClient.General.ClearCache(token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.Admin, 2)]
    public async Task Suite05Admin_02ClearCacheAsManager()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.General.ClearCache(managerToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    [Order(TestSuite.Admin, 3)]
    public async Task Suite05Admin_03SendTestEmailAsAdmin()
    {
        var token = await fixture.LoginHelper.GetAdminToken();
        var result = await fixture.ApiClient.General.TestEmail(token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.Admin, 4)]
    public async Task Suite05Admin_04SendTestEmailAsManager()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.General.TestEmail(managerToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}