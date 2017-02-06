using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;

namespace Tests.Core.TestClasses
{
    public class CashgameInTest : Cashgame
    {
        public CashgameInTest(
            int bunchId = 0, 
            int locationId = 0, 
            int eventId = 0,
            GameStatus status = GameStatus.Finished, 
            int? id = null, 
            IList<Checkpoint> checkpoints = null)
            : base(
                bunchId, 
                locationId, 
                eventId,
                status, 
                id, 
                checkpoints)
        {
        }
    }
}