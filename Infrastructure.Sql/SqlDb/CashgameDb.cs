using System.Linq;
using Core;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;
using SqlKata.Execution;

namespace Infrastructure.Sql.SqlDb;

public class CashgameDb
{
    private readonly IDb _db;

    private static Query CashgameQuery => new(Schema.Cashgame);

    private static Query GetQuery => CashgameQuery
        .Select(
            Schema.Cashgame.Id.FullName,
            Schema.Cashgame.BunchId.FullName,
            Schema.Cashgame.LocationId.FullName,
            Schema.EventCashgame.EventId.FullName,
            Schema.Cashgame.Status.FullName)
        .LeftJoin(Schema.EventCashgame, Schema.EventCashgame.CashgameId.FullName, Schema.Cashgame.Id.FullName);

    public CashgameDb(IDb db)
    {
        _db = db;
    }

    public async Task<Cashgame> Get(string cashgameId)
    {
        var query = GetQuery.Where(Schema.Cashgame.Id.FullName, cashgameId)
            .OrderBy(Schema.Cashgame.Id.FullName);
        var cashgameDto = await _db.QueryFactory.FromQuery(query).FirstOrDefaultAsync<CashgameDto>();

        if (cashgameDto is null)
            throw new PokerBunchException($"Cashgame with id {cashgameId} was not found");

        var checkpointDtos = await GetCheckpoints(cashgameId);
        return cashgameDto.ToCashgame(checkpointDtos);
    }
        
    public async Task<IList<Cashgame>> Get(IList<string> ids)
    {
        if(ids.Count == 0)
            return new List<Cashgame>();

        var query = GetQuery.WhereIn(Schema.Cashgame.Id.FullName, ids.Select(int.Parse))
            .OrderBy(Schema.Cashgame.Id.FullName);

        var cashgameDtos = await _db.QueryFactory.FromQuery(query).GetAsync<CashgameDto>();
        var checkpointDtos = await GetCheckpoints(ids);
        return cashgameDtos.ToCashgameList(checkpointDtos);
    }

    public async Task<IList<string>> FindFinished(string bunchId)
    {
        return await FindByBunchAndStatus(bunchId, GameStatus.Finished);
    }

    public async Task<IList<string>> FindRunning(string bunchId)
    {
        return await FindByBunchAndStatus(bunchId, GameStatus.Running);
    }

    private async Task<IList<string>> FindByBunchAndStatus(string bunchId, GameStatus status)
    {
        var @params = new
        {
            bunchId = int.Parse(bunchId),
            status = (int)status,
        };

        return (await _db.List<int>(CashgameSql.SearchByBunchAndStatusQuery, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindFinished(string bunchId, int year)
    {
        var @params = new
        {
            bunchId = int.Parse(bunchId),
            status = (int)GameStatus.Finished,
            year
        };
        
        return (await _db.List<int>(CashgameSql.SearchByBunchAndStatusAndYearQuery(_db.Engine), @params))
            .Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByEvent(string eventId)
    {
        var @params = new
        {
            eventId = int.Parse(eventId)
        };
        
        return (await _db.List<int>(CashgameSql.SearchByEvent, @params)).Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCheckpoint(string checkpointId)
    {
        var @params = new
        {
            checkpointId = int.Parse(checkpointId)
        };
        
        return (await _db.List<int>(CashgameSql.SearchByCheckpointSql, @params)).Select(o => o.ToString()).ToList();
    }
    
    public async Task DeleteGame(string id)
    {
        var @params = new
        {
            cashgameId = int.Parse(id)
        };

        await _db.Execute(CashgameSql.DeleteQuery, @params);
    }
        
    public async Task<string> AddGame(Bunch bunch, Cashgame cashgame)
    {
        var @params = new
        {
            bunchId = int.Parse(bunch.Id),
            locationId = int.Parse(cashgame.LocationId),
            status = (int)cashgame.Status,
            date = TimeZoneInfo.ConvertTime(DateTime.UtcNow, bunch.Timezone)
        };

        return (await _db.Insert(CashgameSql.AddQuery, @params)).ToString();
    }
        
    public async Task UpdateGame(Cashgame cashgame)
    {
        var @params = new
        {
            locationId = int.Parse(cashgame.LocationId),
            status = (int)cashgame.Status,
            cashgameId = int.Parse(cashgame.Id)
        };

        if (cashgame.AddedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.AddedCheckpoints)
            {
                await AddCheckpoint(checkpoint);
            }
        }
        if (cashgame.UpdatedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.UpdatedCheckpoints)
            {
                await UpdateCheckpoint(checkpoint);
            }
        }
        if (cashgame.DeletedCheckpoints.Any())
        {
            foreach (var checkpoint in cashgame.DeletedCheckpoints)
            {
                await DeleteCheckpoint(checkpoint);
            }
        }
        await _db.Execute(CashgameSql.UpdateQuery, @params);
    }
        
    public async Task<IList<string>> FindByPlayerId(string playerId)
    {
        var @params = new
        {
            playerId = int.Parse(playerId)
        };

        return (await _db.List<int>(CashgameSql.SearchByPlayerIdQuery, @params)).Select(o => o.ToString()).ToList();
    }

    private async Task<int> AddCheckpoint(Checkpoint checkpoint)
    {
        var @params = new
        {
            cashgameId = int.Parse(checkpoint.CashgameId),
            playerId = int.Parse(checkpoint.PlayerId),
            type = (int)checkpoint.Type,
            amount = checkpoint.Amount,
            stack = checkpoint.Stack,
            timestamp = checkpoint.Timestamp.ToUniversalTime()
        };

        return await _db.Insert(CashgameSql.AddCheckpointQuery, @params);
    }

    private async Task UpdateCheckpoint(Checkpoint checkpoint)
    {
        var @params = new
        {
            timestamp = checkpoint.Timestamp,
            amount = checkpoint.Amount,
            stack = checkpoint.Stack,
            checkpointId = int.Parse(checkpoint.Id)
        };

        await _db.Execute(CashgameSql.UpdateCheckpointQuery, @params);
    }

    private async Task DeleteCheckpoint(Checkpoint checkpoint)
    {
        var @params = new
        {
            checkpointId = int.Parse(checkpoint.Id)
        };

        await _db.Execute(CashgameSql.DeleteCheckpointQuery, @params);
    }

    private async Task<IList<CheckpointDto>> GetCheckpoints(string cashgameId)
    {
        var @params = new
        {
            cashgameId = int.Parse(cashgameId)
        };

        return (await _db.List<CheckpointDto>(CashgameSql.GetCheckpointsByCashgameQuery, @params)).ToList();
    }

    private async Task<IList<CheckpointDto>> GetCheckpoints(IList<string> cashgameIdList)
    {
        var param = new ListParam("@cashgameIds", cashgameIdList.Select(int.Parse));
        return (await _db.List<CheckpointDto>(CashgameSql.GetCheckpointsByCashgamesQuery, param)).ToList();
    }
}