using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddPlayer(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    IUserRepository userRepository)
    : UseCase<AddPlayer.Request, AddPlayer.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);
        if (!AccessControl.CanAddPlayer(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var existingPlayers = await playerRepository.List(bunch.Id);
        var player = existingPlayers.FirstOrDefault(o => string.Equals(o.DisplayName, request.Name, StringComparison.CurrentCultureIgnoreCase));
        if (player != null)
            return Error(new PlayerExistsError());

        player = Player.New(bunch.Id, request.Name);
        var id = await playerRepository.Add(player);

        return Success(new Result(id));
    }
    
    public class Request(string userName, string slug, string name)
    {
        public string UserName { get; } = userName;
        public string Slug { get; } = slug;

        [Required(ErrorMessage = "Name can't be empty")]
        public string Name { get; } = name;
    }

    public class Result(string id)
    {
        public string Id { get; } = id;
    }
}