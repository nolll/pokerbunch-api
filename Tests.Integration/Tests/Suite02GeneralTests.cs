using System.Net;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.General, 1)]
    public async Task Suite02General_01Root()
    {
        var result = await fixture.ApiClient.General.Root();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.General, 2)]
    public async Task Suite02General_02Version()
    {
        var result = await fixture.ApiClient.General.Version();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Order(TestSuite.General, 3)]
    public async Task Suite02General_03Swagger()
    {
        var result = await fixture.ApiClient.General.Swagger();
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}