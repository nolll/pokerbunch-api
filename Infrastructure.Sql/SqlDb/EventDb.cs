using System;
using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class EventDb
{
    private readonly IDb _db;

    private static Query EventQuery => new(Schema.Event);
    private static Query EventCashgameQuery => new(Schema.EventCashgame);

    private static Query CheckpointJoinQuery => new Query(Schema.CashgameCheckpoint)
        .Select(Schema.CashgameCheckpoint.CheckpointId, Schema.CashgameCheckpoint.CashgameId)
        .OrderByDesc(Schema.CashgameCheckpoint.Timestamp)
        .Limit(1);

    private static Query GetQuery => EventQuery
        .Select(
            Schema.Event.Id,
            Schema.Event.BunchId,
            Schema.Event.Name,
            Schema.Cashgame.LocationId,
            Schema.Cashgame.Timestamp)
        .LeftJoin(Schema.EventCashgame, Schema.EventCashgame.EventId, Schema.Event.Id)
        .LeftJoin(Schema.Cashgame, Schema.EventCashgame.CashgameId, Schema.Cashgame.Id)
        .LeftJoin(CheckpointJoinQuery.As("j"), j => j.On($"j.{Schema.Cashgame.Id.AsParam()}", Schema.Cashgame.Id))
        .OrderBy(Schema.Event.Id, Schema.Cashgame.Date);

    public EventDb(IDb db)
    {
        _db = db;
    }

    public async Task<Event> Get(string id)
    {
        var query = GetQuery.Where(Schema.Event.Id, int.Parse(id));
        var eventDayDtos = await _db.GetAsync<EventDayDto>(query);

        var events = eventDayDtos.ToEvents();
        var @event = events.FirstOrDefault();

        if (@event is null)
            throw new PokerBunchException($"Event with id {id} was not found");

        return @event;
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        var query = GetQuery.WhereIn(Schema.Event.Id, ids.Select(int.Parse));
        var eventDayDtos = await _db.GetAsync<EventDayDto>(query);

        return eventDayDtos.ToEvents();
    }

    public async Task<IList<string>> FindByBunchId(string bunchId)
    {
        var query = EventQuery.Select(Schema.Event.Id).Where(Schema.Event.BunchId, int.Parse(bunchId));
        var result = await _db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        var query = EventCashgameQuery.Select(Schema.EventCashgame.EventId).Where(Schema.EventCashgame.EventId, int.Parse(cashgameId));
        var result = await _db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Event e)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.Event.Name, e.Name },
            { Schema.Event.BunchId, int.Parse(e.BunchId) }
        };

        var result = await _db.InsertGetIdAsync(EventQuery, parameters);
        return result.ToString();
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.EventCashgame.EventId, int.Parse(eventId) },
            { Schema.EventCashgame.CashgameId, int.Parse(cashgameId) }
        };

        await _db.InsertAsync(EventCashgameQuery, parameters);
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        var query = EventCashgameQuery
            .Where(Schema.EventCashgame.EventId, int.Parse(eventId))
            .Where(Schema.EventCashgame.CashgameId, int.Parse(cashgameId));

        await _db.DeleteAsync(query);
    }
}