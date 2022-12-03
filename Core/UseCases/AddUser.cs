using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddUser : UseCase<AddUser.Request, AddUser.Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IRandomizer _randomizer;
    private readonly IEmailSender _emailSender;

    public AddUser(
        IUserRepository userRepository,
        IRandomizer randomizer,
        IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _randomizer = randomizer;
        _emailSender = emailSender;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);

        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var userByName = await _userRepository.GetByUserName(request.UserName);
        if (userByName != null)
            return Error(new UserExistsError());

        var userByEmail = await _userRepository.GetByUserEmail(request.Email);
        if (userByEmail != null)
            return Error(new EmailExistsError());

        var salt = SaltGenerator.CreateSalt(_randomizer.GetAllowedChars());
        var encryptedPassword = EncryptionService.Encrypt(request.Password, salt);
        var user = CreateUser(request, encryptedPassword, salt);

        await _userRepository.Add(user);

        var message = new RegistrationMessage(request.LoginUrl);
        _emailSender.Send(request.Email, message);

        return Success(new Result());
    }
    
    private static User CreateUser(Request request, string encryptedPassword, string salt)
    {
        return new User(
            null,
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

    public class Result
    {
    }
}