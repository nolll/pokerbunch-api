using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class DeleteCheckpoint
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;

        public DeleteCheckpoint(BunchService bunchService, CashgameService cashgameService, IUserRepository userRepository, PlayerService playerService)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var cashgame = _cashgameService.GetByCheckpoint(request.CheckpointId);
            var checkpoint = cashgame.GetCheckpoint(request.CheckpointId);
            var bunch = _bunchService.Get(cashgame.BunchId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.GetByUserId(cashgame.BunchId, currentUser.Id);
            RequireRole.Manager(currentUser, currentPlayer);
            cashgame.DeleteCheckpoint(checkpoint);
            _cashgameService.UpdateGame(cashgame);

            var gameIsRunning = cashgame.Status == GameStatus.Running;
            return new Result(bunch.Slug, gameIsRunning, cashgame.Id);
        }

        public class Request
        {
            public string UserName { get; }
            public int CheckpointId { get; }

            public Request(string userName, int checkpointId)
            {
                UserName = userName;
                CheckpointId = checkpointId;
            }
        }

        public class Result
        {
            public string Slug { get; private set; }
            public bool GameIsRunning { get; private set; }
            public int CashgameId { get; private set; }

            public Result(string slug, bool gameIsRunning, int cashgameId)
            {
                Slug = slug;
                GameIsRunning = gameIsRunning;
                CashgameId = cashgameId;
            }
        }
    }
}
