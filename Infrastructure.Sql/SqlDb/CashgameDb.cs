using System.Linq;
using Core;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Mappers;
using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Sql.SqlDb;

public class CashgameDb(PokerBunchDbContext db)
{
    public async Task<Cashgame> Get(string id)
    {
        var cashgames = await Get([id]);

        return cashgames.Count != 0 
            ? cashgames.First() 
            : throw new PokerBunchException($"Cashgame with id {id} was not found");
    }
        
    public async Task<IList<Cashgame>> Get(IList<string> ids)
    {
        if(ids.Count == 0)
            return new List<Cashgame>();

        var q = db.PbCashgame
            .Include(o => o.Event)
            .Include(o => o.Bunch)
            .Where(o => ids.Select(int.Parse).Contains(o.CashgameId))
            .Select(o => new CashgameDto
            {
                CashgameId = o.CashgameId,
                BunchSlug = o.Bunch.Name,
                LocationId = o.LocationId,
                EventId = o.Event.Count > 0 ? o.Event.First().EventId : null,
                Status = o.Status
            });

        var dtos = await q.ToListAsync();
        var checkpointDtos = await GetCheckpoints(ids);
        return dtos.ToCashgameList(checkpointDtos);
    }
    
    public async Task<IList<string>> FindFinished(string slug) => await FindByBunchAndStatus(slug, GameStatus.Finished);
    public async Task<IList<string>> FindRunning(string slug) => await FindByBunchAndStatus(slug, GameStatus.Running);
    
    private async Task<IList<string>> FindByBunchAndStatus(string slug, GameStatus status)
    {
        var query = FindByBunchAndStatusQuery(slug, status)
            .Select(o => o.CashgameId);

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }

    private IQueryable<PbCashgame> FindByBunchAndStatusQuery(string slug, GameStatus status) => db.PbCashgame
        .Include(o => o.Bunch)
        .Where(o => o.Bunch.Name == slug)
        .Where(o => o.Status == (int)status);

    public async Task<IList<string>> FindFinished(string slug, int year)
    {
        var query = FindByBunchAndStatusQuery(slug, GameStatus.Finished)
            .Where(o => o.Date.Year == year)
            .Select(o => o.CashgameId);
        
        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByEvent(string eventId)
    {
        var q = db.PbEvent
            .Where(o => o.EventId == int.Parse(eventId))
            .SelectMany(o => o.Cashgame.Select(c => c.CashgameId));

        var ids = await q.ToListAsync();
        return ids.Select(o => o.ToString()).ToList();
    }

    public async Task<IList<string>> FindByCheckpoint(string checkpointId)
    {
        var query = db.PbCashgameCheckpoint
            .Where(o => o.CheckpointId == int.Parse(checkpointId))
            .Select(o => o.CashgameId);

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }
    
    public async Task DeleteGame(string id)
    {
        var dto = new PbCashgame { CashgameId = int.Parse(id) };
        db.PbCashgame.Remove(dto);
        await db.SaveChangesAsync();
    }
        
    public async Task<string> AddGame(Bunch bunch, Cashgame cashgame)
    {
        var dto = new PbCashgame
        {
            BunchId = int.Parse(bunch.Id),
            LocationId = int.Parse(cashgame.LocationId),
            Status = (int)cashgame.Status,
            Date = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(DateTime.UtcNow, bunch.Timezone))
        };

        db.PbCashgame.Add(dto);
        await db.SaveChangesAsync();
        return dto.CashgameId.ToString();
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

        var dto = db.PbCashgame
            .First(o => o.CashgameId == int.Parse(cashgame.Id));

        dto.LocationId = int.Parse(cashgame.LocationId);
        dto.Status = (int)cashgame.Status;
        

        await db.SaveChangesAsync();
    }
        
    public async Task<IList<string>> FindByPlayerId(string playerId)
    {
        var query = db.PbCashgameCheckpoint
            .Where(o => o.PlayerId == int.Parse(playerId))
            .Select(o => o.CashgameId)
            .Distinct();

        var result = await query.ToListAsync();
        return result.Select(o => o.ToString()).ToList();
    }

    private async Task<int> AddCheckpoint(Checkpoint checkpoint)
    {
        var dto = new PbCashgameCheckpoint
        {
            CashgameId = int.Parse(checkpoint.CashgameId),
            PlayerId = int.Parse(checkpoint.PlayerId),
            Type = (int)checkpoint.Type,
            Amount = checkpoint.Amount,
            Stack = checkpoint.Stack,
            Timestamp = DateTime.SpecifyKind(checkpoint.Timestamp.ToUniversalTime(), DateTimeKind.Unspecified)
        };

        db.PbCashgameCheckpoint.Add(dto);
        await db.SaveChangesAsync();
        return dto.CheckpointId;
    }

    private async Task UpdateCheckpoint(Checkpoint checkpoint)
    {
        var dto = db.PbCashgameCheckpoint
            .First(o => o.CheckpointId == int.Parse(checkpoint.Id));

        dto.Timestamp = DateTime.SpecifyKind(checkpoint.Timestamp, DateTimeKind.Unspecified);
        dto.Amount = checkpoint.Amount;
        dto.Stack = checkpoint.Stack;

        await db.SaveChangesAsync();
    }

    private async Task DeleteCheckpoint(Checkpoint checkpoint)
    {
        var dto = new PbCashgameCheckpoint() { CheckpointId = int.Parse(checkpoint.Id) };
        db.PbCashgameCheckpoint.Remove(dto);
        await db.SaveChangesAsync();
    }
    
    private async Task<IList<CheckpointDto>> GetCheckpoints(IList<string> cashgameIds)
    {
        var q = db.PbCashgameCheckpoint
            .Where(o => cashgameIds.Select(int.Parse).Contains(o.CashgameId))
            .OrderBy(o => o.PlayerId)
            .ThenBy(o => o.Timestamp)
            .ThenByDescending(o => o.CheckpointId)
            .Select(o => new CheckpointDto
            {
                CashgameId = o.CashgameId,
                CheckpointId = o.CheckpointId,
                PlayerId = o.PlayerId,
                Type = o.Type,
                Stack = o.Stack,
                Amount = o.Amount,
                Timestamp = o.Timestamp
            });

        return await q.ToListAsync();
    }
}