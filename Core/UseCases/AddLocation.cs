using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddLocation(ILocationRepository locationRepository)
    : UseCase<AddLocation.Request, AddLocation.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        if (!request.Auth.CanAddLocation(request.Slug))
            return Error(new AccessDeniedError());

        var location = new Location("", request.Name, request.Slug);
        var id = await locationRepository.Add(location);

        return Success(new Result(request.Slug, id, location.Name));
    }
    
    public class Request(IAuth auth, string slug, string name)
    {
        public IAuth Auth { get; } = auth;
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