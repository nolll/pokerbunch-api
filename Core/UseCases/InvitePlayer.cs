using System.ComponentModel.DataAnnotations;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class InvitePlayer(
    IPlayerRepository playerRepository,
    IEmailSender emailSender,
    IInvitationCodeCreator invitationCodeCreator)
    : UseCase<InvitePlayer.Request, InvitePlayer.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var player = await playerRepository.Get(request.PlayerId);

        if (!request.Auth.CanInvitePlayer(player.BunchSlug))
            return Error(new AccessDeniedError());
        
        var bunchInfo = request.Auth.GetBunch(player.BunchSlug);
        var invitationCode = invitationCodeCreator.GetCode(player);
        var joinUrl = string.Format(request.JoinUrlFormat, player.BunchSlug);
        var joinWithCodeUrl = string.Format(request.JoinWithCodeUrlFormat, player.BunchSlug, invitationCode);
        var message = new InvitationMessage(bunchInfo.Name, invitationCode, request.RegisterUrl, joinUrl, joinWithCodeUrl);
        emailSender.Send(request.Email, message);

        return Success(new Result(player.Id));
    }

    public class Request(
        IAuth auth,
        string playerId,
        string email,
        string registerUrl,
        string joinUrlFormat,
        string joinWithCodeUrlFormat)
    {
        public IAuth Auth { get; } = auth;
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