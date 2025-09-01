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
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Id, Is.EqualTo(TestData.EventId));
    }

    [Test]
    [Order(2)]
    public async Task Test02ListEvents()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Event.List(managerToken, TestData.BunchId);

        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Count, Is.EqualTo(1));
        var @event = result.Model?[0];
        Assert.That(@event?.Id, Is.EqualTo(TestData.EventId));
        Assert.That(@event?.Name, Is.EqualTo(TestData.EventName));
        Assert.That(@event?.BunchId, Is.EqualTo(TestData.BunchId));
    }

    [Test]
    [Order(3)]
    public async Task Test03GetEvent()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Event.Get(managerToken, TestData.EventId);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Id, Is.EqualTo(TestData.EventId));
        Assert.That(result.Model?.Name, Is.EqualTo(TestData.EventName));
        Assert.That(result.Model?.BunchId, Is.EqualTo(TestData.BunchId));
    }
}