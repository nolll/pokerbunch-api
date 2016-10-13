using Core.Entities;
using Core.Services;

namespace Core.UseCases
{
    public class GetBunch
    {
        private readonly IBunchService _bunchService;
        private readonly IUserService _userService;
        private readonly IPlayerService _playerService;

        public GetBunch(IBunchService bunchService, IUserService userService, IPlayerService playerService)
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
            var role = player.Role;

            return new Result(id, slug, bunchName, description, houseRules, role);
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
            public int Id { get; }
            public string Slug { get; }
            public string Name { get; }
            public string Description { get; }
            public string HouseRules { get; }
            public Role Role { get; }

            public Result(int id, string slug, string name, string description, string houseRules, Role role)
            {
                Id = id;
                Slug = slug;
                Name = name;
                Description = description;
                HouseRules = houseRules;
                Role = role;
            }
        }
    }
}