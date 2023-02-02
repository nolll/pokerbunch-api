using System.ComponentModel.DataAnnotations;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class ResetPassword : UseCase<ResetPassword.Request, ResetPassword.Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;
    private readonly IRandomizer _randomizer;

    public ResetPassword(IUserRepository userRepository, IEmailSender emailSender, IRandomizer randomizer)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
        _randomizer = randomizer;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var user = await _userRepository.GetByUserEmail(request.Email);
        if (user == null)
            return Error(new UserNotFoundError(request.Email));

        var password = PasswordService.CreatePassword(_randomizer.GetAllowedChars());
        var salt = SaltGenerator.CreateSalt(_randomizer.GetAllowedChars());
        var encryptedPassword = EncryptionService.Encrypt(password, salt);

        user.SetPassword(encryptedPassword, salt);

        await _userRepository.Update(user);

        var message = new ResetPasswordMessage(password, request.LoginUrl);
        _emailSender.Send(request.Email, message);

        return Success(new Result());
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

    public class Result
    {
    }

    private class ResetPasswordMessage : IMessage
    {
        private readonly string _password;
        private readonly string _loginUrl;

        public ResetPasswordMessage(string password, string loginUrl)
        {
            _password = password;
            _loginUrl = loginUrl;
        }

        public string Subject => "Poker Bunch Password Recovery";
        public string Body => string.Format(BodyFormat, _password, _loginUrl);

        private const string BodyFormat =
            @"Here is your new password for Poker Bunch:
{0}

Please sign in here: {1}";
    }
}