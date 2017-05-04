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
        private readonly IPlayerRepository _playerRepository;

        public GetBunch(IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
        {
            _bunchRepository = bunchRepository;
            _userRepository = userRepository;
            _playerRepository = playerRepository;
        }

        public BunchResult Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerRepository.Get(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            
            return new BunchResult(bunch, player);
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
        public BunchPlayerResult Player { get; }
        public Role Role { get; }

        public BunchResult(Bunch b, Player p)
        {
            Slug = b.Slug;
            Name = b.DisplayName;
            Description = b.Description;
            HouseRules = b.HouseRules;
            Timezone = b.Timezone;
            Currency = b.Currency;
            DefaultBuyin = b.DefaultBuyin;
            Player = p != null ? new BunchPlayerResult(p.Id, p.DisplayName) : null;
            Role = p?.Role ?? Role.Admin;
        }
    }

    public class BunchPlayerResult
    {
        public int? Id { get; }
        public string Name { get; }

        public BunchPlayerResult(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}