using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EndCashgame
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;

        public EndCashgame(BunchService bunchService, CashgameService cashgameService, IUserRepository userRepository, PlayerService playerService)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public void Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var cashgame = _cashgameService.GetRunning(bunch.Id);

            if (cashgame != null)
                _cashgameService.EndGame(cashgame);
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
    }
}
