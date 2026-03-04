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

        var managerPlayer = result.Model.First(o => o.Id == Data.ManagerPlayerId);
        managerPlayer.Name.Should().Be(Data.ManagerDisplayName);
        managerPlayer.Id.Should().Be(Data.ManagerPlayerId);
        managerPlayer.Color.Should().Be("#9e9e9e");
        managerPlayer.UserId.Should().Be(Data.ManagerUserId);
        managerPlayer.UserName.Should().Be(Data.ManagerUserName);
        
        var playerPlayer = result.Model.First(o => o.Id == Data.PlayerPlayerId);
        playerPlayer.Name.Should().Be(Data.PlayerName);
        playerPlayer.Id.Should().Be(Data.PlayerPlayerId);
        playerPlayer.Color.Should().Be("#9e9e9e");
        playerPlayer.UserId.Should().BeNull();
        playerPlayer.UserName.Should().BeNull();

        var tempPlayer = result.Model.First(o => o.Id == Data.TempPlayerId);
        tempPlayer.Name.Should().Be(Data.TempPlayerName);
        tempPlayer.Id.Should().Be(Data.TempPlayerId);
        tempPlayer.Color.Should().Be("#9e9e9e");
        tempPlayer.UserId.Should().BeNull();
        tempPlayer.UserName.Should().BeNull();

        var userPlayer = result.Model.First(o => o.Id == Data.UserPlayerId);
        userPlayer.Name.Should().Be(Data.UserDisplayName);
        userPlayer.Id.Should().Be(Data.UserPlayerId);
        userPlayer.Color.Should().Be("#9e9e9e");
        userPlayer.UserId.Should().Be(Data.UserUserId);
        userPlayer.UserName.Should().Be(Data.UserUserName);
        
        List<string> orderedNames =
        [
            Data.ManagerDisplayName,
            Data.PlayerName,
            Data.TempPlayerName,
            Data.UserDisplayName
        ];
        orderedNames.Sort();
        
        result.Model.Select(o => o.Name).Should().BeEquivalentTo(orderedNames);
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
