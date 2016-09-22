using Core.UseCases;
using Plumbing;

namespace Web.Common
{
    public class UseCaseContainer
    {
        private readonly Dependencies _deps;

        public UseCaseContainer(Dependencies deps)
        {
            _deps = deps;
        }

        // Contexts

        // Auth and Home
        public Login Login => new Login(_deps.UserService);

        // Admin

        // User

        // Bunch
        public BunchList BunchList => new BunchList(_deps.BunchService, _deps.UserService);
        public BunchDetails BunchDetails => new BunchDetails(_deps.BunchService, _deps.UserService, _deps.PlayerService);

        // Events

        // Locations
        
        // Cashgame
        public TopList TopList => new TopList(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService);
        public RunningCashgame RunningCashgame => new RunningCashgame(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService, _deps.LocationService);
        public Buyin Buyin => new Buyin(_deps.BunchService, _deps.PlayerService, _deps.CashgameService, _deps.UserService);
        public Report Report => new Report(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService);
        public Cashout Cashout => new Cashout(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService);

        // Player
        public PlayerList PlayerList => new PlayerList(_deps.BunchService, _deps.UserService, _deps.PlayerService);
        public PlayerDetails PlayerDetails => new PlayerDetails(_deps.BunchService, _deps.PlayerService, _deps.CashgameService, _deps.UserService);

        // Apps
        public VerifyAppKey VerifyAppKey => new VerifyAppKey(_deps.AppService);
    }
}