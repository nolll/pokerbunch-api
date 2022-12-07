using System.Globalization;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Classes;
using Infrastructure.Sql.Sql;
using Infrastructure.Sql.SqlParameters;

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

        var rawBunches = await _db.List<RawBunch>(BunchSql.GetByIdsQuery, param);
        return rawBunches.Select(CreateBunch).ToList();
    }

    public async Task<Bunch> Get(string id)
    {
        var @params = new
        {
            id = int.Parse(id)
        };

        var rawBunch = await _db.Single<RawBunch>(BunchSql.GetByIdQuery, @params);
        return rawBunch != null ? CreateBunch(rawBunch) : null;
    }

    public async Task<IList<string>> Search()
    {
        return (await _db.List<int>(BunchSql.SearchQuery)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var @params = new
        {
            slug = slug
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
        var rawBunch = RawBunch.Create(bunch);

        var @params = new
        {
            slug = rawBunch.Name,
            displayName = rawBunch.Display_Name,
            description = rawBunch.Description,
            currencySymbol = rawBunch.Currency,
            currencyLayout = rawBunch.Currency_Layout,
            timeZone = rawBunch.Timezone,
            cashgamesEnabled = rawBunch.Cashgames_Enabled,
            tournamentsEnabled = rawBunch.Tournaments_Enabled,
            videosEnabled = rawBunch.Videos_Enabled,
            @houseRules = rawBunch.House_Rules
        };

        return (await _db.Insert(BunchSql.AddQuery, @params)).ToString();
    }

    public async Task Update(Bunch bunch)
    {
        var rawBunch = RawBunch.Create(bunch);

        var @params = new
        {
            slug = rawBunch.Name,
            displayName = rawBunch.Display_Name,
            description = rawBunch.Description,
            houseRules = rawBunch.House_Rules,
            currencySymbol = rawBunch.Currency,
            currencyLayout = rawBunch.Currency_Layout,
            timeZone = rawBunch.Timezone,
            defaultBuyin = rawBunch.Default_Buyin,
            cashgamesEnabled = rawBunch.Cashgames_Enabled,
            tournamentsEnabled = rawBunch.Tournaments_Enabled,
            videosEnabled = rawBunch.Videos_Enabled,
            id = int.Parse(rawBunch.Bunch_Id)
        };

        await _db.Execute(BunchSql.UpdateQuery, @params);
    }

    private static Bunch CreateBunch(RawBunch rawBunch)
    {
        var culture = CultureInfo.CreateSpecificCulture("sv-SE");
        var currency = new Currency(rawBunch.Currency, rawBunch.Currency_Layout, culture);

        return new Bunch(
            rawBunch.Bunch_Id,
            rawBunch.Name,
            rawBunch.Display_Name,
            rawBunch.Description,
            rawBunch.House_Rules,
            TimeZoneInfo.FindSystemTimeZoneById(rawBunch.Timezone),
            rawBunch.Default_Buyin,
            currency);
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