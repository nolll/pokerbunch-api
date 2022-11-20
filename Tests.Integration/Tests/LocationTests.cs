using System.Net;
using System.Text.Json;
using Api.Models.LocationModels;
using Api.Urls.ApiUrls;

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
        var url = new ApiLocationAddUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<LocationModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(TestData.BunchLocationId));
    }

    [Test]
    [Order(2)]
    public async Task ListLocations()
    {
        var url = new ApiLocationListUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.ManagerToken).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<List<LocationModel>>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        var location = result[0];
        Assert.That(location.Id, Is.EqualTo(TestData.BunchLocationId));
        Assert.That(location.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(location.Bunch, Is.EqualTo(TestData.BunchId));
    }

    [Test]
    [Order(3)]
    public async Task GetLocation()
    {
        var url = new ApiLocationUrl(TestData.BunchLocationId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.ManagerToken).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<LocationModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(TestData.BunchLocationId));
        Assert.That(result.Name, Is.EqualTo(TestData.BunchLocationName));
        Assert.That(result.Bunch, Is.EqualTo(TestData.BunchId));
    }
}