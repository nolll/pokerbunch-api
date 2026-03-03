using System.Net;
using Api.Models.PlayerModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    [Fact]
    [Order(TestSuite.Player, 1)]
    public async Task Suite09_01AddPlayer()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var parameters = new PlayerAddPostModel(Data.TempPlayerName);
        var result = await ApiClient.Player.Add(managerToken, Data.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(Data.TempPlayerName);
        result.Model.Id.Should().NotBeEmpty();
        result.Model.Slug.Should().Be(Data.BunchId);
        result.Model.Color.Should().Be("#9e9e9e");
        result.Model.AvatarUrl.Should().Be("");
        result.Model.UserId.Should().BeNull();
        result.Model.UserName.Should().BeNull();
    }

    [Fact]
    [Order(TestSuite.Player, 2)]
    public async Task Suite09_02GetPlayer()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Player.Get(managerToken, Data.TempPlayerId);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(Data.TempPlayerName);
        result.Model.Id.Should().NotBeEmpty();
        result.Model.Slug.Should().Be(Data.BunchId);
        result.Model.Color.Should().Be("#9e9e9e");
        result.Model.AvatarUrl.Should().Be("");
        result.Model.UserId.Should().BeNull();
        result.Model.UserName.Should().BeNull();
    }

    [Fact]
    [Order(TestSuite.Player, 3)]
    public async Task Suite09_03ListPlayers()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var result = await ApiClient.Player.List(managerToken, Data.BunchId);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(4);

        var player1 = result.Model[0];
        player1.Name.Should().Be(Data.ManagerDisplayName);
        player1.Id.Should().Be(Data.ManagerPlayerId);
        player1.Color.Should().Be("#9e9e9e");
        player1.UserId.Should().Be(Data.ManagerUserId);
        player1.UserName.Should().Be(Data.ManagerUserName);

        var player2 = result.Model[1];
        player2.Name.Should().Be(Data.PlayerName);
        player2.Id.Should().Be(Data.PlayerPlayerId);
        player2.Color.Should().Be("#9e9e9e");
        player2.UserId.Should().BeNull();
        player2.UserName.Should().BeNull();

        var player3 = result.Model[2];
        player3.Name.Should().Be(Data.TempPlayerName);
        player3.Id.Should().Be(Data.TempPlayerId);
        player3.Color.Should().Be("#9e9e9e");
        player3.UserId.Should().BeNull();
        player3.UserName.Should().BeNull();

        var player4 = result.Model[3];
        player4.Name.Should().Be(Data.UserDisplayName);
        player4.Id.Should().Be(Data.UserPlayerId);
        player4.Color.Should().Be("#9e9e9e");
        player4.UserId.Should().Be(Data.UserUserId);
        player4.UserName.Should().Be(Data.UserUserName);
    }

    [Fact]
    [Order(TestSuite.Player, 4)]
    public async Task Suite09_04DeletePlayer()
    {
        var managerToken = await LoginHelper.GetManagerToken();
        var deleteResult = await ApiClient.Player.Delete(managerToken, Data.TempPlayerId);
        deleteResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResult = await ApiClient.Player.Get(managerToken, Data.TempPlayerId);
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
