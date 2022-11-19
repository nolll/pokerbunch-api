using Core.Entities;

namespace Core.Repositories;

public interface ICashgameRepository
{
    Task<Cashgame> Get(int cashgameId);
    Task<IList<Cashgame>> GetFinished(int bunchId, int? year = null);
    Task<IList<Cashgame>> GetByEvent(int eventId);
    Task<IList<Cashgame>> GetByPlayer(int playerId);
    Task<Cashgame> GetRunning(int bunchId);
    Task<Cashgame> GetByCheckpoint(int checkpointId);
    Task DeleteGame(int id);
    Task<int> Add(Bunch bunch, Cashgame cashgame);
    Task Update(Cashgame cashgame);
    Task<IList<int>> GetYears(int bunchId);
}