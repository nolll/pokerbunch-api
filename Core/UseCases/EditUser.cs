using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;

namespace Core.UseCases;

public class EditUser(IUserRepository userRepository) : UseCase<EditUser.Request, EditUser.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var user = await userRepository.GetByUserName(request.UserName);
        var userToSave = GetUser(user, request);

        await userRepository.Update(userToSave);

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

    public class Request(string userName, string displayName, string? realName, string email)
    {
        public string UserName { get; } = userName;

        [Required(ErrorMessage = "Display Name can't be empty")]
        public string DisplayName { get; } = displayName;

        public string? RealName { get; } = realName;

        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; } = email;
    }

    public class Result(string userName)
    {
        public string UserName { get; } = userName;
    }
}