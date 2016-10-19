using Core.Cache;
using Core.Services;
using Infrastructure.Cache;
using Infrastructure.Email;
using Infrastructure.Sql;
using Infrastructure.Sql.CachedRepositories;
using Infrastructure.Sql.Repositories;

namespace Plumbing
{
    public class Dependencies
    {
        private readonly ICacheContainer _cacheContainer;
        private readonly SqlServerStorageProvider _db;
        private AppService _appService;
        private BunchService _bunchService;
        private CashgameService _cashgameService;
        private EventService _eventService;
        private PlayerService _playerService;
        private LocationService _locationService;
        private UserService _userService;
        private IRandomService _randomService;
        private IMessageSender _messageSender;
                
        public Dependencies(string connectionString)
        {
            _cacheContainer = new CacheContainer(new AspNetCacheProvider());
            _db = new SqlServerStorageProvider(connectionString);
        }

        public AppService AppService => _appService ?? (_appService = new AppService(new CachedAppRepository(new SqlAppRepository(_db), _cacheContainer)));
        public BunchService BunchService => _bunchService ?? (_bunchService = new BunchService(new CachedBunchRepository(new SqlBunchRepository(_db), _cacheContainer)));
        public CashgameService CashgameService => _cashgameService ?? (_cashgameService = new CashgameService(new CachedCashgameRepository(new SqlCashgameRepository(_db), _cacheContainer)));
        public EventService EventService => _eventService ?? (_eventService = new EventService(new CachedEventRepository(new SqlEventRepository(_db), _cacheContainer)));
        public PlayerService PlayerService => _playerService ?? (_playerService = new PlayerService(new CachedPlayerRepository(new SqlPlayerRepository(_db), _cacheContainer)));
        public LocationService LocationService => _locationService ?? (_locationService = new LocationService(new CachedLocationRepository(new SqlLocationRepository(_db), _cacheContainer)));
        public UserService UserService => _userService ?? (_userService = new UserService(new CachedUserRepository(new SqlUserRepository(_db), _cacheContainer)));
        public IRandomService RandomService => _randomService ?? (_randomService = new RandomService());
        public IMessageSender MessageSender => _messageSender ?? (_messageSender = new EmailMessageSender());
    }
}