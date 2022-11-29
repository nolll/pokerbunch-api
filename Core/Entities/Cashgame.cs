using System;
using System.Linq;
using Core.Entities.Checkpoints;

namespace Core.Entities;

public class Cashgame : IEntity
{
    public IList<Checkpoint> Checkpoints { get; }
    public IList<Checkpoint> AddedCheckpoints { get; }
    public IList<Checkpoint> UpdatedCheckpoints { get; }
    public IList<Checkpoint> DeletedCheckpoints { get; }
    public string Id { get; }
    public string BunchId { get; }
    public string LocationId { get; }
    public string EventId { get; }
    public GameStatus Status { get; private set; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public IList<CashgameResult> Results { get; private set; }
    public int PlayerCount { get; private set; }
    public int Turnover { get; private set; }
    public int AverageBuyin { get; private set; }
        
    public Cashgame(string bunchId, string locationId, string eventId, GameStatus status, string id = null, IList<Checkpoint> checkpoints = null)
    {
        Id = id;
        BunchId = bunchId;
        LocationId = locationId;
        EventId = eventId;
        Status = status;
        Checkpoints = checkpoints ?? new List<Checkpoint>();
        CheckpointsUpdated();
        AddedCheckpoints = new List<Checkpoint>();
        UpdatedCheckpoints = new List<Checkpoint>();
        DeletedCheckpoints = new List<Checkpoint>();
    }

    public void ChangeStatus(GameStatus status)
    {
        Status = status;
    }

    public bool IsReadyToEnd
    {
        get
        {
            return Checkpoints.Count > 0 && Results.Count(o => !o.HasCachedOut) == 0;
        }
    }

    private void CheckpointsUpdated()
    {
        Results = CreateResults(Checkpoints);
        StartTime = GetStartTime(Results);
        EndTime = GetEndTime(Results);
        PlayerCount = Results.Count;
        Turnover = GetBuyinSum(Results);
        AverageBuyin = GetAverageBuyin(Turnover, PlayerCount);
    }

    public Checkpoint GetCheckpoint(string checkpointId)
    {
        return Checkpoints.FirstOrDefault(o => o.Id == checkpointId);
    }

    private static IList<CashgameResult> CreateResults(IEnumerable<Checkpoint> checkpoints)
    {
        var map = new Dictionary<string, IList<Checkpoint>>();
        foreach (var checkpoint in checkpoints)
        {
            IList<Checkpoint> list;
            if (!map.TryGetValue(checkpoint.PlayerId, out list))
            {
                list = new List<Checkpoint>();
                map.Add(checkpoint.PlayerId, list);
            }
            list.Add(checkpoint);
        }

        var results = new List<CashgameResult>();
        foreach (var playerKey in map.Keys)
        {
            var playerCheckpoints = map[playerKey].OrderBy(o => o.Timestamp).ToList();
            var playerResults = new CashgameResult(playerKey, playerCheckpoints);
            results.Add(playerResults);
        }
        return results;
    }

    private static DateTime? GetStartTime(IEnumerable<CashgameResult> results)
    {
        DateTime? startTime = null;
        foreach (var result in results)
        {
            if (!startTime.HasValue || result.BuyinTime < startTime)
            {
                startTime = result.BuyinTime;
            }
        }
        return startTime;
    }

    private static DateTime? GetEndTime(IEnumerable<CashgameResult> results)
    {
        DateTime? endTime = null;
        foreach (var result in results)
        {
            if (!endTime.HasValue || result.CashoutTime > endTime)
            {
                endTime = result.CashoutTime;
            }
        }
        return endTime;
    }

    private static int GetBuyinSum(IEnumerable<CashgameResult> results)
    {
        return results.Sum(result => result.Buyin);
    }

    private static int GetAverageBuyin(int turnover, int playerCount)
    {
        if (playerCount == 0)
            return 0;
        return (int)Math.Round(turnover / (double)playerCount);
    }

    public void AddCheckpoint(Checkpoint checkpoint)
    {
        Checkpoints.Add(checkpoint);
        AddedCheckpoints.Add(checkpoint);
        CheckpointsUpdated();
    }

    public void UpdateCheckpoint(Checkpoint checkpoint)
    {
        var oldCheckpoint = Checkpoints.First(o => o.Id == checkpoint.Id);
        Checkpoints[Checkpoints.IndexOf(oldCheckpoint)] = checkpoint;
        UpdatedCheckpoints.Add(checkpoint);
        CheckpointsUpdated();
    }

    public void DeleteCheckpoint(Checkpoint checkpoint)
    {
        Checkpoints.Remove(checkpoint);
        DeletedCheckpoints.Add(checkpoint);
        CheckpointsUpdated();
    }
        
    public int Duration
    {
        get
        {
            if (!StartTime.HasValue || !EndTime.HasValue)
                return 0;
            var timespan = EndTime - StartTime;
            return (int) Math.Round(timespan.Value.TotalMinutes);
        }
    }

    public CashgameResult GetResult(string playerId)
    {
        return Results.FirstOrDefault(result => result.PlayerId == playerId);
    }

    public bool IsInGame(string playerId)
    {
        return GetResult(playerId) != null;
    }

    public bool IsBestResult(CashgameResult resultToCheck)
    {
        var bestResult = GetBestResult();
        return bestResult != null && resultToCheck.Winnings == bestResult.Winnings;
    }

    public CashgameResult GetBestResult()
    {
        CashgameResult bestResult = null;
        foreach(var result in Results)
        {
            if(bestResult == null || result.Winnings > bestResult.Winnings){
                bestResult = result;
            }
        }
        return bestResult;
    }
}