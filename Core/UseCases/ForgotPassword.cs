using System.ComponentModel.DataAnnotations;
using Core.Exceptions;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class ForgotPassword
    {
        private readonly UserService _userService;
        private readonly IMessageSender _messageSender;
        private readonly IRandomService _randomService;

        public ForgotPassword(UserService userService, IMessageSender messageSender, IRandomService randomService)
        {
            _userService = userService;
            _messageSender = messageSender;
            _randomService = randomService;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var user = _userService.GetByNameOrEmail(request.Email);
            if(user == null)
                throw new UserNotFoundException();

            var password = PasswordGenerator.CreatePassword(_randomService.GetAllowedChars());
            var salt = SaltGenerator.CreateSalt(_randomService.GetAllowedChars());
            var encryptedPassword = EncryptionService.Encrypt(password, salt);

            user.SetPassword(encryptedPassword, salt);

            _userService.Save(user);
            
            var message = new ForgotPasswordMessage(password, request.LoginUrl);
            _messageSender.Send(request.Email, message);
        }

        public class Request
        {
            [Required(ErrorMessage = "Email can't be empty")]
            [EmailAddress(ErrorMessage = "The email address is not valid")]
            public string Email { get; }
            public string LoginUrl { get; }

            public Request(string email, string loginUrl)
            {
                Email = email;
                LoginUrl = loginUrl;
            }
        }

        private class ForgotPasswordMessage : IMessage
        {
            private readonly string _password;
            private readonly string _loginUrl;

            public ForgotPasswordMessage(string password, string loginUrl)
            {
                _password = password;
                _loginUrl = loginUrl;
            }

            public string Subject
            {
                get { return "Poker Bunch Password Recovery"; }
            }

            public string Body
            {
                get
                {
                    return string.Format(BodyFormat, _password, _loginUrl);
                }
            }

            private const string BodyFormat =
@"Here is your new password for Poker Bunch:
{0}

Please sign in here: {1}";
        }
    }
}