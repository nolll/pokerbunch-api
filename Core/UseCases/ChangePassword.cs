using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ChangePassword : UseCase<ChangePassword.Request, ChangePassword.Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IRandomizer _randomizer;

    public ChangePassword(IUserRepository userRepository, IRandomizer randomizer)
    {
        _userRepository = userRepository;
        _randomizer = randomizer;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var user = await _userRepository.GetByUserName(request.UserName);
        var isCurrentPwdValid = PasswordService.IsValid(request.OldPassword, user.Salt, user.EncryptedPassword);
        if (!isCurrentPwdValid)
            return Error(new AuthError("The old password was not correct"));

        var salt = SaltGenerator.CreateSalt(_randomizer.GetAllowedChars());
        var encryptedPassword = EncryptionService.Encrypt(request.NewPassword, salt);
        user = CreateUser(user, encryptedPassword, salt);

        await _userRepository.Update(user);

        return Success(new Result());
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
        public string NewPassword { get; }
        public string OldPassword { get; }

        public Request(string userName, string newPassword, string oldPassword)
        {
            UserName = userName;
            NewPassword = newPassword;
            OldPassword = oldPassword;
        }
    }

    public class Result
    {
    }
}