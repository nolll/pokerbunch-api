using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;

namespace Tests.Core.TestClasses;

public class CashgameInTest(
    string bunchId = "",
    string locationId = "",
    string eventId = "",
    GameStatus status = GameStatus.Finished,
    string id = "",
    IList<Checkpoint>? checkpoints = null)
    : Cashgame(bunchId,
        locationId,
        eventId,
        status,
        id,
        checkpoints ?? new List<Checkpoint>());