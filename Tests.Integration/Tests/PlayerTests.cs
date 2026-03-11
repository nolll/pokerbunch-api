using System.Net;
using Api.Models.PlayerModels;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

public class PlayerTests(TestFixture fixture) : IntegrationTests(fixture)
{
    [Fact]
    public async Task AddPlayer()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        
        var parameters = new PlayerAddPostModel(Data.String());
        var result = await ApiClient.Player.Add(manager.Token, bunch.Id, parameters);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(parameters.Name);
        result.Model.Id.Should().NotBeEmpty();
        result.Model.Slug.Should().Be(bunch.Id);
        result.Model.Color.Should().Be("#9e9e9e");
        result.Model.AvatarUrl.Should().Be("");
        result.Model.UserId.Should().BeNull();
        result.Model.UserName.Should().BeNull();
    }

    [Fact]
    public async Task GetPlayer()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        
        var result = await ApiClient.Player.Get(manager.Token, player.Id);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Name.Should().Be(player.Name);
        result.Model.Id.Should().NotBeEmpty();
        result.Model.Slug.Should().Be(bunch.Id);
        result.Model.Color.Should().Be("#9e9e9e");
        result.Model.AvatarUrl.Should().Be("");
        result.Model.UserId.Should().BeNull();
        result.Model.UserName.Should().BeNull();
    }

    [Fact]
    public async Task ListPlayers()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        
        var result = await ApiClient.Player.List(manager.Token, bunch.Id);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Model.Should().NotBeNull();
        result.Model.Count.Should().Be(2);

        var managerPlayer = result.Model.First(o => o.Id != player.Id);
        managerPlayer.Name.Should().Be(manager.DisplayName);
        managerPlayer.UserName.Should().Be(manager.UserName);
        
        var playerPlayer = result.Model.First(o => o.Id == player.Id);
        playerPlayer.Name.Should().Be(player.Name);
        playerPlayer.UserName.Should().BeNull();
        
        List<string> orderedNames =
        [
            manager.DisplayName,
            player.Name!
        ];
        orderedNames.Sort();
        
        result.Model.Select(o => o.Name).Should().BeEquivalentTo(orderedNames);
    }

    [Fact]
    public async Task DeletePlayer()
    {
        var manager = await Fixture.CreateUser();
        var bunch = await Fixture.CreateBunch(manager);
        var player = await bunch.AddPlayer();
        
        var deleteResult = await ApiClient.Player.Delete(manager.Token, player.Id);
        deleteResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResult = await ApiClient.Player.Get(manager.Token, player.Id);
        getResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
