using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EventDetails
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IBunchRepository _bunchRepository;

        public EventDetails(IEventRepository eventRepository, IUserRepository userRepository, IPlayerRepository playerRepository, IBunchRepository bunchRepository)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _bunchRepository = bunchRepository;
        }

        public Result Execute(Request request)
        {
            var e = _eventRepository.Get(request.EventId);
            var bunch = _bunchRepository.Get(e.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(e.BunchId, user.Id);
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