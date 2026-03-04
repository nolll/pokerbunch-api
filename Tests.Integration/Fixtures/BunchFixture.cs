using Api.Models.BunchModels;
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

    public async Task AddPlayer(UserFixture userToAdd)
    {
        var dbUser = db.PbUser.First(o => o.UserName == userToAdd.UserName);
        var dbBunch = db.PbBunch.First(o => o.Name == Id);
        var player = new PbPlayer
        {
            BunchId = dbBunch.BunchId,
            UserId = dbUser.UserId,
            RoleId = (int)Role.Player,
            Approved = true,
            Color = "#000000"
        };

        db.PbPlayer.Add(player);

        await db.SaveChangesAsync();
        await userToAdd.Refresh();
    }
    
    public async Task<LocationFixture> AddLocation(string? name = null)
    {
        var parameters = new LocationAddPostModel(name ?? dataFactory.String());
        var result = await apiClient.Location.Add(manager.Token, Id, parameters);
        return new LocationFixture(result.Model!);
    }
}