using System;

namespace Core.Entities.Checkpoints;

public abstract class Checkpoint(
    string cashgameId,
    string playerId,
    DateTime timestamp,
    CheckpointType type,
    int stack,
    int amount,
    string? id)
{
    public string CashgameId { get; } = cashgameId;
    public string PlayerId { get; } = playerId;
    public int Amount { get; } = amount;
    public int Stack { get; } = stack;
    public DateTime Timestamp { get; } = timestamp;
    public CheckpointType Type { get; } = type;
    public string Id { get; } = id ?? "";

    public abstract string Description { get; }

    public static Checkpoint Create(string? id, string cashgameId, string playerId, DateTime timestamp, CheckpointType type, int stack, int amount = 0)
    {
        if (type == CheckpointType.Cashout)
            return new CashoutCheckpoint(cashgameId, playerId, timestamp, stack, amount, id);
        if (type == CheckpointType.Buyin)
            return new BuyinCheckpoint(cashgameId, playerId, timestamp, stack, amount, id);
        return new ReportCheckpoint(cashgameId, playerId, timestamp, stack, amount, id);
    }
}