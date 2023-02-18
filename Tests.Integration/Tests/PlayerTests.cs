using System.Net;
using Api.Models.PlayerModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Player)]
public class PlayerTests
{
    private const string TempPlayerName = "Temp player";
    private const string TempPlayerId = "4";

    [Test]
    [Order(1)]
    public async Task AddPlayer()
    {
        var parameters = new PlayerAddPostModel(TempPlayerName);
        var result = await TestClient.Player.Add(TestData.ManagerToken, TestData.BunchId, parameters);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Name, Is.EqualTo(TempPlayerName));
        Assert.That(result.Model?.Id, Is.Not.Empty);
        Assert.That(result.Model?.Slug, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Model?.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.Model?.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.Model?.UserId, Is.Null);
        Assert.That(result.Model?.UserName, Is.Null);
    }

    [Test]
    [Order(2)]
    public async Task GetPlayer()
    {
        var result = await TestClient.Player.Get(TestData.ManagerToken, TempPlayerId);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Name, Is.EqualTo(TempPlayerName));
        Assert.That(result.Model?.Id, Is.Not.Empty);
        Assert.That(result.Model?.Slug, Is.EqualTo(TestData.BunchId));
        Assert.That(result.Model?.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(result.Model?.AvatarUrl, Is.EqualTo(""));
        Assert.That(result.Model?.UserId, Is.Null);
        Assert.That(result.Model?.UserName, Is.Null);
    }

    [Test]
    [Order(2)]
    public async Task ListPlayers()
    {
        var result = await TestClient.Player.List(TestData.ManagerToken, TestData.BunchId);
        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(result.Model, Is.Not.Null);
        Assert.That(result.Model?.Count, Is.EqualTo(4));

        var player1 = result.Model?[0];
        Assert.That(player1?.Name, Is.EqualTo(TestData.ManagerDisplayName));
        Assert.That(player1?.Id, Is.EqualTo(TestData.ManagerPlayerId));
        Assert.That(player1?.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player1?.UserId, Is.EqualTo(TestData.ManagerUserId));
        Assert.That(player1?.UserName, Is.EqualTo("manager"));

        var player2 = result.Model?[1];
        Assert.That(player2?.Name, Is.EqualTo(TestData.PlayerName));
        Assert.That(player2?.Id, Is.EqualTo(TestData.PlayerPlayerId));
        Assert.That(player2?.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player2?.UserId, Is.Null);
        Assert.That(player2?.UserName, Is.Null);

        var player3 = result.Model?[2];
        Assert.That(player3?.Name, Is.EqualTo(TempPlayerName));
        Assert.That(player3?.Id, Is.EqualTo(TempPlayerId));
        Assert.That(player3?.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player3?.UserId, Is.Null);
        Assert.That(player3?.UserName, Is.Null);

        var player4 = result.Model?[3];
        Assert.That(player4?.Name, Is.EqualTo(TestData.UserDisplayName));
        Assert.That(player4?.Id, Is.EqualTo(TestData.UserPlayerId));
        Assert.That(player4?.Color, Is.EqualTo("#9e9e9e"));
        Assert.That(player4?.UserId, Is.EqualTo(TestData.UserUserId));
        Assert.That(player4?.UserName, Is.EqualTo(TestData.UserUserName));
    }

    [Test]
    [Order(4)]
    public async Task DeletePlayer()
    {
        var deleteResult = await TestClient.Player.Delete(TestData.ManagerToken, TempPlayerId);
        Assert.That(deleteResult.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var getResult = await TestClient.Player.Get(TestData.ManagerToken, TempPlayerId);
        Assert.That(getResult.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
