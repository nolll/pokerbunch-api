using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories
{
	public interface ICashgameRepository
    {
        Cashgame Get(int cashgameId);
	    IList<Cashgame> Get(IList<int> ids);

        IList<int> FindFinished(int bunchId, int? year = null);
        IList<int> FindByEvent(int eventId);
        IList<int> FindByPlayerId(int playerId);
        IList<int> FindRunning(int bunchId);
        IList<int> FindByCheckpoint(int checkpointId);
        
        void DeleteGame(int id);
		int AddGame(Bunch bunch, Cashgame cashgame);
		void UpdateGame(Cashgame cashgame);

        IList<int> GetYears(int bunchId);
	}
}