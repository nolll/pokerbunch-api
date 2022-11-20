using System.Net;
using System.Text.Json;
using Api.Models.EventModels;
using Api.Urls.ApiUrls;
using Microsoft.Extensions.Logging;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Event)]
public class EventTests
{
    [Test]
    [Order(1)]
    public async Task AddEvent()
    {
        var parameters = new EventAddPostModel(TestData.EventName);
        var url = new ApiEventAddUrl(TestData.BunchId).Relative;
        var response = await TestSetup.AuthorizedClient(TestData.ManagerToken).PostAsJsonAsync(url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<EventModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(TestData.EventIdInt));
    }

    [Test]
    [Order(2)]
    public async Task ListEvents()
    {
        var result = await TestClient.ListEvents(TestData.ManagerToken, TestData.BunchId);

        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Count, Is.EqualTo(1));
        var @event = result.Model[0];
        Assert.That(@event.Id, Is.EqualTo(TestData.EventIdInt));
        Assert.That(@event.Name, Is.EqualTo(TestData.EventName));
        Assert.That(@event.BunchId, Is.EqualTo(TestData.BunchId));
    }

    [Test]
    [Order(3)]
    public async Task GetEvent()
    {
        var result = await TestClient.GetEvent(TestData.ManagerToken, TestData.EventIdString);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model.Id, Is.EqualTo(TestData.EventIdInt));
        Assert.That(result.Model.Name, Is.EqualTo(TestData.EventName));
        Assert.That(result.Model.BunchId, Is.EqualTo(TestData.BunchId));
    }
}