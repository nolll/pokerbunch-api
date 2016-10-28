using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class DeletePlayer
    {
        private readonly PlayerService _playerService;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;
        private readonly BunchService _bunchService;

        public DeletePlayer(PlayerService playerService, CashgameService cashgameService, IUserRepository userRepository, BunchService bunchService)
        {
            _playerService = playerService;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
            _bunchService = bunchService;
        }

        public Result Execute(Request request)
        {
            var player = _playerService.Get(request.PlayerId);
            var bunch = _bunchService.Get(player.BunchId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.GetByUserId(bunch.Id, currentUser.Id);
            RequireRole.Manager(currentUser, currentPlayer);
            var canDelete = !_cashgameService.HasPlayed(player.Id);

            if (canDelete)
            {
                _playerService.Delete(request.PlayerId);
            }

            return new Result(canDelete, bunch.Slug, request.PlayerId);
        }

        public class Request
        {
            public string UserName { get; }
            public int PlayerId { get; }

            public Request(string userName, int playerId)
            {
                UserName = userName;
                PlayerId = playerId;
            }
        }

        public class Result
        {
            public bool Deleted { get; private set; }
            public string Slug { get; private set; }
            public int PlayerId { get; private set; }

            public Result(bool deleted, string slug, int playerId)
            {
                Deleted = deleted;
                Slug = slug;
                PlayerId = playerId;
            }
        }
    }
}