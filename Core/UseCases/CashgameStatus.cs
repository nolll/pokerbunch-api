using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameStatus
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerRepository _playerRepository;

        public CashgameStatus(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameRepository = cashgameRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var runningGame = _cashgameRepository.GetRunning(bunch.Id);

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