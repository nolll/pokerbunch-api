using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Messages;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AcceptJoinRequest(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    IJoinRequestRepository joinRequestRepository,
    IUserRepository userRepository,
    IEmailSender emailSender)
    : UseCase<AcceptJoinRequest.Request, AcceptJoinRequest.Result>
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

        await playerRepository.Add(Player.New(bunch.Slug, joinRequest.UserId, joinRequest.UserName));
        await joinRequestRepository.Delete(request.Id);

        var user = await userRepository.GetById(joinRequest.UserId);
        var message = new AcceptJoinRequestMessage(bunch.DisplayName);
        await emailSender.SendAsync(user.Email, message);
        
        return Success(new Result("Join request accepted!"));
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