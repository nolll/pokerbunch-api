using System.Linq;
using Core;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Sql;
using SqlKata;

namespace Infrastructure.Sql.SqlDb;

public class CashgameDb(IDb db)
{
    private static Query CashgameQuery => new(Schema.Cashgame);
    private static Query EventCashgameQuery => new(Schema.EventCashgame);
    private static Query CashgameCheckpointQuery => new(Schema.CashgameCheckpoint);

    private static Query GetQuery => CashgameQuery
        .Select(
            Schema.Cashgame.Id,
            Schema.Cashgame.BunchId,
            Schema.Cashgame.LocationId,
            Schema.EventCashgame.EventId,
            Schema.Cashgame.Status)
        .LeftJoin(Schema.EventCashgame, Schema.EventCashgame.CashgameId, Schema.Cashgame.Id);

    private static Query GetCheckpointQuery => CashgameCheckpointQuery
        .Select(
            Schema.CashgameCheckpoint.CashgameId,
            Schema.CashgameCheckpoint.CheckpointId,
            Schema.CashgameCheckpoint.PlayerId,
            Schema.CashgameCheckpoint.Type,
            Schema.CashgameCheckpoint.Stack,
            Schema.CashgameCheckpoint.Amount,
            Schema.CashgameCheckpoint.Timestamp);

    private static Query FindQuery => CashgameQuery.Select(Schema.Cashgame.Id);
    private static Query FindByBunchAndStatusQuery(string bunchId, GameStatus status) => FindQuery
        .Where(Schema.Cashgame.BunchId, int.Parse(bunchId))
        .Where(Schema.Cashgame.Status, (int)status);

    public async Task<Cashgame> Get(string cashgameId)
    {
        var query = GetQuery
            .Where(Schema.Cashgame.Id, int.Parse(cashgameId))
            .OrderBy(Schema.Cashgame.Id);
        
        var cashgameDto = await db.FirstOrDefaultAsync<CashgameDto>(query);

        if (cashgameDto is null)
            throw new PokerBunchException($"Cashgame with id {cashgameId} was not found");

        var checkpointDtos = await GetCheckpoints(cashgameId);
        return cashgameDto.ToCashgame(checkpointDtos);
    }
        
    public async Task<IList<Cashgame>> Get(IList<string> ids)
    {
        if(ids.Count == 0)
            return new List<Cashgame>();

        var query = GetQuery.WhereIn(Schema.Cashgame.Id, ids.Select(int.Parse))
            .OrderBy(Schema.Cashgame.Id);

        var cashgameDtos = await db.GetAsync<CashgameDto>(query);
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
        var query = FindByBunchAndStatusQuery(bunchId, status);

        var result = await db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindFinished(string bunchId, int year)
    {
        var query = FindByBunchAndStatusQuery(bunchId, GameStatus.Finished)
            .WhereDatePart("year", Schema.Cashgame.Date, year);

        var result = await db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByEvent(string eventId)
    {
        var subQuery = EventCashgameQuery
            .Select(Schema.EventCashgame.CashgameId)
            .Where(Schema.EventCashgame.EventId, int.Parse(eventId));

        var query = FindQuery.WhereIn(Schema.Cashgame.Id, subQuery);
        var result = await db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCheckpoint(string checkpointId)
    {
        var query = CashgameCheckpointQuery.Select(Schema.CashgameCheckpoint.CashgameId)
            .Where(Schema.CashgameCheckpoint.CheckpointId, int.Parse(checkpointId));

        var result = await db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }
    
    public async Task DeleteGame(string id)
    {
        var query = CashgameQuery.Where(Schema.Cashgame.Id, int.Parse(id));
        await db.DeleteAsync(query);
    }
        
    public async Task<string> AddGame(Bunch bunch, Cashgame cashgame)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.Cashgame.BunchId, int.Parse(bunch.Id) },
            { Schema.Cashgame.LocationId, int.Parse(cashgame.LocationId) },
            { Schema.Cashgame.Status, (int)cashgame.Status },
            { Schema.Cashgame.Date, TimeZoneInfo.ConvertTime(DateTime.UtcNow, bunch.Timezone) }
        };

        var result = await db.InsertGetIdAsync(CashgameQuery, parameters);
        return result.ToString();
    }
        
    public async Task UpdateGame(Cashgame cashgame)
    {
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
        
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.Cashgame.LocationId, int.Parse(cashgame.LocationId) },
            { Schema.Cashgame.Status, (int)cashgame.Status }
        };

        var query = CashgameQuery.Where(Schema.Cashgame.Id, int.Parse(cashgame.Id));
        await db.UpdateAsync(query, parameters);
    }
        
    public async Task<IList<string>> FindByPlayerId(string playerId)
    {
        var query = CashgameCheckpointQuery
            .Select(Schema.CashgameCheckpoint.CashgameId)
            .Distinct()
            .Where(Schema.CashgameCheckpoint.PlayerId, int.Parse(playerId));

        var result = await db.GetAsync<int>(query);
        return result.Select(o => o.ToString()).ToList();
    }

    private async Task<int> AddCheckpoint(Checkpoint checkpoint)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.CashgameCheckpoint.CashgameId, int.Parse(checkpoint.CashgameId) },
            { Schema.CashgameCheckpoint.PlayerId, int.Parse(checkpoint.PlayerId) },
            { Schema.CashgameCheckpoint.Type, (int)checkpoint.Type },
            { Schema.CashgameCheckpoint.Amount, checkpoint.Amount },
            { Schema.CashgameCheckpoint.Stack, checkpoint.Stack },
            { Schema.CashgameCheckpoint.Timestamp, checkpoint.Timestamp.ToUniversalTime() }
        };

        return await db.InsertGetIdAsync(CashgameCheckpointQuery, parameters);
    }

    private async Task UpdateCheckpoint(Checkpoint checkpoint)
    {
        var parameters = new Dictionary<SqlColumn, object?>
        {
            { Schema.CashgameCheckpoint.Timestamp, checkpoint.Timestamp },
            { Schema.CashgameCheckpoint.Amount, checkpoint.Amount },
            { Schema.CashgameCheckpoint.Stack, checkpoint.Stack }
        };

        var query = CashgameCheckpointQuery
            .Where(Schema.CashgameCheckpoint.CheckpointId, int.Parse(checkpoint.Id));

        await db.UpdateAsync(query, parameters);
    }

    private async Task DeleteCheckpoint(Checkpoint checkpoint)
    {
        var query = CashgameCheckpointQuery.Where(Schema.CashgameCheckpoint.CheckpointId, int.Parse(checkpoint.Id));
        await db.DeleteAsync(query);
    }

    private async Task<IList<CheckpointDto>> GetCheckpoints(string cashgameId)
    {
        var query = GetCheckpointQuery
            .Where(
                Schema.CashgameCheckpoint.CashgameId, int.Parse(cashgameId))
            .OrderBy(
                Schema.CashgameCheckpoint.PlayerId,
                Schema.CashgameCheckpoint.Timestamp,
                Schema.CashgameCheckpoint.CheckpointId
            );

        return (await db.GetAsync<CheckpointDto>(query)).ToList();
    }

    private async Task<IList<CheckpointDto>> GetCheckpoints(IList<string> cashgameIdList)
    {
        var query = GetCheckpointQuery
            .WhereIn(
                Schema.CashgameCheckpoint.CashgameId, cashgameIdList.Select(int.Parse))
            .OrderBy(Schema.CashgameCheckpoint.PlayerId)
            .OrderBy(Schema.CashgameCheckpoint.Timestamp)
            .OrderByDesc(Schema.CashgameCheckpoint.CheckpointId);

        return (await db.GetAsync<CheckpointDto>(query)).ToList();
    }
}