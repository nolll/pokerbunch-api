using System.Net;
using Api.Models.LocationModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Location)]
public class Suite07LocationTests
{
    [Test]
    [Order(1)]
    public async Task Test01AddLocation()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new LocationAddPostModel(TestData.BunchLocationName);
        var result = await TestClient.Location.Add(managerToken, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model?.Id.Should().Be(TestData.BunchLocationId);
    }

    [Test]
    [Order(2)]
    public async Task Test02ListLocations()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Location.List(managerToken, TestData.BunchId);
        result.Model.Should().NotBeNull();
        result.Model?.Count.Should().Be(1);
        var location = result.Model?[0];
        location?.Id.Should().Be(TestData.BunchLocationId);
        location?.Name.Should().Be(TestData.BunchLocationName);
        location?.Bunch.Should().Be(TestData.BunchId);
    }

    [Test]
    [Order(3)]
    public async Task Test03GetLocation()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Location.Get(managerToken, TestData.BunchLocationId);
        result.Model.Should().NotBeNull();
        result.Model?.Id.Should().Be(TestData.BunchLocationId);
        result.Model?.Name.Should().Be(TestData.BunchLocationName);
        result.Model?.Bunch.Should().Be(TestData.BunchId);
    }
}