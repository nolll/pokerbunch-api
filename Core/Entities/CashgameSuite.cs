using System.Collections.Generic;
using System.Linq;
using Core.Services;

namespace Core.Entities
{
    public class CashgameSuite
    {
	    public IList<Cashgame> Cashgames { get; }
        public IList<CashgameTotalResult> TotalResults { get; }
        public bool SpansMultipleYears => CashgameServiceTemp.SpansMultipleYears(Cashgames);

        public CashgameSuite(IList<Cashgame> cashgames, IEnumerable<Player> players)
        {
            var sortedCashgames = cashgames.OrderByDescending(o => o.StartTime).ToList();
            var totalResults = CreateTotalResults(players, cashgames);

            Cashgames = sortedCashgames;
            TotalResults = totalResults;
        }

        private static IList<CashgameTotalResult> CreateTotalResults(IEnumerable<Player> players, IEnumerable<Cashgame> cashgames)
        {
            return players.Select(player => new CashgameTotalResult(player, cashgames)).Where(o => o.GameCount > 0).OrderByDescending(o => o.Winnings).ToList();
        }
    }
}