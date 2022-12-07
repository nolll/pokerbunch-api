using System.Globalization;
using System.Linq;
using Core.Entities;
using Infrastructure.Sql.Dtos;
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
        return dtos.Select(CreateBunch).ToList();
    }

    public async Task<Bunch> Get(string id)
    {
        var @params = new
        {
            id = int.Parse(id)
        };

        var dto = await _db.Single<BunchDto>(BunchSql.GetByIdQuery, @params);
        return dto != null ? CreateBunch(dto) : null;
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
        var dto = BunchDto.Create(bunch);

        var @params = new
        {
            slug = dto.Name,
            displayName = dto.Display_Name,
            description = dto.Description,
            currencySymbol = dto.Currency,
            currencyLayout = dto.Currency_Layout,
            timeZone = dto.Timezone,
            cashgamesEnabled = dto.Cashgames_Enabled,
            tournamentsEnabled = dto.Tournaments_Enabled,
            videosEnabled = dto.Videos_Enabled,
            @houseRules = dto.House_Rules
        };

        return (await _db.Insert(BunchSql.AddQuery, @params)).ToString();
    }

    public async Task Update(Bunch bunch)
    {
        var dto = BunchDto.Create(bunch);

        var @params = new
        {
            slug = dto.Name,
            displayName = dto.Display_Name,
            description = dto.Description,
            houseRules = dto.House_Rules,
            currencySymbol = dto.Currency,
            currencyLayout = dto.Currency_Layout,
            timeZone = dto.Timezone,
            defaultBuyin = dto.Default_Buyin,
            cashgamesEnabled = dto.Cashgames_Enabled,
            tournamentsEnabled = dto.Tournaments_Enabled,
            videosEnabled = dto.Videos_Enabled,
            id = int.Parse(dto.Bunch_Id)
        };

        await _db.Execute(BunchSql.UpdateQuery, @params);
    }

    private static Bunch CreateBunch(BunchDto bunchDto)
    {
        var culture = CultureInfo.CreateSpecificCulture("sv-SE");
        var currency = new Currency(bunchDto.Currency, bunchDto.Currency_Layout, culture);

        return new Bunch(
            bunchDto.Bunch_Id,
            bunchDto.Name,
            bunchDto.Display_Name,
            bunchDto.Description,
            bunchDto.House_Rules,
            TimeZoneInfo.FindSystemTimeZoneById(bunchDto.Timezone),
            bunchDto.Default_Buyin,
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