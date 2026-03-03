using System.Net;
using Api.Models.PlayerModels;
using Xunit;

namespace Tests.Integration.Tests;

public partial class IntegrationTests
{
    private const string TempPlayerName = "Temp player";
    private const string TempPlayerId = "4";

    [Fact]
    [Order(TestSuite.Player, 1)]
    public async Task Suite09_01AddPlayer()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var parameters = new PlayerAddPostModel(TempPlayerName);
        var result = await fixture.ApiClient.Player.Add(managerToken, TestData.BunchId, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(TempPlayerName);
        result.Model.Id.Should().NotBeEmpty();
        result.Model.Slug.Should().Be(TestData.BunchId);
        result.Model.Color.Should().Be("#9e9e9e");
        result.Model.AvatarUrl.Should().Be("");
        result.Model.UserId.Should().BeNull();
        result.Model.UserName.Should().BeNull();
    }

    [Fact]
    [Order(TestSuite.Player, 2)]
    public async Task Suite09_02GetPlayer()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Player.Get(managerToken, TempPlayerId);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(TempPlayerName);
        result.Model.Id.Should().NotBeEmpty();
        result.Model.Slug.Should().Be(TestData.BunchId);
        result.Model.Color.Should().Be("#9e9e9e");
        result.Model.AvatarUrl.Should().Be("");
        result.Model.UserId.Should().BeNull();
        result.Model.UserName.Should().BeNull();
    }

    [Fact]
    [Order(TestSuite.Player, 3)]
    public async Task Suite09_03ListPlayers()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var result = await fixture.ApiClient.Player.List(managerToken, TestData.BunchId);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(4);

        var player1 = result.Model[0];
        player1.Name.Should().Be(TestData.ManagerDisplayName);
        player1.Id.Should().Be(TestData.ManagerPlayerId);
        player1.Color.Should().Be("#9e9e9e");
        player1.UserId.Should().Be(TestData.ManagerUserId);
        player1.UserName.Should().Be("manager");

        var player2 = result.Model[1];
        player2.Name.Should().Be(TestData.PlayerName);
        player2.Id.Should().Be(TestData.PlayerPlayerId);
        player2.Color.Should().Be("#9e9e9e");
        player2.UserId.Should().BeNull();
        player2.UserName.Should().BeNull();

        var player3 = result.Model[2];
        player3.Name.Should().Be(TempPlayerName);
        player3.Id.Should().Be(TempPlayerId);
        player3.Color.Should().Be("#9e9e9e");
        player3.UserId.Should().BeNull();
        player3.UserName.Should().BeNull();

        var player4 = result.Model[3];
        player4.Name.Should().Be(TestData.UserDisplayName);
        player4.Id.Should().Be(TestData.UserPlayerId);
        player4.Color.Should().Be("#9e9e9e");
        player4.UserId.Should().Be(TestData.UserUserId);
        player4.UserName.Should().Be(TestData.UserUserName);
    }

    [Fact]
    [Order(TestSuite.Player, 4)]
    public async Task Suite09_04DeletePlayer()
    {
        var managerToken = await fixture.LoginHelper.GetManagerToken();
        var deleteResult = await fixture.ApiClient.Player.Delete(managerToken, TempPlayerId);
        deleteResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResult = await fixture.ApiClient.Player.Get(managerToken, TempPlayerId);
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
