using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EndCashgame
    {
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public EndCashgame(ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            _cashgameRepository = cashgameRepository;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        public void Execute(Request request)
        {
            var cashgame = _cashgameRepository.Get(request.CashgameId);
            if (cashgame != null)
            {
                var user = _userRepository.Get(request.UserName);
                var player = _playerRepository.Get(cashgame.BunchId, user.Id);
                RequireRole.Player(user, player);
            
                cashgame.ChangeStatus(GameStatus.Finished);
                _cashgameRepository.Update(cashgame);
            }
        }

        public class Request
        {
            public string UserName { get; }
            public int CashgameId { get; }

            public Request(string userName, int cashgameId)
            {
                UserName = userName;
                CashgameId = cashgameId;
            }
        }
    }
}
