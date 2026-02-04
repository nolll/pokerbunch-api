using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class JoinBunchRequest(
    IBunchRepository bunchRepository,
    IJoinRequestRepository joinRequestRepository)
    : UseCase<JoinBunchRequest.Request, JoinBunchRequest.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);

        var id = await joinRequestRepository.Add(new JoinRequest("", request.Slug, request.Auth.Id));
        return Success(new Result(bunch.Slug, id));
    }

    public class Request(IAuth auth, string slug)
    {
        public IAuth Auth { get; } = auth;
        public string Slug { get; } = slug;
    }

    public class Result(string slug, string playerId)
    {
        public string Slug { get; private set; } = slug;
        public string PlayerId { get; private set; } = playerId;
    }
}