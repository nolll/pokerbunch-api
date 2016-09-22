using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class EditUser
    {
        private readonly UserService _userService;

        public EditUser(UserService userService)
        {
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            var user = _userService.GetByNameOrEmail(request.UserName);
            var userToSave = GetUser(user, request);

            _userService.Save(userToSave);

            return new Result(userToSave.UserName, userToSave.Id);
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
            public string UserName { get; private set; }
            public int UserId { get; private set; }

            public Result(string userName, int userId)
            {
                UserName = userName;
                UserId = userId;
            }
        }
    }
}
