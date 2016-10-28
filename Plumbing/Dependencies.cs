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
        private ICashgameRepository _cashgameRepository;
        private IEventRepository _eventRepository;
        private IPlayerRepository _playerRepository;
        private ILocationRepository _locationRepository;
        private IUserRepository _userRepository;
        private IRandomizer _randomizer;
        private IMessageSender _messageSender;
                
        public Dependencies(string connectionString)
        {
            _cacheContainer = new CacheContainer(new AspNetCacheProvider());
            _db = new SqlServerStorageProvider(connectionString);
        }

        public IAppRepository AppRepository => _appRepository ?? (_appRepository = new AppRepository(_db, _cacheContainer));
        public IBunchRepository BunchRepository => _bunchRepository ?? (_bunchRepository = new BunchRepository(_db, _cacheContainer));
        public ICashgameRepository CashgameRepository => _cashgameRepository ?? (_cashgameRepository = new CashgameRepository(_db, _cacheContainer));
        public IEventRepository EventRepository => _eventRepository ?? (_eventRepository = new EventRepository(_db, _cacheContainer));
        public IPlayerRepository PlayerRepository => _playerRepository ?? (_playerRepository = new PlayerRepository(_db, _cacheContainer));
        public ILocationRepository LocationRepository => _locationRepository ?? (_locationRepository = new LocationRepository(_db, _cacheContainer));
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_db, _cacheContainer));
        public IRandomizer Randomizer => _randomizer ?? (_randomizer = new Randomizer());
        public IMessageSender MessageSender => _messageSender ?? (_messageSender = new EmailMessageSender());
    }
}