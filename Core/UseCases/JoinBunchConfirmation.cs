using Core.Services;

namespace Core.UseCases
{
    public class JoinBunchConfirmation
    {
        private readonly BunchService _bunchService;
        private readonly UserService _userService;
        private readonly PlayerService _playerService;

        public JoinBunchConfirmation(BunchService bunchService, UserService userService, PlayerService playerService)
        {
            _bunchService = bunchService;
            _userService = userService;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var bunchName = bunch.DisplayName;

            return new Result(bunchName, bunch.Slug);
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
            public string BunchName { get; private set; }
            public string Slug { get; private set; }

            public Result(string bunchName, string slug)
            {
                Slug = slug;
                BunchName = bunchName;
            }
        }
    }
}