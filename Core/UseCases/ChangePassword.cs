using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ChangePassword(IUserRepository userRepository, IRandomizer randomizer)
    : UseCase<ChangePassword.Request, ChangePassword.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var user = await userRepository.GetByUserName(request.Auth.UserName);
        var isCurrentPwdValid = PasswordService.IsValid(request.OldPassword, user.Salt, user.EncryptedPassword);
        if (!isCurrentPwdValid)
            return Error(new AuthError("The old password was not correct"));

        var salt = SaltGenerator.CreateSalt(randomizer.GetAllowedChars());
        var encryptedPassword = EncryptionService.Encrypt(request.NewPassword, salt);
        user = CreateUser(user, encryptedPassword, salt);

        await userRepository.Update(user);

        return Success(new Result());
    }

    private static User CreateUser(User user, string encryptedPassword, string salt) => new(
        user.Id,
        user.UserName,
        user.DisplayName,
        user.RealName,
        user.Email,
        user.GlobalRole,
        encryptedPassword,
        salt);

    public class Request(IAuth auth, string newPassword, string oldPassword)
    {
        public IAuth Auth { get; } = auth;

        [Required(ErrorMessage = "Password can't be empty")]
        public string NewPassword { get; } = newPassword;

        public string OldPassword { get; } = oldPassword;
    }

    public class Result
    {
    }
}