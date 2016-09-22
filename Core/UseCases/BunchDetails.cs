using Core.Entities;
using Core.Services;

namespace Core.UseCases
{
    public class BunchDetails
    {
        private readonly IBunchService _bunchService;
        private readonly IUserService _userService;
        private readonly IPlayerService _playerService;

        public BunchDetails(IBunchService bunchService, IUserService userService, IPlayerService playerService)
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

            var id = bunch.Id;
            var slug = bunch.Slug;
            var bunchName = bunch.DisplayName;
            var description = bunch.Description;
            var houseRules = bunch.HouseRules;
            var canEdit = RoleHandler.IsInRole(user, player, Role.Manager);

            return new Result(id, slug, bunchName, description, houseRules, canEdit);
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
            public int Id { get; private set; }
            public string Slug { get; private set; }
            public string BunchName { get; private set; }
            public string Description { get; private set; }
            public string HouseRules { get; private set; }
            public bool CanEdit { get; private set; }

            public Result(int id, string slug, string bunchName, string description, string houseRules, bool canEdit)
            {
                Id = id;
                Slug = slug;
                BunchName = bunchName;
                Description = description;
                HouseRules = houseRules;
                CanEdit = canEdit;
            }
        }
    }
}