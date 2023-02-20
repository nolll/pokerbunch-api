using System.Linq;
using Core;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql.SqlDb;

public class EventDb
{
    private readonly IDb _db;

    private static Query TableQuery => new(Schema.Event);

    private static Query CheckpointJoinQuery => new Query(Schema.CashgameCheckpoint)
        .Select(Schema.CashgameCheckpoint.CheckpointId, Schema.CashgameCheckpoint.CashgameId)
        .OrderByDesc(Schema.CashgameCheckpoint.Timestamp)
        .Limit(1);

    private static Query GetQuery => TableQuery
        .Select(
            Schema.Event.Id.FullName,
            Schema.Event.BunchId.FullName,
            Schema.Event.Name.FullName,
            Schema.Cashgame.LocationId.FullName,
            Schema.Cashgame.Timestamp.FullName)
        .LeftJoin(Schema.EventCashgame, Schema.EventCashgame.EventId.FullName, Schema.Event.Id.FullName)
        .LeftJoin(Schema.Cashgame, Schema.EventCashgame.CashgameId.FullName, Schema.Cashgame.Id.FullName)
        .LeftJoin(CheckpointJoinQuery.As("j"), j => j.On($"j.{Schema.Cashgame.Id}", Schema.Cashgame.Id.FullName))
        .OrderBy(Schema.Event.Id.FullName, Schema.Cashgame.Date.FullName);

    public EventDb(IDb db)
    {
        _db = db;
    }

    public async Task<Event> Get(string id)
    {
        var query = GetQuery.Where(Schema.Event.Id.FullName, int.Parse(id));
        var eventDayDtos = await _db.QueryFactory.FromQuery(query).GetAsync<EventDayDto>();

        var events = eventDayDtos.ToEvents();
        var @event = events.FirstOrDefault();

        if (@event is null)
            throw new PokerBunchException($"Event with id {id} was not found");

        return @event;
    }

    public async Task<IList<Event>> Get(IList<string> ids)
    {
        var query = GetQuery.WhereIn(Schema.Event.Id.FullName, ids.Select(int.Parse));
        var eventDayDtos = await _db.QueryFactory.FromQuery(query).GetAsync<EventDayDto>();

        return eventDayDtos.ToEvents();
    }

    public async Task<IList<string>> FindByBunchId(string bunchId)
    {
        var @params = new
        {
            id = int.Parse(bunchId)
        };
        
        return (await _db.List<int>(EventSql.SearchByIdQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCashgameId(string cashgameId)
    {
        var @params = new
        {
            id = int.Parse(cashgameId)
        };

        return (await _db.List<int>(EventSql.SearchByCashgameQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<string> Add(Event e)
    {
        var @params = new
        {
            name = e.Name,
            bunchId = int.Parse(e.BunchId)
        };

        return (await _db.Insert(EventSql.AddQuery, @params)).ToString();
    }

    public async Task AddCashgame(string eventId, string cashgameId)
    {
        var @params = new
        {
            eventId = int.Parse(eventId),
            cashgameId = int.Parse(cashgameId)
        };

        await _db.Insert(EventSql.AddCashgameQuery, @params);
    }

    public async Task RemoveCashgame(string eventId, string cashgameId)
    {
        var @params = new
        {
            eventId = int.Parse(eventId),
            cashgameId = int.Parse(cashgameId)
        };

        await _db.Execute(EventSql.RemoveCashgameQuery, @params);
    }
}