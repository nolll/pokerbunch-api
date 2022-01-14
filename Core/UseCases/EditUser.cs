using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Repositories;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases;

public class EditUser
{
    private readonly IUserRepository _userRepository;

    public EditUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Result Execute(Request request)
    {
        var validator = new Validator(request);
        if(!validator.IsValid)
            throw new ValidationException(validator);

        var user = _userRepository.Get(request.UserName);
        var userToSave = GetUser(user, request);

        _userRepository.Update(userToSave);

        return new Result(userToSave.UserName);
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