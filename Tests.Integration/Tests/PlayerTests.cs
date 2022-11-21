using System.Net;
using System.Text.Json;
using Api.Models.PlayerModels;
using Api.Urls.ApiUrls;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Player)]
public class PlayerTests
{
    private const string TempPlayerName = "Temp player";
    private const int TempPlayerIdInt = 4;
    private const string TempPlayerIdString = "4";

    [Test]
    [Order(1)]
    public async Task AddPlayer()
    {
        var parameters = new PlayerAddPostModel(TempPlayerName);
        var url = new ApiPlayerAddUrl(TestData.BunchId).Relative;
        var response = await TestClient.Post(TestData.ManagerToken, url, parameters);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlayerModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(TempPlayerName));
        Assert.That(result.Id, Is.Not.Empty);
        Assert.That(result.Slug, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.UserId, Is.EqualTo(""));
        Assert.That(result.UserName, Is.EqualTo(""));
    }

    [Test]
    [Order(2)]
    public async Task GetPlayer()
    {
        var url = new ApiPlayerUrl(TempPlayerIdString).Relative;
        var response = await TestClient.Get(TestData.ManagerToken, url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlayerModel>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(TempPlayerName));
        Assert.That(result.Id, Is.Not.Empty);
        Assert.That(result.Slug, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.UserId, Is.EqualTo(""));
        Assert.That(result.UserName, Is.EqualTo(""));
    }

    [Test]
    [Order(2)]
    public async Task ListPlayers()
    {
        var url = new ApiPlayerListUrl(TestData.BunchId).Relative;
        var response = await TestClient.Get(TestData.ManagerToken, url);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<PlayerListItemModel>>(content);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(4));

        var player1 = result[0];
        Assert.That(player1.Name, Is.EqualTo(TestData.ManagerDisplayName));
        Assert.That(player1.Id, Is.EqualTo(TestData.ManagerPlayerIdInt));
        Assert.That(player1.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player1.UserId, Is.EqualTo(TestData.ManagerUserId));
        Assert.That(player1.UserName, Is.EqualTo("manager"));

        var player2 = result[1];
        Assert.That(player2.Name, Is.EqualTo(TestData.PlayerName));
        Assert.That(player2.Id, Is.EqualTo(TestData.PlayerPlayerIdInt));
        Assert.That(player2.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player2.UserId, Is.Null);
        Assert.That(player2.UserName, Is.Null);

        var player3 = result[2];
        Assert.That(player3.Name, Is.EqualTo(TempPlayerName));
        Assert.That(player3.Id, Is.EqualTo(TempPlayerIdInt));
        Assert.That(player3.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player3.UserId, Is.Null);
        Assert.That(player3.UserName, Is.Null);

        var player4 = result[3];
        Assert.That(player4.Name, Is.EqualTo(TestData.UserDisplayName));
        Assert.That(player4.Id, Is.EqualTo(TestData.UserPlayerIdInt));
        Assert.That(player4.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player4.UserId, Is.EqualTo(TestData.UserUserId));
        Assert.That(player4.UserName, Is.EqualTo(TestData.UserUserName));
    }

    [Test]
    [Order(4)]
    public async Task DeletePlayer()
    {
        var deleteUrl = new ApiPlayerDeleteUrl(TempPlayerIdString).Relative;
        var deleteResponse = await TestClient.Delete(TestData.ManagerToken, deleteUrl);
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getUrl = new ApiPlayerUrl(TempPlayerIdString).Relative;
        var getResponse = await TestClient.Get(TestData.ManagerToken, getUrl);
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
