using Core.Cache;
using Core.Repositories;
using Core.Services;
using Infrastructure.Cache;
using Infrastructure.Email;
using Infrastructure.Sql;
using Infrastructure.Sql.CachedRepositories;

namespace Plumbing
{
    public class Dependencies
    {
        private readonly ICacheContainer _cacheContainer;
        private readonly SqlServerStorageProvider _db;
        private IAppRepository _appRepository;
        private IBunchRepository _bunchRepository;
        private CashgameService _cashgameService;
        private IEventRepository _eventRepository;
        private PlayerService _playerService;
        private ILocationRepository _locationRepository;
        private IUserRepository _userRepository;
        private IRandomService _randomService;
        private IMessageSender _messageSender;
                
        public Dependencies(string connectionString)
        {
            _cacheContainer = new CacheContainer(new AspNetCacheProvider());
            _db = new SqlServerStorageProvider(connectionString);
        }

        public IAppRepository AppRepository => _appRepository ?? (_appRepository = new AppRepository(_db, _cacheContainer));
        public IBunchRepository BunchRepository => _bunchRepository ?? (_bunchRepository = new BunchRepository(_db, _cacheContainer));
        public CashgameService CashgameService => _cashgameService ?? (_cashgameService = new CashgameService(new CashgameRepository(_db, _cacheContainer)));
        public IEventRepository EventRepository => _eventRepository ?? (_eventRepository = new EventRepository(_db, _cacheContainer));
        public PlayerService PlayerService => _playerService ?? (_playerService = new PlayerService(new PlayerRepository(_db, _cacheContainer)));
        public ILocationRepository LocationRepository => _locationRepository ?? (_locationRepository = new LocationRepository(_db, _cacheContainer));
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_db, _cacheContainer));
        public IRandomService RandomService => _randomService ?? (_randomService = new RandomService());
        public IMessageSender MessageSender => _messageSender ?? (_messageSender = new EmailMessageSender());
    }
}