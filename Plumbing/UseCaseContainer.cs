using Core.Repositories;
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
        public TestEmail TestEmail => new TestEmail(_deps.MessageSender, _deps.UserRepository);
        public ClearCache ClearCache => new ClearCache(_deps.Cache, _deps.UserRepository);

        // User
        public UserDetails UserDetails => new UserDetails(_deps.UserRepository);
        public UserList UserList => new UserList(_deps.UserRepository);
        public EditUser EditUser => new EditUser(_deps.UserRepository);

        // Bunch
        public GetBunchList GetBunchList => new GetBunchList(_deps.BunchRepository, _deps.UserRepository);
        public GetBunch GetBunch => new GetBunch(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerRepository);
        public AddBunch AddBunch => new AddBunch(_deps.UserRepository, _deps.BunchRepository, _deps.PlayerRepository);
        public EditBunch EditBunch => new EditBunch(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerRepository);
        
        // Events
        public EventDetails GetEvent => new EventDetails(_deps.EventRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.BunchRepository, _deps.LocationRepository);
        public EventList GetEventList => new EventList(_deps.BunchRepository, _deps.EventRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.LocationRepository);
        public AddEvent AddEvent => new AddEvent(_deps.BunchRepository, _deps.PlayerRepository, _deps.UserRepository, _deps.EventRepository);

        // Locations
        public GetLocationList GetLocationList => new GetLocationList(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.LocationRepository);
        public GetLocation GetLocation => new GetLocation(_deps.LocationRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.BunchRepository);
        public AddLocation AddLocation => new AddLocation(_deps.BunchRepository, _deps.PlayerRepository, _deps.UserRepository, _deps.LocationRepository);

        // Cashgame
        public CashgameList CashgameList => new CashgameList(_deps.BunchRepository, _deps.CashgameRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.LocationRepository);
        public CashgameYearList CashgameYearList => new CashgameYearList(_deps.BunchRepository, _deps.CashgameRepository, _deps.UserRepository, _deps.PlayerRepository);
        public EventCashgameList EventCashgameList => new EventCashgameList(_deps.BunchRepository, _deps.CashgameRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.LocationRepository, _deps.EventRepository);
        public PlayerCashgameList PlayerCashgameList => new PlayerCashgameList(_deps.BunchRepository, _deps.CashgameRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.LocationRepository);
        public CurrentCashgames CurrentCashgames => new CurrentCashgames(_deps.UserRepository, _deps.BunchRepository, _deps.CashgameRepository, _deps.PlayerRepository);
        public CashgameDetails CashgameDetails => new CashgameDetails(_deps.BunchRepository, _deps.CashgameRepository, _deps.PlayerRepository, _deps.UserRepository, _deps.LocationRepository, _deps.EventRepository);
        public Buyin Buyin => new Buyin(_deps.CashgameRepository, _deps.PlayerRepository, _deps.UserRepository);
        public Report Report => new Report(_deps.CashgameRepository, _deps.PlayerRepository, _deps.UserRepository);
        public Cashout Cashout => new Cashout(_deps.CashgameRepository, _deps.PlayerRepository, _deps.UserRepository);
        public EndCashgame EndCashgame => new EndCashgame(_deps.CashgameRepository, _deps.PlayerRepository, _deps.UserRepository);
        public AddCashgame AddCashgame => new AddCashgame(_deps.BunchRepository, _deps.CashgameRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.LocationRepository, _deps.EventRepository);
        public EditCashgame EditCashgame => new EditCashgame(_deps.CashgameRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.LocationRepository, _deps.EventRepository);
        public DeleteCashgame DeleteCashgame => new DeleteCashgame(_deps.CashgameRepository, _deps.BunchRepository, _deps.UserRepository, _deps.PlayerRepository);
        public EditCheckpoint EditCheckpoint => new EditCheckpoint(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerRepository, _deps.CashgameRepository);

        // Player
        public GetPlayer GetPlayer => new GetPlayer(_deps.BunchRepository, _deps.PlayerRepository, _deps.CashgameRepository, _deps.UserRepository);
        public GetPlayerList GetPlayerList => new GetPlayerList(_deps.BunchRepository, _deps.UserRepository, _deps.PlayerRepository);
        public AddPlayer AddPlayer => new AddPlayer(_deps.BunchRepository, _deps.PlayerRepository, _deps.UserRepository);
        public DeletePlayer DeletePlayer => new DeletePlayer(_deps.PlayerRepository, _deps.CashgameRepository, _deps.UserRepository, _deps.BunchRepository);
        public InvitePlayer InvitePlayer => new InvitePlayer(_deps.BunchRepository, _deps.PlayerRepository, _deps.MessageSender, _deps.UserRepository);
        public JoinBunch JoinBunch => new JoinBunch(_deps.BunchRepository, _deps.PlayerRepository, _deps.UserRepository);

        // Apps
        public VerifyAppKey VerifyAppKey => new VerifyAppKey(_deps.AppRepository);
        public GetApp GetApp => new GetApp(_deps.AppRepository, _deps.UserRepository);
        public AppList GetAppList => new AppList(_deps.AppRepository, _deps.UserRepository);
        public AddApp AddApp => new AddApp(_deps.AppRepository, _deps.UserRepository);
    }
}