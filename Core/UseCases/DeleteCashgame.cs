using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class DeleteCashgame
    {
        private readonly CashgameService _cashgameService;
        private readonly IBunchRepository _bunchRepository;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;

        public DeleteCashgame(CashgameService cashgameService, IBunchRepository bunchRepository, IUserRepository userRepository, PlayerService playerService)
        {
            _cashgameService = cashgameService;
            _bunchRepository = bunchRepository;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var cashgame = _cashgameService.GetById(request.Id);
            var bunch = _bunchRepository.Get(cashgame.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Manager(user, player);

            if (cashgame.PlayerCount > 0)
                throw new CashgameHasResultsException();

            _cashgameService.DeleteGame(cashgame.Id);

            return new Result(bunch.Slug);
        }

        public class Request
        {
            public string UserName { get; }
            public int Id { get; }

            public Request(string userName, int id)
            {
                UserName = userName;
                Id = id;
            }
        }

        public class Result
        {
            public string Slug { get; private set; }

            public Result(string slug)
            {
                Slug = slug;
            }
        }
    }
}
