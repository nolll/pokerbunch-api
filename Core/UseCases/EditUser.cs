using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;

namespace Core.UseCases;

public class EditUser : UseCase<EditUser.Request, EditUser.Result>
{
    private readonly IUserRepository _userRepository;

    public EditUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var user = await _userRepository.Get(request.UserName);
        var userToSave = GetUser(user, request);

        await _userRepository.Update(userToSave);

        return Success(new Result(userToSave.UserName));
    }

    private static User GetUser(User user, Request request)
    {
        return new User(
            user.Id,
            user.UserName,
            request.DisplayName,
            request.RealName,
            request.Email,
            user.GlobalRole,
            user.EncryptedPassword,
            user.Salt);
    }

    public class Request
    {
        public string UserName { get; }
        [Required(ErrorMessage = "Display Name can't be empty")]
        public string DisplayName { get; }
        public string RealName { get; }
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; }

        public Request(string userName, string displayName, string realName, string email)
        {
            UserName = userName;
            DisplayName = displayName;
            RealName = realName;
            Email = email;
        }
    }

    public class Result
    {
        public string UserName { get; }

        public Result(string userName)
        {
            UserName = userName;
        }
    }
}