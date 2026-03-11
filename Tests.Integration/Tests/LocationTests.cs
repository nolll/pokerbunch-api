using System.Net;
using Api.Models.LocationModels;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class LocationTests(TestFixture fixture) : IntegrationTests(fixture)
{
    [Fact]
    public async Task AddLocation()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var parameters = new LocationAddPostModel(Data.String());
        var result = await ApiClient.Location.Add(manager.Token, bunch.Id, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(parameters.Name);
    }

    [Fact]
    public async Task ListLocations()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var locationFixture = await bunch.AddLocation();
        
        var result = await ApiClient.Location.List(manager.Token, bunch.Id);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(1);
        var location = result.Model[0];
        location.Id.Should().Be(locationFixture.Id);
        location.Name.Should().Be(locationFixture.Name);
        location.Bunch.Should().Be(locationFixture.BunchId);
    }

    [Fact]
    public async Task GetLocation()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var locationFixture = await bunch.AddLocation();
        
        var result = await ApiClient.Location.Get(manager.Token, locationFixture.Id);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(locationFixture.Id);
        result.Model.Name.Should().Be(locationFixture.Name);
        result.Model.Bunch.Should().Be(locationFixture.BunchId);
    }
}