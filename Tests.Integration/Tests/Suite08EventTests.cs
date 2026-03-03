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
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new EventAddPostModel(Data.EventName);
        var result = await ApiClient.Event.Add(managerToken, Data.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(Data.EventId);
    }

    [Fact]
    [Order(TestSuite.Event, 2)]
    public async Task Suite08_Event02ListEvents()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Event.List(managerToken, Data.BunchId);

        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(1);
        var @event = result.Model?[0];
        @event!.Id.Should().Be(Data.EventId);
        @event.Name.Should().Be(Data.EventName);
        @event.BunchId.Should().Be(Data.BunchId);
    }

    [Fact]
    [Order(TestSuite.Event, 3)]
    public async Task Suite08_Event03GetEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Event.Get(managerToken, Data.EventId);

        result.Success.Should().BeTrue();
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(Data.EventId);
        result.Model.Name.Should().Be(Data.EventName);
        result.Model.BunchId.Should().Be(Data.BunchId);
    }
}