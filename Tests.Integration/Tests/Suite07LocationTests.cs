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
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Id, Is.EqualTo(TestData.BunchLocationId));
    }

    [Test]
    [Order(2)]
    public async Task Test02ListLocations()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Location.List(managerToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Count, Is.EqualTo(1));
        var location = result.Model?[0];
        Assert.That(location?.Id, Is.EqualTo(TestData.BunchLocationId));
        Assert.That(location?.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(location?.Bunch, Is.EqualTo(TestData.BunchId));
    }

    [Test]
    [Order(3)]
    public async Task Test03GetLocation()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Location.Get(managerToken, TestData.BunchLocationId);
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Id, Is.EqualTo(TestData.BunchLocationId));
        Assert.That(result.Model?.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(result.Model?.Bunch, Is.EqualTo(TestData.BunchId));
    }
}