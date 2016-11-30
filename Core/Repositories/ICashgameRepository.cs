using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
	public interface ICashgameRepository
    {
        Cashgame Get(int cashgameId);

        IList<Cashgame> GetFinished(int bunchId, int? year = null);
        IList<Cashgame> GetByEvent(int eventId);
        IList<Cashgame> GetByPlayer(int playerId);
        Cashgame GetRunning(int bunchId);
        Cashgame GetByCheckpoint(int checkpointId);
        
        void DeleteGame(int id);
		int Add(Bunch bunch, Cashgame cashgame);
		void Update(Cashgame cashgame);

        IList<int> GetYears(int bunchId);
	}
}