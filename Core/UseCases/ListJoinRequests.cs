using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ListJoinRequests(
    IJoinRequestRepository joinRequestRepository)
    : UseCase<ListJoinRequests.Request, ListJoinRequests.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));
        
        if (!request.Auth.CanListJoinRequests(request.Slug))
            return Error(new AccessDeniedError());

        var joinRequests = await joinRequestRepository.List(request.Slug);
        
        return Success(new Result(joinRequests));
    }

    public class Request(IAuth auth, string slug)
    {
        public IAuth Auth { get; } = auth;
        public string Slug { get; } = slug;
    }

    public class Result(IList<JoinRequest> joinRequests)
    {
        public IList<JoinRequest> JoinRequests { get; } = joinRequests;
    }
}