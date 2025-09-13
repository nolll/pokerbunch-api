using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddEvent(IEventRepository eventRepository)
    : UseCase<AddEvent.Request, AddEvent.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));
        
        var bunchInfo = request.Auth.GetBunchBySlug(request.Slug);

        if (!request.Auth.CanAddEvent(bunchInfo.Id))
            return Error(new AccessDeniedError());

        var e = new Event("", bunchInfo.Id, request.Name);
        var id = await eventRepository.Add(e);

        return Success(new Result(id));
    }
    
    public class Request(IAuth auth, string slug, string name)
    {
        public IAuth Auth { get; } = auth;
        public string Slug { get; } = slug;

        [Required(ErrorMessage = "Name can't be empty")]
        public string Name { get; } = name;
    }

    public class Result(string id)
    {
        public string Id { get; } = id;
    }
}