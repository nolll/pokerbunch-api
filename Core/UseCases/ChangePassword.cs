using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class ChangePassword
    {
        private readonly UserService _userService;
        private readonly IRandomService _randomService;

        public ChangePassword(UserService userService, IRandomService randomService)
        {
            _userService = userService;
            _randomService = randomService;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            if (request.Password != request.Repeat)
                throw new ValidationException("The passwords dos not match");

            var salt = SaltGenerator.CreateSalt(_randomService.GetAllowedChars());
            var encryptedPassword = EncryptionService.Encrypt(request.Password, salt);
            var user = _userService.GetByNameOrEmail(request.UserName);
            user = CreateUser(user, encryptedPassword, salt);

            _userService.Save(user);
        }

        private static User CreateUser(User user, string encryptedPassword, string salt)
        {
            return new User(
                user.Id,
                user.UserName,
                user.DisplayName,
                user.RealName,
                user.Email,
                user.GlobalRole,
                encryptedPassword,
                salt);
        }

        public class Request
        {
            public string UserName { get; }
            [Required(ErrorMessage = "Password can't be empty")]
            public string Password { get; }
            public string Repeat { get; }

            public Request(string userName, string password, string repeat)
            {
                UserName = userName;
                Password = password;
                Repeat = repeat;
            }
        }
    }
}
