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
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var parameters = new LocationAddPostModel(DataFactory.String());
        var result = await ApiClient.Location.Add(manager.Token, bunch.Id, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(parameters.Name);
    }

    [Fact]
    [Order(TestSuite.Location, 2)]
    public async Task Suite07Location_02ListLocations()
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
    [Order(TestSuite.Location, 3)]
    public async Task Suite07Location_03GetLocation()
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