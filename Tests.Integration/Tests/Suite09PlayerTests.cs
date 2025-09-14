using System.Net;
using Api.Models.PlayerModels;

namespace Tests.Integration.Tests;

[TestFixture]
[NonParallelizable]
[Order(TestOrder.Player)]
public class Suite09PlayerTests
{
    private const string TempPlayerName = "Temp player";
    private const string TempPlayerId = "4";

    [Test]
    [Order(1)]
    public async Task Test01AddPlayer()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new PlayerAddPostModel(TempPlayerName);
        var result = await TestClient.Player.Add(managerToken, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model?.Name.Should().Be(TempPlayerName);
        result.Model?.Id.Should().NotBeEmpty();
        result.Model?.Slug.Should().Be(TestData.BunchId);
        result.Model?.Color.Should().Be("#9e9e9e");
        result.Model?.AvatarUrl.Should().Be("");
        result.Model?.UserId.Should().BeNull();
        result.Model?.UserName.Should().BeNull();
    }

    [Test]
    [Order(2)]
    public async Task Test02GetPlayer()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Player.Get(managerToken, TempPlayerId);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model?.Name.Should().Be(TempPlayerName);
        result.Model?.Id.Should().NotBeEmpty();
        result.Model?.Slug.Should().Be(TestData.BunchId);
        result.Model?.Color.Should().Be("#9e9e9e");
        result.Model?.AvatarUrl.Should().Be("");
        result.Model?.UserId.Should().BeNull();
        result.Model?.UserName.Should().BeNull();
    }

    [Test]
    [Order(3)]
    public async Task Test03ListPlayers()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await TestClient.Player.List(managerToken, TestData.BunchId);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model?.Count.Should().Be(4);

        var player1 = result.Model?[0];
        player1?.Name.Should().Be(TestData.ManagerDisplayName);
        player1?.Id.Should().Be(TestData.ManagerPlayerId);
        player1?.Color.Should().Be("#9e9e9e");
        player1?.UserId.Should().Be(TestData.ManagerUserId);
        player1?.UserName.Should().Be("manager");

        var player2 = result.Model?[1];
        player2?.Name.Should().Be(TestData.PlayerName);
        player2?.Id.Should().Be(TestData.PlayerPlayerId);
        player2?.Color.Should().Be("#9e9e9e");
        player2?.UserId.Should().BeNull();
        player2?.UserName.Should().BeNull();

        var player3 = result.Model?[2];
        player3?.Name.Should().Be(TempPlayerName);
        player3?.Id.Should().Be(TempPlayerId);
        player3?.Color.Should().Be("#9e9e9e");
        player3?.UserId.Should().BeNull();
        player3?.UserName.Should().BeNull();

        var player4 = result.Model?[3];
        player4?.Name.Should().Be(TestData.UserDisplayName);
        player4?.Id.Should().Be(TestData.UserPlayerId);
        player4?.Color.Should().Be("#9e9e9e");
        player4?.UserId.Should().Be(TestData.UserUserId);
        player4?.UserName.Should().Be(TestData.UserUserName);
    }

    [Test]
    [Order(4)]
    public async Task Test04DeletePlayer()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var deleteResult = await TestClient.Player.Delete(managerToken, TempPlayerId);
        deleteResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResult = await TestClient.Player.Get(managerToken, TempPlayerId);
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
