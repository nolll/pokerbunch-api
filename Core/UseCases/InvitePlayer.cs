using System.ComponentModel.DataAnnotations;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class InvitePlayer
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMessageSender _messageSender;
        private readonly IUserRepository _userRepository;

        public InvitePlayer(IBunchRepository bunchRepository, IPlayerRepository playerRepository, IMessageSender messageSender, IUserRepository userRepository)
        {
            _bunchRepository = bunchRepository;
            _playerRepository = playerRepository;
            _messageSender = messageSender;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var player = _playerRepository.Get(request.PlayerId);
            var bunch = _bunchRepository.Get(player.BunchId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);
            RequireRole.Manager(currentUser, currentPlayer);

            var invitationCode = InvitationCodeCreator.GetCode(player);
            var joinUrl = string.Format(request.JoinUrlFormat, bunch.Slug);
            var joinWithCodeUrl = string.Format(request.JoinWithCodeUrlFormat, bunch.Slug, invitationCode);
            var message = new InvitationMessage(bunch.DisplayName, invitationCode, request.RegisterUrl, joinUrl, joinWithCodeUrl);
            _messageSender.Send(request.Email, message);

            return new Result(player.Id);
        }

        public class Request
        {
            public string UserName { get; }
            public int PlayerId { get; }
            [Required(ErrorMessage = "Email can't be empty")]
            [EmailAddress(ErrorMessage = "The email address is not valid")]
            public string Email { get; }
            public string RegisterUrl { get; }
            public string JoinUrlFormat { get; }
            public string JoinWithCodeUrlFormat { get; }

            public Request(string userName, int playerId, string email, string registerUrl, string joinUrlFormat, string joinWithCodeUrlFormat)
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
            public int PlayerId { get; private set; }

            public Result(int playerId)
            {
                PlayerId = playerId;
            }
        }
    }
}