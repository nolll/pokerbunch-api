using Core.Cache;
using Core.Repositories;
using Core.Services;
using Infrastructure.Cache;
using Infrastructure.Email;
using Infrastructure.Sql;
using Infrastructure.Sql.Repositories;

namespace Plumbing
{
    public class Dependencies
    {
        private readonly string _connectionString;
        private readonly string _smtpHost;
        private readonly bool _useSendGrid;
        private readonly string _sendGridApiKey;

        private ICacheContainer _cache;
        private SqlServerStorageProvider _db;

        private IAppRepository _appRepository;
        private IBunchRepository _bunchRepository;
        private ICashgameRepository _cashgameRepository;
        private IEventRepository _eventRepository;
        private IPlayerRepository _playerRepository;
        private ILocationRepository _locationRepository;
        private IUserRepository _userRepository;
        private IRandomizer _randomizer;
        private IEmailSender _emailSender;
                
        public Dependencies(string connectionString, string smtpHost, bool useSendGrid, string sendGridApiKey)
        {
            _connectionString = connectionString;
            _smtpHost = smtpHost;
            _useSendGrid = useSendGrid;
            _sendGridApiKey = sendGridApiKey;
        }

        private SqlServerStorageProvider Db => _db ?? (_db = new SqlServerStorageProvider(_connectionString));
        public ICacheContainer Cache => _cache ?? (_cache = new CacheContainer(new AspNetCacheProvider()));
        public IAppRepository AppRepository => _appRepository ?? (_appRepository = new AppRepository(Db, Cache));
        public IBunchRepository BunchRepository => _bunchRepository ?? (_bunchRepository = new BunchRepository(Db, Cache));
        public ICashgameRepository CashgameRepository => _cashgameRepository ?? (_cashgameRepository = new CashgameRepository(Db, Cache));
        public IEventRepository EventRepository => _eventRepository ?? (_eventRepository = new EventRepository(Db, Cache));
        public IPlayerRepository PlayerRepository => _playerRepository ?? (_playerRepository = new PlayerRepository(Db, Cache));
        public ILocationRepository LocationRepository => _locationRepository ?? (_locationRepository = new LocationRepository(Db, Cache));
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(Db, Cache));
        public IRandomizer Randomizer => _randomizer ?? (_randomizer = new Randomizer());
        public IEmailSender EmailSender => _emailSender ?? (_emailSender = CreateEmailSender());

        private IEmailSender CreateEmailSender()
        {
            if (_useSendGrid)
                return new SendGridEmailSender(_sendGridApiKey);
            return new SmtpEmailSender(_smtpHost);
        }
    }
}