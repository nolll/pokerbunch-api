using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class LocationDb(PokerBunchDbContext db) : BaseDb(db)
{
    private readonly PokerBunchDbContext _db = db;

    public async Task<Location> Get(string id)
    {
        var query = _db.PbLocation
            .Where(o => o.LocationId == int.Parse(id))
            .Select(o => new LocationDto
            {
                Location_Id = o.LocationId,
                Name = o.Name,
                Bunch_Slug = o.Bunch.Name
            });

        var dto = await query.FirstOrDefaultAsync();
        var location = dto?.ToLocation();

        return location ?? throw new PokerBunchException($"Location with id {id} was not found");
    }
        
    public async Task<IList<Location>> Get(IList<string> ids)
    {
        if (!ids.Any())
            return new List<Location>();
        
        var q = _db.PbLocation
            .Where(o => ids.Select(int.Parse).Contains(o.LocationId))
            .Select(o => new LocationDto
            {
                Location_Id = o.LocationId,
                Name = o.Name,
                Bunch_Slug = o.Bunch.Name
            });

        var dtos = await q.ToListAsync();
        return dtos.Select(LocationMapper.ToLocation).ToList();
    }

    public async Task<IList<string>> Find(string slug)
    {
        var q = _db.PbLocation
            .Where(o => o.Bunch.Name == slug)
            .Select(o => o.LocationId);

        var ids = await q.ToListAsync();
        return ids.Select(o => o.ToString()).ToList();
    }
        
    public async Task<string> Add(Location location)
    {
        var bunchId = await GetBunchId(location.BunchSlug);

        var dto = new PbLocation()
        {
            BunchId = bunchId,
            Name = location.Name
        };

        _db.PbLocation.Add(dto);
        
        return dto.LocationId.ToString();
    }
}