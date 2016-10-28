using System.ComponentModel.DataAnnotations;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class ForgotPassword
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageSender _messageSender;
        private readonly IRandomizer _randomizer;

        public ForgotPassword(IUserRepository userRepository, IMessageSender messageSender, IRandomizer randomizer)
        {
            _userRepository = userRepository;
            _messageSender = messageSender;
            _randomizer = randomizer;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var user = _userRepository.Get(request.Email);
            if(user == null)
                throw new UserNotFoundException(request.Email);

            var password = PasswordGenerator.CreatePassword(_randomizer.GetAllowedChars());
            var salt = SaltGenerator.CreateSalt(_randomizer.GetAllowedChars());
            var encryptedPassword = EncryptionService.Encrypt(password, salt);

            user.SetPassword(encryptedPassword, salt);

            _userRepository.Update(user);
            
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