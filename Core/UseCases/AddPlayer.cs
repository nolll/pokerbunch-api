using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases;

public class AddPlayer
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;

    public AddPlayer(IBunchRepository bunchRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
    }

    public Result Execute(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            throw new ValidationException(validator);

        var bunch = _bunchRepository.GetBySlug(request.Slug);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);
        RequireRole.Manager(currentUser, currentPlayer);
        var existingPlayers = _playerRepository.List(bunch.Id);
        var player = existingPlayers.FirstOrDefault(o => string.Equals(o.DisplayName, request.Name, StringComparison.CurrentCultureIgnoreCase));
        if(player != null)
            throw new PlayerExistsException();

        player = Player.New(bunch.Id, request.Name);
        var id = _playerRepository.Add(player);

        return new Result(id);
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
        public int Id { get; private set; }

        public Result(int id)
        {
            Id = id;
        }
    }
}