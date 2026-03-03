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
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new LocationAddPostModel(Data.BunchLocationName);
        var result = await ApiClient.Location.Add(managerToken, Data.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(Data.BunchLocationId);
    }

    [Fact]
    [Order(TestSuite.Location, 2)]
    public async Task Suite07Location_02ListLocations()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Location.List(managerToken, Data.BunchId);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(1);
        var location = result.Model?[0];
        location!.Id.Should().Be(Data.BunchLocationId);
        location.Name.Should().Be(Data.BunchLocationName);
        location.Bunch.Should().Be(Data.BunchId);
    }

    [Fact]
    [Order(TestSuite.Location, 3)]
    public async Task Suite07Location_03GetLocation()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Location.Get(managerToken, Data.BunchLocationId);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(Data.BunchLocationId);
        result.Model.Name.Should().Be(Data.BunchLocationName);
        result.Model.Bunch.Should().Be(Data.BunchId);
    }
}