using System.Net;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class AdminTests(TestFixture fixture) : IntegrationTests(fixture)
{
    [Fact]
    public async Task ClearCacheAsAdmin()
    {
        var user = await Fixture.CreateUser();
        await user.AsAdmin();
        var result = await ApiClient.General.ClearCache(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ClearCacheAsPlayer()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.General.ClearCache(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task SendTestEmailAsAdmin()
    {
        var user = await Fixture.CreateUser();
        await user.AsAdmin();
        var result = await ApiClient.General.TestEmail(user.Token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SendTestEmailAsPlayer()
    {
        var user = await Fixture.CreateUser();
        var result = await ApiClient.General.TestEmail(user.Token);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}