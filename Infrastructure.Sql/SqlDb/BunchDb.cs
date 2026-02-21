using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class BunchDb(IDb db)
{
    private static Query BunchQuery => new(Schema.Bunch);

    private static Query GetQuery => BunchQuery
        .Select(
            Schema.Bunch.Id,
            Schema.Bunch.Name,
            Schema.Bunch.DisplayName,
            Schema.Bunch.Description,
            Schema.Bunch.Currency,
            Schema.Bunch.CurrencyLayout,
            Schema.Bunch.Timezone,
            Schema.Bunch.DefaultBuyin,
            Schema.Bunch.CashgamesEnabled,
            Schema.Bunch.TournamentsEnabled,
            Schema.Bunch.VideosEnabled,
            Schema.Bunch.HouseRules);

    private static Query FindQuery => BunchQuery.Select(Schema.Bunch.Id);

    public async Task<Bunch> Get(string id)
    {
        var query = GetQuery.Where(Schema.Bunch.Id, int.Parse(id));
        var dto = await db.FirstAsync<BunchDto>(query);

        if (dto is null)
            throw new PokerBunchException($"Bunch with id {id} was not found");

        return dto.ToBunch();
    }
    
    public async Task<IList<Bunch>> Get(IList<string> ids)
    {
        var query = GetQuery.WhereIn(Schema.Bunch.Id, ids.Select(int.Parse));
        var dtos = await db.GetAsync<BunchDto>(query);
        return dtos.Select(BunchMapper.ToBunch).ToList();
    }

    public async Task<IList<string>> Search()
    {
        var result = await db.GetAsync<int>(FindQuery);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var query = FindQuery.Where(Schema.Bunch.Name, slug);
        var id = await db.FirstOrDefaultAsync<int?>(query);

        return id is not null
            ? [id.Value.ToString()]
            : [];
    }

    public async Task<IList<string>> SearchByUser(string userId)
    {
        var query = FindQuery.Join(Schema.Player, $"{Schema.Player.BunchId}", $"{Schema.Bunch.Id}")
            .Where($"{Schema.Player.UserId}", int.Parse(userId))
            .OrderBy($"{Schema.Bunch.Name}");

        var result = await db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Bunch bunch)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.Bunch.Name, bunch.Slug },
            { Schema.Bunch.DisplayName, bunch.DisplayName },
            { Schema.Bunch.Description, bunch.Description },
            { Schema.Bunch.Currency, bunch.Currency.Symbol },
            { Schema.Bunch.CurrencyLayout, bunch.Currency.Layout },
            { Schema.Bunch.Timezone, bunch.Timezone.Id },
            { Schema.Bunch.DefaultBuyin, 0 },
            { Schema.Bunch.CashgamesEnabled, bunch.CashgamesEnabled },
            { Schema.Bunch.TournamentsEnabled, bunch.TournamentsEnabled },
            { Schema.Bunch.VideosEnabled, bunch.VideosEnabled },
            { Schema.Bunch.HouseRules, bunch.HouseRules }
        };

        var result = await db.InsertGetIdAsync(BunchQuery, parameters);
        return result.ToString();
    }

    public async Task Update(Bunch bunch)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.Bunch.Name, bunch.Slug },
            { Schema.Bunch.DisplayName, bunch.DisplayName },
            { Schema.Bunch.Description, bunch.Description },
            { Schema.Bunch.Currency, bunch.Currency.Symbol },
            { Schema.Bunch.CurrencyLayout, bunch.Currency.Layout },
            { Schema.Bunch.Timezone, bunch.Timezone.Id },
            { Schema.Bunch.DefaultBuyin, bunch.DefaultBuyin },
            { Schema.Bunch.CashgamesEnabled, bunch.CashgamesEnabled },
            { Schema.Bunch.TournamentsEnabled, bunch.TournamentsEnabled },
            { Schema.Bunch.VideosEnabled, bunch.VideosEnabled },
            { Schema.Bunch.HouseRules, bunch.HouseRules }
        };

        var query = BunchQuery.Where(Schema.Bunch.Id, int.Parse(bunch.Id));
        await db.UpdateAsync(query, parameters);
    }

    public async Task<bool> DeleteBunch(string id)
    {
        var query = BunchQuery.Where(Schema.Bunch.Id, int.Parse(id));
        var rowCount = await db.DeleteAsync(query);

        return rowCount > 0;
    }
}