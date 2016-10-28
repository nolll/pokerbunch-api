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
        public Login Login => new Login(_deps.UserRepository);

        // Admin

        // User
        public UserDetails UserDetails => new UserDetails(_deps.UserRepository);

        // Bunch
        public GetBunchList GetBunchList => new GetBunchList(_deps.BunchRepository, _deps.UserRepository);
        public GetBunch GetBunch => new GetBunch(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerService);
        public AddBunch AddBunch => new AddBunch(_deps.UserRepository, _deps.BunchRepository, _deps.PlayerService);
        public EditBunch EditBunch => new EditBunch(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerService);

        // Events

        // Locations
        public GetLocationList GetLocationList => new GetLocationList(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerService, _deps.LocationRepository);
        public GetLocation GetLocation => new GetLocation(_deps.LocationRepository, _deps.UserRepository, _deps.PlayerService, _deps.BunchRepository);
        public AddLocation AddLocation => new AddLocation(_deps.BunchRepository, _deps.PlayerService, _deps.UserRepository, _deps.LocationRepository);

        // Cashgame
        public TopList TopList => new TopList(_deps.BunchRepository, _deps.CashgameService, _deps.PlayerService, _deps.UserRepository);
        public RunningCashgame RunningCashgame => new RunningCashgame(_deps.BunchRepository, _deps.CashgameService, _deps.PlayerService, _deps.UserRepository, _deps.LocationRepository);
        public Buyin Buyin => new Buyin(_deps.BunchRepository, _deps.PlayerService, _deps.CashgameService, _deps.UserRepository);
        public Report Report => new Report(_deps.BunchRepository, _deps.CashgameService, _deps.PlayerService, _deps.UserRepository);
        public Cashout Cashout => new Cashout(_deps.BunchRepository, _deps.CashgameService, _deps.PlayerService, _deps.UserRepository);

        // Player
        public GetPlayerList GetPlayerList => new GetPlayerList(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerService);
        public GetPlayer GetPlayer => new GetPlayer(_deps.BunchRepository, _deps.PlayerService, _deps.CashgameService, _deps.UserRepository);

        // Apps
        public VerifyAppKey VerifyAppKey => new VerifyAppKey(_deps.AppRepository);
        public GetApp GetApp => new GetApp(_deps.AppRepository);
        public AppList GetAppList => new AppList(_deps.AppRepository, _deps.UserRepository);
        public AddApp AddApp => new AddApp(_deps.AppRepository, _deps.UserRepository);
    }
}