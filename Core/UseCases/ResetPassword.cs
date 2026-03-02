using System.ComponentModel.DataAnnotations;
using Core.Errors;
using Core.Messages;
using Core.Repositories;
using Core.Services;
using Core.Services.Interfaces;

namespace Core.UseCases;

public class ResetPassword(
    IUserRepository userRepository, 
    IEmailSender emailSender, 
    IRandomizer randomizer, 
    ISiteUrlProvider siteUrlProvider)
    : UseCase<ResetPassword.Request, ResetPassword.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));
        
        var password = PasswordService.CreatePassword(randomizer.GetAllowedChars());
        var salt = SaltGenerator.CreateSalt(randomizer.GetAllowedChars());
        var encryptedPassword = EncryptionService.Encrypt(password, salt);
        
        var user = await userRepository.GetByUserEmail(request.Email);
        if (user is null)
            return Error(new UserNotFoundError(request.Email));
        
        user.SetPassword(encryptedPassword, salt);

        await userRepository.Update(user);

        var message = new ResetPasswordMessage(password, siteUrlProvider.Login());
        emailSender.Send(request.Email, message);

        return Success(new Result());
    }

    public class Request(string email)
    {
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; } = email.ToLower();
    }

    public class Result
    {
    }
}