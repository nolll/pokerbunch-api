using Core.Services;

namespace Core.UseCases
{
    public class InvitePlayerConfirmation
    {
        private readonly BunchService _bunchService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public InvitePlayerConfirmation(BunchService bunchService, PlayerService playerService, UserService userService)
        {
            _bunchService = bunchService;
            _playerService = playerService;
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var player = _playerService.Get(request.PlayerId);
            var bunch = _bunchService.Get(player.BunchId);
            var currentUser = _userService.GetByNameOrEmail(request.UserName);
            var currentPlayer = _playerService.GetByUserId(bunch.Id, currentUser.Id);
            RequireRole.Manager(currentUser, currentPlayer);

            return new Result(bunch.Slug);
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
            public string Slug { get; private set; }

            public Result(string slug)
            {
                Slug = slug;
            }
        }
    }
}