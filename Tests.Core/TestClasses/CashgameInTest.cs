using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;

namespace Tests.Core.TestClasses;

public class CashgameInTest : Cashgame
{
    public CashgameInTest(
        string bunchId = null,
        string locationId = null,
        string eventId = null,
        GameStatus status = GameStatus.Finished,
        string id = null, 
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