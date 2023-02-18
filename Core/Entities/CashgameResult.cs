using System;
using System.Linq;
using Core.Entities.Checkpoints;

namespace Core.Entities;

public class CashgameResult
{
    public string PlayerId { get; }
    public int Buyin { get; }
    public int Winnings { get; }
    public IList<Checkpoint> Checkpoints { get; }
    public DateTime? BuyinTime { get; }
    public DateTime? CashoutTime { get; }
    public int Stack { get; }
    public DateTime LastReportTime { get; }
    public Checkpoint? CashoutCheckpoint { get; }
    public bool HasCachedOut => CashoutCheckpoint != null;

    public CashgameResult(string playerId, IList<Checkpoint> checkpoints)
    {
        PlayerId = playerId;
        Stack = GetStack(checkpoints);
        Buyin = GetBuyinSum(checkpoints);
        Winnings = Stack - Buyin;
        BuyinTime = GetBuyinTime(checkpoints);
        LastReportTime = GetLastReportTime(checkpoints);
        CashoutCheckpoint = GetCashoutCheckpoint(checkpoints);
        if (CashoutCheckpoint != null)
            CashoutTime = CashoutCheckpoint.Timestamp;
        Checkpoints = checkpoints;
    }

    private static int GetBuyinSum(IEnumerable<Checkpoint> checkpoints)
    {
        var buyinCheckpoints = GetCheckpointsOfType(checkpoints, CheckpointType.Buyin);
        return buyinCheckpoints.Sum(checkpoint => checkpoint.Amount);
    }

    private static List<Checkpoint> GetCheckpointsOfType(IEnumerable<Checkpoint> checkpoints, CheckpointType type)
    {
        return checkpoints.Where(checkpoint => checkpoint.Type == type).ToList();
    }

    private static int GetStack(IList<Checkpoint> checkpoints)
    {
        var checkpoint = GetLastCheckpoint(checkpoints);
        return checkpoint?.Stack ?? 0;
    }

    private static Checkpoint? GetLastCheckpoint(IList<Checkpoint> checkpoints)
    {
        return checkpoints.Count > 0 ? checkpoints[^1] : null;
    }
    
    private static DateTime? GetBuyinTime(IEnumerable<Checkpoint> checkpoints)
    {
        var checkpoint = GetFirstBuyinCheckpoint(checkpoints);
        if (checkpoint == null)
            return null;
        return checkpoint.Timestamp;
    }

    private static Checkpoint? GetFirstBuyinCheckpoint(IEnumerable<Checkpoint> checkpoints)
    {
        return GetCheckpointOfType(checkpoints, CheckpointType.Buyin);
    }

    private static Checkpoint? GetCashoutCheckpoint(IEnumerable<Checkpoint> checkpoints)
    {
        return GetCheckpointOfType(checkpoints, CheckpointType.Cashout);
    }

    private static Checkpoint? GetCheckpointOfType(IEnumerable<Checkpoint> checkpoints, CheckpointType type)
    {
        return checkpoints.FirstOrDefault(checkpoint => checkpoint.Type == type);
    }
    
    private static DateTime GetLastReportTime(IList<Checkpoint> checkpoints)
    {
        var checkpoint = GetLastCheckpoint(checkpoints);
        return checkpoint?.Timestamp ?? DateTime.MinValue;
    }
}