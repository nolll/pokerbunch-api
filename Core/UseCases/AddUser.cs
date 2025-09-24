using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddUser(
    IUserRepository userRepository,
    IRandomizer randomizer,
    IEmailSender emailSender)
    : UseCase<AddUser.Request, AddUser.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));
        
        var userByName = await GetExistingUserByUserName(request.UserName);
        if (userByName != null)
            return Error(new UserExistsError());

        var userByEmail = await GetExistingUserByEmail(request.Email);
        if (userByEmail != null)
            return Error(new EmailExistsError());

        var salt = SaltGenerator.CreateSalt(randomizer.GetAllowedChars());
        var encryptedPassword = EncryptionService.Encrypt(request.Password, salt);
        var user = CreateUser(request, encryptedPassword, salt);

        await userRepository.Add(user);

        var message = new RegistrationMessage(request.LoginUrl);
        emailSender.Send(request.Email, message);

        return Success(new Result());
    }

    private async Task<User?> GetExistingUserByUserName(string userName)
    {
        try
        {
            return await userRepository.GetByUserName(userName);
        }
        catch (PokerBunchException)
        {
            return null;
        }
    }

    private async Task<User?> GetExistingUserByEmail(string email)
    {
        try
        {
            return await userRepository.GetByUserEmail(email);
        }
        catch (PokerBunchException)
        {
            return null;
        }
    }

    private static User CreateUser(Request request, string encryptedPassword, string salt) =>
        new(
            "",
            request.UserName,
            request.DisplayName,
            "",
            request.Email,
            Role.Player,
            encryptedPassword,
            salt);

    public class Request(string userName, string displayName, string email, string password, string loginUrl)
    {
        [Required(ErrorMessage = "Login Name can't be empty")]
        public string UserName { get; } = userName.ToLower();

        [Required(ErrorMessage = "Display Name can't be empty")]
        public string DisplayName { get; } = displayName;

        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; } = email.ToLower();

        [Required(ErrorMessage = "Password can't be empty")]
        public string Password { get; } = password;

        public string LoginUrl { get; } = loginUrl;
    }

    public class Result
    {
    }
}