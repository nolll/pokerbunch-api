using System;

namespace Core.Entities.Checkpoints;

public abstract class Checkpoint
{
    public string CashgameId { get; }
    public string PlayerId { get; }
    public int Amount { get; }
    public int Stack { get; }
    public DateTime Timestamp { get; }
    public CheckpointType Type { get; }
    public string Id { get; }
        
    protected Checkpoint(
        string cashgameId,
        string playerId,
        DateTime timestamp, 
        CheckpointType type,
        int stack,
        int amount,
        string id)
    {
        Timestamp = timestamp;
        Type = type;
        Stack = stack;
        Amount = amount;
        Id = id;
        PlayerId = playerId;
        CashgameId = cashgameId;
    }

    public abstract string Description { get; }

    public static Checkpoint Create(string cashgameId, string playerId, DateTime timestamp, CheckpointType type, int stack, int amount = 0, string id = null)
    {
        if (type == CheckpointType.Cashout)
            return new CashoutCheckpoint(cashgameId, playerId, timestamp, stack, amount, id);
        if (type == CheckpointType.Buyin)
            return new BuyinCheckpoint(cashgameId, playerId, timestamp, stack, amount, id);
        return new ReportCheckpoint(cashgameId, playerId, timestamp, stack, amount, id);
    }
}