using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class ChangePassword
    {
        private readonly IUserRepository _userRepository;
        private readonly IRandomizer _randomizer;

        public ChangePassword(IUserRepository userRepository, IRandomizer randomizer)
        {
            _userRepository = userRepository;
            _randomizer = randomizer;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            var user = _userRepository.Get(request.UserName);
            var isCurrentPwdValid = PasswordService.IsValid(request.OldPassword, user.Salt, user.EncryptedPassword);
            if (!isCurrentPwdValid)
                throw new AuthException();

            var salt = SaltGenerator.CreateSalt(_randomizer.GetAllowedChars());
            var encryptedPassword = EncryptionService.Encrypt(request.Password, salt);
            user = CreateUser(user, encryptedPassword, salt);

            _userRepository.Update(user);
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
            public string OldPassword { get; }

            public Request(string userName, string password, string oldPassword)
            {
                UserName = userName;
                Password = password;
                OldPassword = oldPassword;
            }
        }
    }
}
