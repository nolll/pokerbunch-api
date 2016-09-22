using Core.Services;

namespace Core.UseCases
{
    public class EventDetails
    {
        private readonly EventService _eventService;
        private readonly UserService _userService;
        private readonly PlayerService _playerService;
        private readonly BunchService _bunchService;

        public EventDetails(EventService eventService, UserService userService, PlayerService playerService, BunchService bunchService)
        {
            _eventService = eventService;
            _userService = userService;
            _playerService = playerService;
            _bunchService = bunchService;
        }

        public Result Execute(Request request)
        {
            var e = _eventService.Get(request.EventId);
            var bunch = _bunchService.Get(e.BunchId);
            var user = _userService.GetByNameOrEmail(request.UserName);
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