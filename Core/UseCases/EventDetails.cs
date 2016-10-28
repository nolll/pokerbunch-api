using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EventDetails
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;
        private readonly BunchService _bunchService;

        public EventDetails(IEventRepository eventRepository, IUserRepository userRepository, PlayerService playerService, BunchService bunchService)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _playerService = playerService;
            _bunchService = bunchService;
        }

        public Result Execute(Request request)
        {
            var e = _eventRepository.Get(request.EventId);
            var bunch = _bunchService.Get(e.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(e.BunchId, user.Id);
            RequireRole.Player(user, player);
            
            return new Result(e.Name, bunch.Slug);
        }

        public class Request
        {
            public string UserName { get; }
            public int EventId { get; }

            public Request(string userName, int eventId)
            {
                UserName = userName;
                EventId = eventId;
            }
        }

        public class Result
        {
            public string Name { get; private set; }
            public string Slug { get; private set; }

            public Result(string name, string slug)
            {
                Name = name;
                Slug = slug;
            }
        }
    }
}