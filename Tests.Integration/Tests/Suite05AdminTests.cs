using System.Net;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Admin, 1)]
    public async Task Suite05Admin_01ClearCacheAsAdmin()
    {
        var user = await Fixture.CreateUser();
        await user.AsAdmin();
        var result = await ApiClient.General.ClearCache(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.Admin, 2)]
    public async Task Suite05Admin_02ClearCacheAsPlayer()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.General.ClearCache(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    [Order(TestSuite.Admin, 3)]
    public async Task Suite05Admin_03SendTestEmailAsAdmin()
    {
        var user = await Fixture.CreateUser();
        await user.AsAdmin();
        var result = await ApiClient.General.TestEmail(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.Admin, 4)]
    public async Task Suite05Admin_04SendTestEmailAsPlayer()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.General.TestEmail(user.Token);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}