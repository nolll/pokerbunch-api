using System.Net;
using Api.Models.EventModels;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class EventTests(TestFixture fixture) : IntegrationTests(fixture)
{
    [Fact]
    public async Task AddEvent()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var parameters = new EventAddPostModel(Data.String());
        var result = await ApiClient.Event.Add(manager.Token, bunch.Id, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(parameters.Name);
    }

    [Fact]
    public async Task ListEvents()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var eventFixture = await bunch.AddEvent();
        
        var result = await ApiClient.Event.List(manager.Token, bunch.Id);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(1);
        var @event = result.Model[0];
        @event.Id.Should().Be(eventFixture.Id);
        @event.Name.Should().Be(eventFixture.Name);
        @event.BunchId.Should().Be(eventFixture.BunchId);
    }

    [Fact]
    public async Task GetEvent()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var eventFixture = await bunch.AddEvent();
        
        var result = await ApiClient.Event.Get(manager.Token, eventFixture.Id);

        result.Success.Should().BeTrue();
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(eventFixture.Id);
        result.Model.Name.Should().Be(eventFixture.Name);
        result.Model.BunchId.Should().Be(eventFixture.BunchId);
    }
}