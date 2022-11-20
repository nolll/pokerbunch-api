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
        var url = new ApiEventListUrl(TestData.BunchId).Relative;
        var content = await TestSetup.AuthorizedClient(TestData.ManagerToken).GetStringAsync(url);
        var result = JsonSerializer.Deserialize<List<EventModel>>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        var @event = result[0];
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

public static class TestClient
{
    public static async Task<TestClientResult<EventModel>> GetEvent(string token, string eventId)
    {
        return await Get<EventModel>(token, new ApiEventUrl(eventId));
    }

    private static async Task<TestClientResult<T>> Get<T>(string token, ApiUrl url) where T : class
    {
        var response = await TestSetup.AuthorizedClient(token).GetAsync(url.Relative);
        if (!response.IsSuccessStatusCode)
            return new TestClientResult<T>(false, response.StatusCode, null);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(content);

        return new TestClientResult<T>(true, response.StatusCode, result);
    }
}

public class TestClientResult<T> where T : class
{
    public bool Success { get; }
    public HttpStatusCode StatusCode { get; }
    public T Model { get; }

    public TestClientResult(bool success, HttpStatusCode statusCode, T model)
    {
        Success = success;
        StatusCode = statusCode;
        Model = model;
    }
}