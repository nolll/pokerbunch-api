using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class AddPlayer
    {
        private readonly BunchService _bunchService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public AddPlayer(BunchService bunchService, PlayerService playerService, UserService userService)
        {
            _bunchService = bunchService;
            _playerService = playerService;
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var bunch = _bunchService.GetBySlug(request.Slug);
            var currentUser = _userService.GetByNameOrEmail(request.UserName);
            var currentPlayer = _playerService.GetByUserId(bunch.Id, currentUser.Id);
            RequireRole.Manager(currentUser, currentPlayer);
            var existingPlayers = _playerService.GetList(bunch.Id);
            var player = existingPlayers.FirstOrDefault(o => string.Equals(o.DisplayName, request.Name, StringComparison.CurrentCultureIgnoreCase));
            if(player != null)
                throw new PlayerExistsException();

            player = Player.New(bunch.Id, request.Name);
            _playerService.Add(player);

            return new Result(bunch.Slug);
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }
            [Required(ErrorMessage = "Name can't be empty")]
            public string Name { get; }

            public Request(string userName, string slug, string name)
            {
                UserName = userName;
                Slug = slug;
                Name = name;
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