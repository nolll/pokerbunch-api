using System.ComponentModel.DataAnnotations;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class InvitePlayer : UseCase<InvitePlayer.Request, InvitePlayer.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;

    public InvitePlayer(IBunchRepository bunchRepository, IPlayerRepository playerRepository, IEmailSender emailSender, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _playerRepository = playerRepository;
        _emailSender = emailSender;
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var player = await _playerRepository.Get(request.PlayerId);
        var bunch = await _bunchRepository.Get(player.BunchId);
        var currentUser = await _userRepository.GetByUserName(request.UserName);
        var currentPlayer = await _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanInvitePlayer(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var invitationCode = InvitationCodeCreator.GetCode(player);
        var joinUrl = string.Format(request.JoinUrlFormat, bunch.Slug);
        var joinWithCodeUrl = string.Format(request.JoinWithCodeUrlFormat, bunch.Slug, invitationCode);
        var message = new InvitationMessage(bunch.DisplayName, invitationCode, request.RegisterUrl, joinUrl, joinWithCodeUrl);
        _emailSender.Send(request.Email, message);

        return Success(new Result(player.Id));
    }

    public class Request
    {
        public string UserName { get; }
        public string PlayerId { get; }
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; }
        public string RegisterUrl { get; }
        public string JoinUrlFormat { get; }
        public string JoinWithCodeUrlFormat { get; }

        public Request(string userName, string playerId, string email, string registerUrl, string joinUrlFormat, string joinWithCodeUrlFormat)
        {
            UserName = userName;
            PlayerId = playerId;
            Email = email;
            RegisterUrl = registerUrl;
            JoinUrlFormat = joinUrlFormat;
            JoinWithCodeUrlFormat = joinWithCodeUrlFormat;
        }
    }

    public class Result
    {
        public string PlayerId { get; private set; }

        public Result(string playerId)
        {
            PlayerId = playerId;
        }
    }
}