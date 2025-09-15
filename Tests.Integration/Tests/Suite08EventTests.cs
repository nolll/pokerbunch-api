using System.Net;
using Api.Models.EventModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Event)]
public class Suite08EventTests
{
    [Test]
    [Order(1)]
    public async Task Test01AddEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new EventAddPostModel(TestData.EventName);
        var result = await TestClient.Event.Add(managerToken, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(TestData.EventId);
    }

    [Test]
    [Order(2)]
    public async Task Test02ListEvents()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Event.List(managerToken, TestData.BunchId);

        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(1);
        var @event = result.Model?[0];
        @event!.Id.Should().Be(TestData.EventId);
        @event.Name.Should().Be(TestData.EventName);
        @event.BunchId.Should().Be(TestData.BunchId);
    }

    [Test]
    [Order(3)]
    public async Task Test03GetEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Event.Get(managerToken, TestData.EventId);

        result.Success.Should().BeTrue();
        result.Model.Should().NotBeNull();
        result.Model.Id.Should().Be(TestData.EventId);
        result.Model.Name.Should().Be(TestData.EventName);
        result.Model.BunchId.Should().Be(TestData.BunchId);
    }
}