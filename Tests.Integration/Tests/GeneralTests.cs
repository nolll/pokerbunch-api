using System.Net;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class GeneralTests(TestFixture fixture) : IntegrationTests2(fixture)
{
    [Fact]
    public async Task Root()
    {
        var result = await ApiClient.General.Root();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Version()
    {
        var result = await ApiClient.General.Version();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Swagger()
    {
        var result = await ApiClient.General.Swagger();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}