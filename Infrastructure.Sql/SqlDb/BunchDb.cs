using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Infrastructure.Sql.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class BunchDb(PokerBunchDbContext db)
{
    public async Task<Bunch> Get(string id)
    {
        var query = db.PbBunch
            .Where(o => o.BunchId == int.Parse(id))
            .ToBunchDto();

        var dto = await query.FirstAsync();

        return dto is not null 
            ? dto.ToBunch() 
            : throw new PokerBunchException($"Bunch with id {id} was not found");
    }
    
    public async Task<IList<Bunch>> Get(IList<string> ids)
    {
        var query2 = db.PbBunch
            .Where(o => ids.Select(int.Parse).Contains(o.BunchId))
            .ToBunchDto();

        var dtos2 = await query2.ToListAsync();
        return dtos2.Select(BunchMapper.ToBunch).ToList();
    }

    public async Task<IList<string>> Search()
    {
        var query = db.PbBunch.Select(o => o.BunchId);
        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> Search(string slug)
    {
        var query2 = db.PbBunch
            .Where(o => o.Name == slug)
            .Select(o => o.BunchId);

        var ids = await query2.ToListAsync();
        return ids.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> SearchByUser(string userId)
    {
        var query = db.PbPlayer
            .Where(o => o.UserId == int.Parse(userId))
            .Select(o => o.Bunch)
            .OrderBy(o => o.Name)
            .Select(o => o.BunchId);

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Bunch bunch)
    {
        var dto = new PbBunch
        {
            Name = bunch.Slug,
            DisplayName = bunch.DisplayName,
            Description = bunch.Description,
            Currency = bunch.Currency.Symbol,
            CurrencyLayout = bunch.Currency.Layout,
            Timezone = bunch.Timezone.Id,
            DefaultBuyin = bunch.DefaultBuyin,
            CashgamesEnabled = bunch.CashgamesEnabled,
            TournamentsEnabled = bunch.TournamentsEnabled,
            VideosEnabled = bunch.VideosEnabled,
            HouseRules = bunch.HouseRules
        };

        db.PbBunch.Add(dto);
        await db.SaveChangesAsync();
        return dto.BunchId.ToString();
    }

    public async Task Update(Bunch bunch)
    {
        var dto = db.PbBunch
            .First(o => o.BunchId == int.Parse(bunch.Id));

        dto.Name = bunch.Slug;
        dto.DisplayName = bunch.DisplayName;
        dto.Description = bunch.Description;
        dto.Currency = bunch.Currency.Symbol;
        dto.CurrencyLayout = bunch.Currency.Layout;
        dto.Timezone = bunch.Timezone.Id;
        dto.DefaultBuyin = bunch.DefaultBuyin;
        dto.CashgamesEnabled = bunch.CashgamesEnabled;
        dto.TournamentsEnabled = bunch.TournamentsEnabled;
        dto.VideosEnabled = bunch.VideosEnabled;
        dto.HouseRules = bunch.HouseRules;

        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteBunch(string id)
    {
        var dto = new PbBunch { BunchId = int.Parse(id) };
        db.PbBunch.Remove(dto);
        var rowCount = await db.SaveChangesAsync();
        return rowCount > 0;
    }
}

internal static class DtoMapper
{
    internal static IQueryable<BunchDto> ToBunchDto(this IQueryable<PbBunch> input) => input.Select(o => new BunchDto
    {
        Bunch_Id = o.BunchId,
        Name = o.Name,
        Display_Name = o.DisplayName,
        Description = o.Description,
        House_Rules = o.HouseRules,
        Timezone = o.Timezone,
        Default_Buyin = o.DefaultBuyin,
        Currency_Layout = o.CurrencyLayout,
        Currency = o.Currency,
        Cashgames_Enabled = o.CashgamesEnabled,
        Tournaments_Enabled = o.TournamentsEnabled,
        Videos_Enabled = o.VideosEnabled
    });
}