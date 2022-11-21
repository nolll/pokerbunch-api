using System.Net;
using Api.Models.LocationModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Location)]
public class LocationTests
{
    [Test]
    [Order(1)]
    public async Task AddLocation()
    {
        var parameters = new LocationAddPostModel(TestData.BunchLocationName);
        var result = await TestClient.Location.Add(TestData.ManagerToken, TestData.BunchId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Id, Is.EqualTo(TestData.BunchLocationIdInt));
    }

    [Test]
    [Order(2)]
    public async Task ListLocations()
    {
        var result = await TestClient.Location.List(TestData.ManagerToken, TestData.BunchId);
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Count, Is.EqualTo(1));
        var location = result.Model[0];
        Assert.That(location.Id, Is.EqualTo(TestData.BunchLocationIdInt));
        Assert.That(location.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(location.Bunch, Is.EqualTo(TestData.BunchId));
    }

    [Test]
    [Order(3)]
    public async Task GetLocation()
    {
        var result = await TestClient.Location.Get(TestData.ManagerToken, TestData.BunchLocationIdString);
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Id, Is.EqualTo(TestData.BunchLocationIdInt));
        Assert.That(result.Model.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(result.Model.Bunch, Is.EqualTo(TestData.BunchId));
    }
}