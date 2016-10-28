using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class DeletePlayer
    {
        private readonly PlayerService _playerService;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;
        private readonly IBunchRepository _bunchRepository;

        public DeletePlayer(PlayerService playerService, CashgameService cashgameService, IUserRepository userRepository, IBunchRepository bunchRepository)
        {
            _playerService = playerService;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
            _bunchRepository = bunchRepository;
        }

        public Result Execute(Request request)
        {
            var player = _playerService.Get(request.PlayerId);
            var bunch = _bunchRepository.Get(player.BunchId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.Get(bunch.Id, currentUser.Id);
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