using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class GetPlayerList
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;

        public GetPlayerList(IBunchRepository bunchRepository, IUserRepository userRepository, PlayerService playerService)
        {
            _bunchRepository = bunchRepository;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Player(user, player);
            var players = _playerService.GetList(bunch.Id);
            var isManager = RoleHandler.IsInRole(user, player, Role.Manager);

            return new Result(bunch, players, isManager);
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
            public IList<ResultItem> Players { get; private set; }
            public bool CanAddPlayer { get; private set; }
            public string Slug { get; private set; }

            public Result(Bunch bunch, IEnumerable<Player> players, bool isManager)
            {
                Players = players.Select(o => new ResultItem(o)).OrderBy(o => o.Name).ToList();
                CanAddPlayer = isManager;
                Slug = bunch.Slug;
            }
        }

        public class ResultItem
        {
            public string Name { get; }
            public int Id { get; private set; }
            public string Color { get; set; }

            public ResultItem(Player player)
            {
                Name = player.DisplayName;
                Id = player.Id;
                Color = player.Color;
            }
        }
    }
}