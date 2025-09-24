using System;
using System.Linq;
using Core.Entities.Checkpoints;

namespace Core.Entities;

public class Cashgame : IEntity
{
    public IList<Checkpoint> Checkpoints { get; private set; }
    public IList<Checkpoint> AddedCheckpoints { get; }
    public IList<Checkpoint> UpdatedCheckpoints { get; }
    public IList<Checkpoint> DeletedCheckpoints { get; }
    public string Id { get; }
    public string BunchId { get; }
    public string BunchSlug { get; }
    public string LocationId { get; }
    public string? EventId { get; }
    public GameStatus Status { get; private set; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public IList<CashgameResult> Results { get; private set; }
    public int PlayerCount { get; private set; }

    public Cashgame(
        string bunchId,
        string bunchSlug,
        string locationId,
        string? eventId,
        GameStatus status,
        string id = "",
        IList<Checkpoint>? checkpoints = null)
    {
        Results = new List<CashgameResult>();
        Id = id;
        BunchId = bunchId;
        BunchSlug = bunchSlug;
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

    public bool IsReadyToEnd => Checkpoints.Count > 0 && Results.All(o => o.HasCachedOut);

    private void CheckpointsUpdated()
    {
        Results = CreateResults(Checkpoints);
        StartTime = GetStartTime(Results);
        EndTime = GetEndTime(Results);
        PlayerCount = Results.Count;
    }

    public Checkpoint GetCheckpoint(string checkpointId) => Checkpoints.First(o => o.Id == checkpointId);

    private static IList<CashgameResult> CreateResults(IEnumerable<Checkpoint> checkpoints)
    {
        var map = new Dictionary<string, IList<Checkpoint>>();
        foreach (var checkpoint in checkpoints)
        {
            if (!map.TryGetValue(checkpoint.PlayerId, out var list))
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

    public void SetCheckpoints(List<Checkpoint> checkpoints)
    {
        Checkpoints = checkpoints;
        CheckpointsUpdated();
    }

    public CashgameResult? GetResult(string playerId) => 
        Results.FirstOrDefault(result => result.PlayerId == playerId);
}