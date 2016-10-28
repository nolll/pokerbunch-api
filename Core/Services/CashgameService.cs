using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class CashgameService : ICashgameService
    {
        private readonly ICashgameRepository _cashgameRepository;

        public CashgameService(ICashgameRepository cashgameRepository)
        {
            _cashgameRepository = cashgameRepository;
        }

        public IList<Cashgame> GetFinished(int bunchId, int? year = null)
        {
            return _cashgameRepository.GetFinished(bunchId, year);
        }

        public IList<Cashgame> GetByEvent(int eventId)
        {
            return _cashgameRepository.GetByEvent(eventId);
        }

        public Cashgame GetRunning(int bunchId)
        {
            return _cashgameRepository.GetRunning(bunchId);
        }

        public Cashgame GetByCheckpoint(int checkpointId)
        {
            return _cashgameRepository.GetByCheckpoint(checkpointId);
        }

        public IList<Cashgame> GetByPlayer(int playerId)
        {
            return _cashgameRepository.GetByPlayer(playerId);
        }

        public Cashgame Get(int cashgameId)
        {
            return _cashgameRepository.Get(cashgameId);
        }

        public IList<int> GetYears(int bunchId)
        {
            return _cashgameRepository.GetYears(bunchId);
        }

        public void DeleteGame(int id)
        {
            _cashgameRepository.DeleteGame(id);
        }

        public int Add(Bunch bunch, Cashgame cashgame)
        {
            return _cashgameRepository.Add(bunch, cashgame);
        }

        public void Update(Cashgame cashgame)
        {
            _cashgameRepository.Update(cashgame);
        }
    }

    public class CashgameServiceTemp
    {
        public static bool SpansMultipleYears(IEnumerable<Cashgame> cashgames)
        {
            var years = new List<int>();
            foreach (var cashgame in cashgames)
            {
                if (cashgame.StartTime.HasValue)
                {
                    var year = cashgame.StartTime.Value.Year;
                    if (!years.Contains(year))
                    {
                        years.Add(year);
                    }
                }
            }
            return years.Count > 1;
        }
    }
}