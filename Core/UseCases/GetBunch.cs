using System;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class GetBunch
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPlayerService _playerService;

        public GetBunch(IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerService playerService)
        {
            _bunchRepository = bunchRepository;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public BunchResult Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);

            var role = player.Role;

            return new BunchResult(bunch, role);
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
    }

    public class BunchResult
    {
        public string Slug { get; }
        public string Name { get; }
        public string Description { get; }
        public string HouseRules { get; }
        public TimeZoneInfo Timezone { get; }
        public Currency Currency { get; }
        public int DefaultBuyin { get; }
        public Role Role { get; }

        public BunchResult(Bunch b, Role r)
        {
            Slug = b.Slug;
            Name = b.DisplayName;
            Description = b.Description;
            HouseRules = b.HouseRules;
            Timezone = b.Timezone;
            Currency = b.Currency;
            DefaultBuyin = b.DefaultBuyin;
            Role = r;
        }
    }
}