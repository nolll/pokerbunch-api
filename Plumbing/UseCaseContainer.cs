using Core.Repositories;
using Core.Services;
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

        private ICacheContainer Cache => _deps.Cache;
        private IMessageSender MessageSender => _deps.MessageSender;
        private IAppRepository AppRepository => _deps.AppRepository;
        private IBunchRepository BunchRepository => _deps.BunchRepository;
        private ICashgameRepository CashgameRepository => _deps.CashgameRepository;
        private IEventRepository EventRepository => _deps.EventRepository;
        private ILocationRepository LocationRepository => _deps.LocationRepository;
        private IPlayerRepository PlayerRepository => _deps.PlayerRepository;
        private IUserRepository UserRepository => _deps.UserRepository;

        // Auth and Home
        public Login Login => new Login(UserRepository);

        // Admin
        public TestEmail TestEmail => new TestEmail(MessageSender, UserRepository);
        public ClearCache ClearCache => new ClearCache(Cache, UserRepository);

        // User
        public UserDetails UserDetails => new UserDetails(UserRepository);
        public UserList UserList => new UserList(UserRepository);
        public EditUser EditUser => new EditUser(UserRepository);

        // Bunch
        public GetBunchList GetBunchList => new GetBunchList(BunchRepository, UserRepository);
        public GetBunch GetBunch => new GetBunch(BunchRepository, UserRepository, PlayerRepository);
        public AddBunch AddBunch => new AddBunch(UserRepository, BunchRepository, PlayerRepository);
        public EditBunch EditBunch => new EditBunch(BunchRepository, UserRepository, PlayerRepository);
        
        // Events
        public EventDetails GetEvent => new EventDetails(EventRepository, UserRepository, PlayerRepository, BunchRepository, LocationRepository);
        public EventList GetEventList => new EventList(BunchRepository, EventRepository, UserRepository, PlayerRepository, LocationRepository);
        public AddEvent AddEvent => new AddEvent(BunchRepository, PlayerRepository, UserRepository, EventRepository);

        // Locations
        public GetLocationList GetLocationList => new GetLocationList(BunchRepository, UserRepository, PlayerRepository, LocationRepository);
        public GetLocation GetLocation => new GetLocation(LocationRepository, UserRepository, PlayerRepository, BunchRepository);
        public AddLocation AddLocation => new AddLocation(BunchRepository, PlayerRepository, UserRepository, LocationRepository);

        // Cashgame
        public CashgameList CashgameList => new CashgameList(BunchRepository, CashgameRepository, UserRepository, PlayerRepository, LocationRepository);
        public CashgameYearList CashgameYearList => new CashgameYearList(BunchRepository, CashgameRepository, UserRepository, PlayerRepository);
        public EventCashgameList EventCashgameList => new EventCashgameList(BunchRepository, CashgameRepository, UserRepository, PlayerRepository, LocationRepository, EventRepository);
        public PlayerCashgameList PlayerCashgameList => new PlayerCashgameList(BunchRepository, CashgameRepository, UserRepository, PlayerRepository, LocationRepository);
        public CurrentCashgames CurrentCashgames => new CurrentCashgames(UserRepository, BunchRepository, CashgameRepository, PlayerRepository);
        public CashgameDetails CashgameDetails => new CashgameDetails(BunchRepository, CashgameRepository, PlayerRepository, UserRepository, LocationRepository, EventRepository);
        public Buyin Buyin => new Buyin(CashgameRepository, PlayerRepository, UserRepository);
        public Report Report => new Report(CashgameRepository, PlayerRepository, UserRepository);
        public Cashout Cashout => new Cashout(CashgameRepository, PlayerRepository, UserRepository);
        public EndCashgame EndCashgame => new EndCashgame(CashgameRepository, PlayerRepository, UserRepository);
        public AddCashgame AddCashgame => new AddCashgame(BunchRepository, CashgameRepository, UserRepository, PlayerRepository, LocationRepository, EventRepository);
        public EditCashgame EditCashgame => new EditCashgame(CashgameRepository, UserRepository, PlayerRepository, LocationRepository, EventRepository);
        public DeleteCashgame DeleteCashgame => new DeleteCashgame(CashgameRepository, BunchRepository, UserRepository, PlayerRepository);
        public EditCheckpoint EditCheckpoint => new EditCheckpoint(BunchRepository, UserRepository, PlayerRepository, CashgameRepository);
        public DeleteCheckpoint DeleteCheckpoint => new DeleteCheckpoint(BunchRepository, CashgameRepository, UserRepository, PlayerRepository);

        // Player
        public GetPlayer GetPlayer => new GetPlayer(BunchRepository, PlayerRepository, CashgameRepository, UserRepository);
        public GetPlayerList GetPlayerList => new GetPlayerList(BunchRepository, UserRepository, PlayerRepository);
        public AddPlayer AddPlayer => new AddPlayer(BunchRepository, PlayerRepository, UserRepository);
        public DeletePlayer DeletePlayer => new DeletePlayer(PlayerRepository, CashgameRepository, UserRepository, BunchRepository);
        public InvitePlayer InvitePlayer => new InvitePlayer(BunchRepository, PlayerRepository, MessageSender, UserRepository);
        public JoinBunch JoinBunch => new JoinBunch(BunchRepository, PlayerRepository, UserRepository);

        // Apps
        public VerifyAppKey VerifyAppKey => new VerifyAppKey(AppRepository);
        public GetApp GetApp => new GetApp(AppRepository, UserRepository);
        public AppList GetAppList => new AppList(AppRepository, UserRepository);
        public AddApp AddApp => new AddApp(AppRepository, UserRepository);
        public DeleteApp DeleteApp => new DeleteApp(AppRepository, UserRepository);
    }
}