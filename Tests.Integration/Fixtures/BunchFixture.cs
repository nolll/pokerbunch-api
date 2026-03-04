using Api.Models.BunchModels;
using Api.Models.EventModels;
using Api.Models.LocationModels;
using Core.Entities;
using Infrastructure.Sql.Models;
using Tests.Common;

namespace Tests.Integration.Fixtures;

public class BunchFixture(
    PokerBunchDbContext db,
    ApiClientForTest apiClient,
    TestDataFactory dataFactory,
    UserFixture manager,
    string id,
    AddBunchPostModel parameters)
{
    public string Id { get; } = id;
    public string Name { get; } = parameters.Name;
    public string CurrencySymbol { get; } = parameters.CurrencySymbol;
    public string CurrencyLayout { get; } = parameters.CurrencyLayout;
    public string Description { get; } = parameters.Description;
    public string Timezone { get; } = parameters.Timezone;

    public async Task<PlayerFixture> AddPlayer(UserFixture userToAdd)
    {
        var dbUser = db.PbUser.First(o => o.UserName == userToAdd.UserName);
        var player = await AddPlayer(null, dbUser.UserId);
        await userToAdd.Refresh();

        return player;
    }
    
    public Task<PlayerFixture> AddPlayer(string? playerName = null) => 
        AddPlayer(playerName ?? dataFactory.String(), null);

    private async Task<PlayerFixture> AddPlayer(string? playerName, int? userId)
    {
        var dbBunch = db.PbBunch.First(o => o.Name == Id);
        var player = new PbPlayer
        {
            PlayerName = playerName,
            BunchId = dbBunch.BunchId,
            UserId = userId,
            RoleId = (int)Role.Player,
            Approved = true,
            Color = "#9e9e9e"
        };

        db.PbPlayer.Add(player);

        await db.SaveChangesAsync();

        return new PlayerFixture(player.PlayerId.ToString(), playerName, userId.ToString(), Id);
    }
    
    public async Task<LocationFixture> AddLocation(string? name = null)
    {
        var parameters = new LocationAddPostModel(name ?? dataFactory.String());
        var result = await apiClient.Location.Add(manager.Token, Id, parameters);
        return new LocationFixture(result.Model!);
    }
    
    public async Task<EventFixture> AddEvent(string? name = null)
    {
        var parameters = new EventAddPostModel(name ?? dataFactory.String());
        var result = await apiClient.Event.Add(manager.Token, Id, parameters);
        return new EventFixture(result.Model!);
    }
}