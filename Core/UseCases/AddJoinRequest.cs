using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Messages;
using Core.Repositories;
using Core.Services;
using Core.Services.Interfaces;

namespace Core.UseCases;

public class AddJoinRequest(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    IJoinRequestRepository joinRequestRepository,
    IUserRepository userRepository,
    IEmailSender emailSender,
    ISiteUrlProvider siteUrlProvider)
    : UseCase<AddJoinRequest.Request, AddJoinRequest.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var existingPlayer = await playerRepository.Get(bunch.Id, request.Auth.Id);
        if (existingPlayer is not null)
            return Error(new ValidationError($"You are already a member of {bunch.DisplayName}"));

        var existingApplication = await joinRequestRepository.Get(bunch.Id, request.Auth.Id);
        if (existingApplication is not null)
            return Error(new ValidationError($"You have already applied for membership to {bunch.DisplayName}"));

        var managerPlayers = await playerRepository.List(bunch.Slug, Role.Manager);
        var managerUserIds = managerPlayers.Where(o => o.IsUser).Select(o => o.UserId!).ToList();
        if (managerUserIds.Count > 0)
        {
            var managerUsers = await userRepository.GetByIds(managerUserIds);
            var url = siteUrlProvider.JoinRequestList(bunch.Slug);
            var message = new JoinRequestMessage(bunch.DisplayName, request.Auth.UserName, url);
            foreach (var user in managerUsers)
            {
                emailSender.Send(user.Email, message);
            }
        }
        
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