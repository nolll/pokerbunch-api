using Core.UseCases;

namespace Plumbing
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
        public UserDetails UserDetails => new UserDetails(_deps.UserService);

        // Bunch
        public GetBunchList GetBunchList => new GetBunchList(_deps.BunchService, _deps.UserService);
        public GetBunch GetBunch => new GetBunch(_deps.BunchService, _deps.UserService, _deps.PlayerService);
        public AddBunch AddBunch => new AddBunch(_deps.UserService, _deps.BunchService, _deps.PlayerService);
        public EditBunch EditBunch => new EditBunch(_deps.BunchService, _deps.UserService, _deps.PlayerService);

        // Events

        // Locations
        public GetLocationList GetLocationList => new GetLocationList(_deps.BunchService, _deps.UserService, _deps.PlayerService, _deps.LocationService);
        public GetLocation GetLocation => new GetLocation(_deps.LocationService, _deps.UserService, _deps.PlayerService, _deps.BunchService);
        public AddLocation AddLocation => new AddLocation(_deps.BunchService, _deps.PlayerService, _deps.UserService, _deps.LocationService);

        // Cashgame
        public TopList TopList => new TopList(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService);
        public RunningCashgame RunningCashgame => new RunningCashgame(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService, _deps.LocationService);
        public Buyin Buyin => new Buyin(_deps.BunchService, _deps.PlayerService, _deps.CashgameService, _deps.UserService);
        public Report Report => new Report(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService);
        public Cashout Cashout => new Cashout(_deps.BunchService, _deps.CashgameService, _deps.PlayerService, _deps.UserService);

        // Player
        public GetPlayerList GetPlayerList => new GetPlayerList(_deps.BunchService, _deps.UserService, _deps.PlayerService);
        public GetPlayer GetPlayer => new GetPlayer(_deps.BunchService, _deps.PlayerService, _deps.CashgameService, _deps.UserService);

        // Apps
        public VerifyAppKey VerifyAppKey => new VerifyAppKey(_deps.AppService);
        public GetApp GetApp => new GetApp(_deps.AppService);
        public AppList GetAppList => new AppList(_deps.AppService, _deps.UserService);
        public AddApp AddApp => new AddApp(_deps.AppService, _deps.UserService);
    }
}