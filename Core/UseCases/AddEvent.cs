using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddEvent(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    IUserRepository userRepository,
    IEventRepository eventRepository)
    : UseCase<AddEvent.Request, AddEvent.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanAddEvent(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var e = new Event("", bunch.Id, request.Name);
        var id = await eventRepository.Add(e);

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