using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Infrastructure.Sql.Sql;
using Microsoft.EntityFrameworkCore;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class EventDb(PokerBunchDbContext db, IDb dbold)
{
    private static Query EventCashgameQuery => new(Schema.EventCashgame);

    public async Task<Event> Get(string id)
    {
        var query = db.PbEvent
            .Where(o => o.EventId == int.Parse(id))
            .Select(o => new EventDayDto
            {
                Event_Id = o.EventId,
                Bunch_Slug = o.Bunch.Name,
                Name = o.Name,
                Location_Id = o.Cashgame.Any() ? o.Cashgame.First().LocationId : null,
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
        var query = db.PbEvent
            .Where(o => ids.Select(int.Parse).Contains(o.EventId))
            .Select(o => new EventDayDto
            {
                Event_Id = o.EventId,
                Bunch_Slug = o.Bunch.Name,
                Name = o.Name,
                Location_Id = o.Cashgame.Any() ? o.Cashgame.First().LocationId : null,
                Timestamp = o.Cashgame.Any() ? o.Cashgame.First().Timestamp : DateTime.MinValue
            });

        var dtos = await query.ToListAsync();
        return dtos.ToEvents();
    }
    
    public async Task<IList<string>> FindBySlug(string slug)
    {
        var query = db.PbEvent
            .Where(o => o.Bunch.Name == slug)
            .Select(o => o.EventId);

        var ids = await query.ToListAsync();
        return ids.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        var query = EventCashgameQuery.Select(Schema.EventCashgame.EventId).Where(Schema.EventCashgame.EventId, int.Parse(cashgameId));
        var result = await dbold.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Event e)
    {
        var sql = $"""
                  INSERT INTO {Schema.Event} 
                  (
                    {Schema.Event.BunchId.AsParam()},
                    {Schema.Event.Name.AsParam()}
                  )
                  VALUES
                  (
                    (SELECT {Schema.Bunch.Id} FROM {Schema.Bunch} WHERE {Schema.Bunch.Name} = @{Schema.Bunch.Slug.AsParam()}),
                    @{Schema.Event.Name.AsParam()}
                  )
                  RETURNING {Schema.Event.Id.AsParam()}
                  """;
        
        var parameters = new Dictionary<string, object?>
        {
            { Schema.Event.Name.AsParam(), e.Name },
            { Schema.Bunch.Slug.AsParam(), e.BunchSlug }
        };

        var result = await dbold.CustomInsert(sql, parameters);
        return result.ToString();
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.EventCashgame.EventId, int.Parse(eventId) },
            { Schema.EventCashgame.CashgameId, int.Parse(cashgameId) }
        };

        await dbold.InsertAsync(EventCashgameQuery, parameters);
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        var query = EventCashgameQuery
            .Where(Schema.EventCashgame.EventId, int.Parse(eventId))
            .Where(Schema.EventCashgame.CashgameId, int.Parse(cashgameId));

        await dbold.DeleteAsync(query);
    }
}