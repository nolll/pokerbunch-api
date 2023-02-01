using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;

namespace Infrastructure.Sql.SqlDb;

public class BunchDb
{
    private readonly IDb _db;

    public BunchDb(IDb db)
    {
        _db = db;
    }

    public async Task<IList<Bunch>> Get(IList<string> ids)
    {
        var param = new ListParam("@ids", ids.Select(int.Parse));

        var dtos = await _db.List<BunchDto>(BunchSql.GetByIdsQuery, param);
        return dtos.Select(BunchMapper.ToBunch).ToList();
    }

    public async Task<Bunch> Get(string id)
    {
        var @params = new
        {
            id = int.Parse(id)
        };

        var dto = await _db.Single<BunchDto>(BunchSql.GetByIdQuery, @params);
        return dto.ToBunch();
    }

    public async Task<IList<string>> Search()
    {
        return (await _db.List<int>(BunchSql.SearchQuery)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var @params = new
        {
            slug
        };

        var id = (await _db.Single<int?>(BunchSql.SearchBySlugQuery, @params))?.ToString();
        return id != null
            ? new List<string> { id }
            : new List<string>();
    }

    public async Task<IList<string>> SearchByUser(string userId)
    {
        var @params = new
        {
            userId = int.Parse(userId)
        };

        return (await _db.List<int>(BunchSql.SearchByUserQuery, @params)).Select(o => o.ToString()).ToList();
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
        var @params = new
        {
            id = int.Parse(id)
        };

        var rowCount = await _db.Execute(BunchSql.DeleteSql, @params);
        return rowCount > 0;
    }
}