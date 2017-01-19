using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities.Checkpoints;

namespace Core.Entities
{
    public class CashgameResult
    {
        public int PlayerId { get; private set; }
        public int Buyin { get; }
        public int Winnings { get; }
        public IList<Checkpoint> Checkpoints { get; private set; }
        public DateTime? BuyinTime { get; }
        public DateTime? CashoutTime { get; }
        public int PlayedTime { get; }
        public int Stack { get; }
        public DateTime LastReportTime { get; private set; }
        public Checkpoint CashoutCheckpoint { get; }
        public int WinRate { get; private set; }

        public CashgameResult(int playerId, IList<Checkpoint> checkpoints)
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
            PlayedTime = GetPlayedTime(BuyinTime, CashoutTime);
            WinRate = GetWinRate(Winnings, PlayedTime);
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
            return checkpoint != null ? checkpoint.Stack : 0;
        }

        private static Checkpoint GetLastCheckpoint(IList<Checkpoint> checkpoints)
        {
            return checkpoints.Count > 0 ? checkpoints[checkpoints.Count - 1] : null;
        }

        private int GetWinRate(int winnings, int playedTime)
        {
            if (playedTime > 0)
                return (int)Math.Round((double)winnings / playedTime * 60);
            return 0;
        }

        private static DateTime? GetBuyinTime(IEnumerable<Checkpoint> checkpoints)
        {
            var checkpoint = GetFirstBuyinCheckpoint(checkpoints);
            if (checkpoint == null)
                return null;
            return checkpoint.Timestamp;
        }

        private static Checkpoint GetFirstBuyinCheckpoint(IEnumerable<Checkpoint> checkpoints)
        {
            return GetCheckpointOfType(checkpoints, CheckpointType.Buyin);
        }

        private static Checkpoint GetCashoutCheckpoint(IEnumerable<Checkpoint> checkpoints)
        {
            return GetCheckpointOfType(checkpoints, CheckpointType.Cashout);
        }

        private static Checkpoint GetCheckpointOfType(IEnumerable<Checkpoint> checkpoints, CheckpointType type)
        {
            return checkpoints.FirstOrDefault(checkpoint => checkpoint.Type == type);
        }

        private int GetPlayedTime(DateTime? startTime = null, DateTime? endTime = null)
        {
            if (!startTime.HasValue || !endTime.HasValue)
                return 0;
            var timespan = endTime - startTime;
            return (int)Math.Round(timespan.Value.TotalMinutes);
        }

        private DateTime GetLastReportTime(IList<Checkpoint> checkpoints)
        {
            var checkpoint = GetLastCheckpoint(checkpoints);
            return checkpoint.Timestamp;
        }
	}
}