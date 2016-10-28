using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class AddUser
    {
        private readonly IUserRepository _userRepository;
        private readonly IRandomizer _randomizer;
        private readonly IMessageSender _messageSender;

        public AddUser(
            IUserRepository userRepository,
            IRandomizer randomizer,
            IMessageSender messageSender)
        {
            _userRepository = userRepository;
            _randomizer = randomizer;
            _messageSender = messageSender;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            if (_userRepository.Get(request.UserName) != null)
                throw new UserExistsException();

            if (_userRepository.Get(request.Email) != null)
                throw new EmailExistsException();

            var salt = SaltGenerator.CreateSalt(_randomizer.GetAllowedChars());
            var encryptedPassword = EncryptionService.Encrypt(request.Password, salt);
            var user = CreateUser(request, encryptedPassword, salt);

            _userRepository.Add(user);
            
            var message = new RegistrationMessage(request.LoginUrl);
            _messageSender.Send(request.Email, message);
        }

        private static User CreateUser(Request request, string encryptedPassword, string salt)
        {
            return new User(
                0,
                request.UserName,
                request.DisplayName,
                string.Empty,
                request.Email,
                Role.Player,
                encryptedPassword,
                salt);
        }

        public class Request
        {
            [Required(ErrorMessage = "Login Name can't be empty")]
            public string UserName { get; }

            [Required(ErrorMessage = "Display Name can't be empty")]
            public string DisplayName { get; }

            [Required(ErrorMessage = "Email can't be empty")]
            [EmailAddress(ErrorMessage = "The email address is not valid")]
            public string Email { get; }

            [Required(ErrorMessage = "Password can't be empty")]
            public string Password { get; }

            public string LoginUrl { get; }

            public Request(string userName, string displayName, string email, string password, string loginUrl)
            {
                UserName = userName;
                DisplayName = displayName;
                Email = email;
                Password = password;
                LoginUrl = loginUrl;
            }
        }
    }
}