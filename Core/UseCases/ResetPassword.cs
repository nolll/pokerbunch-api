using System.ComponentModel.DataAnnotations;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ResetPassword(IUserRepository userRepository, IEmailSender emailSender, IRandomizer randomizer)
    : UseCase<ResetPassword.Request, ResetPassword.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        // todo: Find a way to handle these cases. Null or something else.
        var user = await userRepository.GetByUserEmail(request.Email);
        if (user == null)
            return Error(new UserNotFoundError(request.Email));

        var password = PasswordService.CreatePassword(randomizer.GetAllowedChars());
        var salt = SaltGenerator.CreateSalt(randomizer.GetAllowedChars());
        var encryptedPassword = EncryptionService.Encrypt(password, salt);

        user.SetPassword(encryptedPassword, salt);

        await userRepository.Update(user);

        var message = new ResetPasswordMessage(password, request.LoginUrl);
        emailSender.Send(request.Email, message);

        return Success(new Result());
    }

    public class Request(string email, string loginUrl)
    {
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; } = email.ToLower();

        public string LoginUrl { get; } = loginUrl;
    }

    public class Result
    {
    }

    private class ResetPasswordMessage(string password, string loginUrl) : IMessage
    {
        public string Subject => "Poker Bunch Password Recovery";
        public string Body => string.Format(BodyFormat, password, loginUrl);

        private const string BodyFormat =
            """
            Here is your new password for Poker Bunch:
            {0}

            Please sign in here: {1}
            """;
    }
}