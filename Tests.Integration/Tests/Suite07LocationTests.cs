using System.Net;
using Api.Models.LocationModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Location, 1)]
    public async Task Suite07Location_01AddLocation()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var parameters = new LocationAddPostModel(TestData.BunchLocationName);
        var result = await fixture.ApiClient.Location.Add(managerToken, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(TestData.BunchLocationId);
    }

    [Fact]
    [Order(TestSuite.Location, 2)]
    public async Task Suite07Location_02ListLocations()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Location.List(managerToken, TestData.BunchId);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(1);
        var location = result.Model?[0];
        location!.Id.Should().Be(TestData.BunchLocationId);
        location.Name.Should().Be(TestData.BunchLocationName);
        location.Bunch.Should().Be(TestData.BunchId);
    }

    [Fact]
    [Order(TestSuite.Location, 3)]
    public async Task Suite07Location_03GetLocation()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Location.Get(managerToken, TestData.BunchLocationId);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(TestData.BunchLocationId);
        result.Model.Name.Should().Be(TestData.BunchLocationName);
        result.Model.Bunch.Should().Be(TestData.BunchId);
    }
}