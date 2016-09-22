using System.Collections.Generic;
using Core.Entities;

namespace Core.Services
{
    public interface ICashgameService
    {
        IList<Cashgame> GetFinished(int bunchId, int? year = null);
        IList<Cashgame> GetByEvent(int eventId);
        Cashgame GetRunning(int bunchId);
        Cashgame GetByCheckpoint(int checkpointId);
        Cashgame GetById(int cashgameId);
        IList<int> GetYears(int bunchId);
        void DeleteGame(int id);
        int AddGame(Bunch bunch, Cashgame cashgame);
        void UpdateGame(Cashgame cashgame);
        void EndGame(Cashgame cashgame);
        bool HasPlayed(int playerId);
    }
}