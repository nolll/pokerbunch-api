using System.ComponentModel.DataAnnotations;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class InvitePlayer(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository,
    IEmailSender emailSender,
    IUserRepository userRepository,
    IInvitationCodeCreator invitationCodeCreator)
    : UseCase<InvitePlayer.Request, InvitePlayer.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var player = await playerRepository.Get(request.PlayerId);
        var bunch = await bunchRepository.Get(player.BunchId);
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanInvitePlayer(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var invitationCode = invitationCodeCreator.GetCode(player);
        var joinUrl = string.Format(request.JoinUrlFormat, bunch.Slug);
        var joinWithCodeUrl = string.Format(request.JoinWithCodeUrlFormat, bunch.Slug, invitationCode);
        var message = new InvitationMessage(bunch.DisplayName, invitationCode, request.RegisterUrl, joinUrl, joinWithCodeUrl);
        emailSender.Send(request.Email, message);

        return Success(new Result(player.Id));
    }

    public class Request(
        string userName,
        string playerId,
        string email,
        string registerUrl,
        string joinUrlFormat,
        string joinWithCodeUrlFormat)
    {
        public string UserName { get; } = userName;
        public string PlayerId { get; } = playerId;

        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; } = email;

        public string RegisterUrl { get; } = registerUrl;
        public string JoinUrlFormat { get; } = joinUrlFormat;
        public string JoinWithCodeUrlFormat { get; } = joinWithCodeUrlFormat;
    }

    public class Result(string playerId)
    {
        public string PlayerId { get; } = playerId;
    }
}