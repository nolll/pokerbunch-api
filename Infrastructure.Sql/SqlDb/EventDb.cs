using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class EventDb(PokerBunchDbContext db) : BaseDb(db)
{
    private readonly PokerBunchDbContext _db = db;

    public async Task<Event> Get(string id)
    {
        var query = _db.PbEvent
            .Where(o => o.EventId == int.Parse(id))
            .Select(o => new EventDayDto
            {
                EventId = o.EventId,
                BunchSlug = o.Bunch.Name,
                Name = o.Name,
                LocationId = o.Cashgame.Any() ? o.Cashgame.First().LocationId : null,
                Timestamp = o.Cashgame.Any() ? o.Cashgame.First().Timestamp : DateTime.MinValue
            });
        
        var dtos = await query.ToListAsync();
        var events = dtos.ToEvents();
        
        return events.Count > 0 
            ? events.First() 
            : throw new PokerBunchException($"Event with id {id} was not found");
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        var query = _db.PbEvent
            .Where(o => ids.Select(int.Parse).Contains(o.EventId))
            .Select(o => new EventDayDto
            {
                EventId = o.EventId,
                BunchSlug = o.Bunch.Name,
                Name = o.Name,
                LocationId = o.Cashgame.Any() ? o.Cashgame.First().LocationId : null,
                Timestamp = o.Cashgame.Any() ? o.Cashgame.First().Timestamp : DateTime.MinValue
            });

        var dtos = await query.ToListAsync();
        return dtos.ToEvents();
    }
    
    public async Task<IList<string>> FindBySlug(string slug)
    {
        var query = _db.PbEvent
            .Where(o => o.Bunch.Name == slug)
            .Select(o => o.EventId);

        var ids = await query.ToListAsync();
        return ids.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        var cashgame = _db.PbCashgame.First(o => o.CashgameId == int.Parse(cashgameId));
        var query = _db.PbEvent
            .Where(o => o.Cashgame.Contains(cashgame))
            .Select(o => o.EventId);
        
        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Event e)
    {
        var bunchId = await GetBunchId(e.BunchSlug);
        
        var dto = new PbEvent
        {
            BunchId = bunchId,
            Name = e.Name
        };

        _db.PbEvent.Add(dto);
        await _db.SaveChangesAsync();
        return dto.EventId.ToString();
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        var e = _db.PbEvent.First(o => o.EventId == int.Parse(eventId));
        var c = _db.PbCashgame.First(o => o.CashgameId == int.Parse(cashgameId));
        e.Cashgame.Add(c);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        var e = _db.PbEvent
            .Include(o => o.Cashgame)
            .First(o => o.EventId == int.Parse(eventId));
        
        foreach (var cashgame in e.Cashgame.Where(o => o.CashgameId == int.Parse(cashgameId)).ToList())
        {
            e.Cashgame.Remove(cashgame);
        }

        var c = new PbCashgame { CashgameId = int.Parse(cashgameId) };
        e.Cashgame.Remove(c);
        await _db.SaveChangesAsync();
    }
}