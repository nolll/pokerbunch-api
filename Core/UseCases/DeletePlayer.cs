using System.Linq;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class DeletePlayer
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBunchRepository _bunchRepository;

        public DeletePlayer(IPlayerRepository playerRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IBunchRepository bunchRepository)
        {
            _playerRepository = playerRepository;
            _cashgameRepository = cashgameRepository;
            _userRepository = userRepository;
            _bunchRepository = bunchRepository;
        }

        public Result Execute(Request request)
        {
            var player = _playerRepository.Get(request.PlayerId);
            var bunch = _bunchRepository.Get(player.BunchId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);
            RequireRole.Manager(currentUser, currentPlayer);
            var cashgames = _cashgameRepository.GetByPlayer(player.Id);
            var hasPlayed = cashgames.Any();
            var canDelete = !hasPlayed;

            if (canDelete)
            {
                _playerRepository.Delete(request.PlayerId);
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