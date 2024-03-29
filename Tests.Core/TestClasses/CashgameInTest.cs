using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;

namespace Tests.Core.TestClasses;

public class CashgameInTest : Cashgame
{
    public CashgameInTest(
        string bunchId = "",
        string locationId = "",
        string eventId = "",
        GameStatus status = GameStatus.Finished,
        string id = "", 
        IList<Checkpoint>? checkpoints = null)
        : base(
            bunchId, 
            locationId, 
            eventId,
            status, 
            id,
            checkpoints ?? new List<Checkpoint>())
    {
    }
}