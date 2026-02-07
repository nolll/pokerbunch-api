using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddJoinRequest(
    IBunchRepository bunchRepository,
    IJoinRequestRepository joinRequestRepository)
    : UseCase<AddJoinRequest.Request, AddJoinRequest.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        
        await joinRequestRepository.Add(new JoinRequest("", request.Slug, request.Auth.Id, request.Auth.UserName));
        return Success(new Result(bunch.Slug, bunch.DisplayName));
    }

    public class Request(IAuth auth, string slug)
    {
        public IAuth Auth { get; } = auth;
        public string Slug { get; } = slug;
    }

    public class Result(string slug, string bunchName)
    {
        public string Slug { get; private set; } = slug;
        public string BunchName { get; private set; } = bunchName;
    }
}