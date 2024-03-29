﻿using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddLocation(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    IUserRepository userRepository,
    ILocationRepository locationRepository)
    : UseCase<AddLocation.Request, AddLocation.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanAddLocation(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var location = new Location("", request.Name, bunch.Id);
        var id = await locationRepository.Add(location);

        return Success(new Result(bunch.Slug, id, location.Name));
    }
    
    public class Request(string userName, string slug, string name)
    {
        public string UserName { get; } = userName;
        public string Slug { get; } = slug;

        [Required(ErrorMessage = "Name can't be empty")]
        public string Name { get; } = name;
    }

    public class Result(string slug, string id, string name)
    {
        public string Slug { get; } = slug;
        public string Id { get; } = id;
        public string Name { get; } = name;
    }
}