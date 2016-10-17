using System;
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

            var slug = bunch.Slug;
            var bunchName = bunch.DisplayName;
            var description = bunch.Description;
            var houseRules = bunch.HouseRules;
            var timezone = bunch.Timezone;
            var currency = bunch.Currency;
            var defaultBuyin = bunch.DefaultBuyin;
            var role = player.Role;

            return new Result(slug, bunchName, description, houseRules, timezone, currency, defaultBuyin, role);
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
            public string Slug { get; }
            public string Name { get; }
            public string Description { get; }
            public string HouseRules { get; }
            public TimeZoneInfo Timezone { get; }
            public Currency Currency { get; }
            public int DefaultBuyin { get; }
            public Role Role { get; }

            public Result(string slug, string name, string description, string houseRules, TimeZoneInfo timezone, Currency currency, int defaultBuyin, Role role)
            {
                Slug = slug;
                Name = name;
                Description = description;
                HouseRules = houseRules;
                Timezone = timezone;
                Currency = currency;
                DefaultBuyin = defaultBuyin;
                Role = role;
            }
        }
    }
}