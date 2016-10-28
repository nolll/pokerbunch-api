using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameStatus
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;

        public CashgameStatus(BunchService bunchService, CashgameService cashgameService, IUserRepository userRepository, PlayerService playerService)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var runningGame = _cashgameService.GetRunning(bunch.Id);

            var gameIsRunning = runningGame != null;

            return new Result(
                request.Slug,
                gameIsRunning);
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }

            public Request(string userName, string slug)
            {
                UserName = userName;
                Slug = slug;
            }
        }

        public class Result
        {
            public string Slug { get; private set; }
            public bool GameIsRunning { get; private set; }

            public Result(
                string slug,
                bool gameIsRunning)
            {
                Slug = slug;
                GameIsRunning = gameIsRunning;
            }
        }
    }
}