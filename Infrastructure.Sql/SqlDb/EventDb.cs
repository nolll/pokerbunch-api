using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class EventDb(IDb db)
{
    private static Query EventQuery => new(Schema.Event);
    private static Query EventCashgameQuery => new(Schema.EventCashgame);

    private static Query CheckpointJoinQuery => new Query(Schema.CashgameCheckpoint)
        .Select(Schema.CashgameCheckpoint.CheckpointId, Schema.CashgameCheckpoint.CashgameId)
        .OrderByDesc(Schema.CashgameCheckpoint.Timestamp)
        .Limit(1);

    private static Query GetQuery => EventQuery
        .Select(
            Schema.Event.Id,
            Schema.Event.Name,
            Schema.Cashgame.LocationId,
            Schema.Cashgame.Timestamp)
        .SelectRaw($"{Schema.Bunch.Name} AS {Schema.Bunch.Slug.AsParam()}")
        .LeftJoin(Schema.EventCashgame, Schema.EventCashgame.EventId, Schema.Event.Id)
        .LeftJoin(Schema.Cashgame, Schema.EventCashgame.CashgameId, Schema.Cashgame.Id)
        .LeftJoin(Schema.Bunch, Schema.Bunch.Id, Schema.Event.BunchId)
        .LeftJoin(CheckpointJoinQuery.As("j"), j => j.On($"j.{Schema.Cashgame.Id.AsParam()}", Schema.Cashgame.Id))
        .OrderBy(Schema.Event.Id, Schema.Cashgame.Date);
    
    private static Query FindQuery => EventQuery
        .Select(Schema.Event.Id)
        .LeftJoin(Schema.Bunch, Schema.Bunch.Id, Schema.Event.BunchId);

    public async Task<Event> Get(string id)
    {
        var query = GetQuery.Where(Schema.Event.Id, int.Parse(id));
        var eventDayDtos = await db.GetAsync<EventDayDto>(query);

        var events = eventDayDtos.ToEvents();
        var @event = events.FirstOrDefault();

        if (@event is null)
            throw new PokerBunchException($"Event with id {id} was not found");

        return @event;
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        var query = GetQuery.WhereIn(Schema.Event.Id, ids.Select(int.Parse));
        var eventDayDtos = await db.GetAsync<EventDayDto>(query);

        return eventDayDtos.ToEvents();
    }
    
    public async Task<IList<string>> FindBySlug(string slug)
    {
        var query = FindQuery.Where(Schema.Bunch.Name, slug);
        var result = await db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        var query = EventCashgameQuery.Select(Schema.EventCashgame.EventId).Where(Schema.EventCashgame.EventId, int.Parse(cashgameId));
        var result = await db.GetAsync<int>(query);
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

        var result = await db.CustomInsert(sql, parameters);
        return result.ToString();
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.EventCashgame.EventId, int.Parse(eventId) },
            { Schema.EventCashgame.CashgameId, int.Parse(cashgameId) }
        };

        await db.InsertAsync(EventCashgameQuery, parameters);
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        var query = EventCashgameQuery
            .Where(Schema.EventCashgame.EventId, int.Parse(eventId))
            .Where(Schema.EventCashgame.CashgameId, int.Parse(cashgameId));

        await db.DeleteAsync(query);
    }
}