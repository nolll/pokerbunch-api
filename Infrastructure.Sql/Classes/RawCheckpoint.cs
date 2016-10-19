using System;
using Core.Entities.Checkpoints;

namespace Infrastructure.Sql.Classes
{
	public class RawCheckpoint
    {
        public int CashgameId { get; }
        public int PlayerId { get; }
	    public int Amount { get; }
	    public int Stack { get; }
	    public DateTime Timestamp { get; }
	    public int Id { get; }
        public int Type { get; }

	    public RawCheckpoint(int cashgameId, int playerId, int amount, int stack, DateTime timestamp, int id, int type)
	    {
	        CashgameId = cashgameId;
	        PlayerId = playerId;
	        Amount = amount;
	        Stack = stack;
	        Timestamp = timestamp;
	        Id = id;
	        Type = type;
	    }

        public static RawCheckpoint Create(Checkpoint checkpoint)
        {
            return new RawCheckpoint(
                checkpoint.CashgameId,
                checkpoint.PlayerId,
                checkpoint.Amount,
                checkpoint.Stack,
                checkpoint.Timestamp,
                checkpoint.Id,
                (int)checkpoint.Type);
        }

        public static Checkpoint CreateReal(RawCheckpoint rawCheckpoint)
        {
            return Checkpoint.Create(
                rawCheckpoint.CashgameId,
                rawCheckpoint.PlayerId,
                rawCheckpoint.Timestamp,
                (CheckpointType)rawCheckpoint.Type,
                rawCheckpoint.Stack,
                rawCheckpoint.Amount,
                rawCheckpoint.Id);
        }
    }
}