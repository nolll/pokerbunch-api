using Core.Entities;

namespace Core.Repositories;

public interface ICashgameRepository
{
    Task<Cashgame> Get(string cashgameId);
    Task<IList<Cashgame>> GetFinished(string bunchId, int? year = null);
    Task<IList<Cashgame>> GetByEvent(string eventId);
    Task<IList<Cashgame>> GetByPlayer(string playerId);
    Task<Cashgame?> GetRunning(string bunchId);
    Task<Cashgame> GetByCheckpoint(string checkpointId);
    Task DeleteGame(string id);
    Task<string> Add(Bunch bunch, Cashgame cashgame);
    Task Update(Cashgame cashgame);
}