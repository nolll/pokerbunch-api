using System.Net;
using Api.Models.EventModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Event, 1)]
    public async Task Suite08_Event01AddEvent()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var parameters = new EventAddPostModel(TestData.EventName);
        var result = await fixture.ApiClient.Event.Add(managerToken, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(TestData.EventId);
    }

    [Fact]
    [Order(TestSuite.Event, 2)]
    public async Task Suite08_Event02ListEvents()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Event.List(managerToken, TestData.BunchId);

        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(1);
        var @event = result.Model?[0];
        @event!.Id.Should().Be(TestData.EventId);
        @event.Name.Should().Be(TestData.EventName);
        @event.BunchId.Should().Be(TestData.BunchId);
    }

    [Fact]
    [Order(TestSuite.Event, 3)]
    public async Task Suite08_Event03GetEvent()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Event.Get(managerToken, TestData.EventId);

        result.Success.Should().BeTrue();
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(TestData.EventId);
        result.Model.Name.Should().Be(TestData.EventName);
        result.Model.BunchId.Should().Be(TestData.BunchId);
    }
}