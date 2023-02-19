using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql.SqlDb;

public class BunchDb
{
    private readonly IDb _db;

    private static Query TableQuery => new(Schema.Bunch);

    private static Query GetQuery => TableQuery
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

    private static Query FindQuery => TableQuery.Select($"{Schema.Bunch.Id.FullName}");

    public BunchDb(IDb db)
    {
        _db = db;
    }

    public async Task<Bunch> Get(string id)
    {
        var query = GetQuery.Where(Schema.Bunch.Id, int.Parse(id));
        var dto = await _db.QueryFactory.FromQuery(query).FirstAsync<BunchDto>();

        if (dto is null)
            throw new PokerBunchException($"Bunch with id {id} was not found");

        return dto.ToBunch();
    }

    public async Task<IList<Bunch>> Get(IList<string> ids)
    {
        var param = new ListParam("@ids", ids.Select(int.Parse));

        var query = GetQuery.WhereIn(Schema.Bunch.Id, ids.Select(int.Parse));
        var dtos = await _db.QueryFactory.FromQuery(query).GetAsync<BunchDto>();
        return dtos.Select(BunchMapper.ToBunch).ToList();
    }

    public async Task<IList<string>> Search()
    {
        var result = await _db.QueryFactory.FromQuery(FindQuery).GetAsync<int>();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var query = FindQuery.Where(Schema.Bunch.Name, slug);
        var id = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<int?>();

        return id is not null
            ? new List<string> { id.Value.ToString() }
            : new List<string>();
    }

    public async Task<IList<string>> SearchByUser(string userId)
    {
        var query = FindQuery.Join(Schema.Player, $"{Schema.Player.BunchId.FullName}", $"{Schema.Bunch.Id.FullName}")
            .Where($"{Schema.Player.UserId}", int.Parse(userId))
            .OrderBy($"{Schema.Bunch.Name}");

        var result = await _db.QueryFactory.FromQuery(query).GetAsync<int>();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Bunch bunch)
    {
        var @params = new
        {
            slug = bunch.Slug,
            displayName = bunch.DisplayName,
            description = bunch.Description,
            currencySymbol = bunch.Currency.Symbol,
            currencyLayout = bunch.Currency.Layout,
            timeZone = bunch.Timezone.Id,
            cashgamesEnabled = bunch.CashgamesEnabled,
            tournamentsEnabled = bunch.TournamentsEnabled,
            videosEnabled = bunch.VideosEnabled,
            houseRules = bunch.HouseRules
        };

        return (await _db.Insert(BunchSql.AddQuery, @params)).ToString();
    }

    public async Task Update(Bunch bunch)
    {
        var @params = new
        {
            slug = bunch.Slug,
            displayName = bunch.DisplayName,
            description = bunch.Description,
            houseRules = bunch.HouseRules,
            currencySymbol = bunch.Currency.Symbol,
            currencyLayout = bunch.Currency.Layout,
            timeZone = bunch.Timezone.Id,
            defaultBuyin = bunch.DefaultBuyin,
            cashgamesEnabled = bunch.CashgamesEnabled,
            tournamentsEnabled = bunch.TournamentsEnabled,
            videosEnabled = bunch.VideosEnabled,
            id = int.Parse(bunch.Id)
        };

        await _db.Execute(BunchSql.UpdateQuery, @params);
    }

    public async Task<bool> DeleteBunch(string id)
    {
        var query = TableQuery.Where(Schema.Bunch.Id, int.Parse(id));
        var rowCount = await _db.QueryFactory.FromQuery(query).DeleteAsync();

        return rowCount > 0;
    }
}