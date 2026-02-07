using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class DenyJoinRequest(
    IBunchRepository bunchRepository,
    IJoinRequestRepository joinRequestRepository)
    : UseCase<DenyJoinRequest.Request, DenyJoinRequest.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var joinRequest = (await joinRequestRepository.Get([request.Id])).FirstOrDefault();
        if (joinRequest is null)
            return Error(new JoinRequestNotFoundError(request.Id));

        var bunch = await bunchRepository.Get(joinRequest.BunchId);
        if (!request.Auth.CanHandleJoinRequest(bunch.Slug))
            return Error(new AccessDeniedError());

        await joinRequestRepository.Delete(request.Id);

        return Success(new Result("Join request denied"));
    }

    public class Request(IAuth auth, string id)
    {
        public IAuth Auth { get; } = auth;
        public string Id { get; } = id;
    }

    public class Result(string message)
    {
        public string Message { get; private set; } = message;
    }
}