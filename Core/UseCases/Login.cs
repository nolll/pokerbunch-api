using System;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Login : UseCase<Login.Request, Login.Result>
{
    private readonly IUserRepository _userRepository;

    public Login(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await GetLoggedInUser(request.UserNameOrEmail, request.Password);

        return user is null
            ? Error(new LoginError("There was something wrong with your username or password. Please try again."))
            : Success(new Result(user.UserName));
    }
    
    private async Task<User?> GetLoggedInUser(string userNameOrEmail, string password)
    {
        try
        {
            var user = await _userRepository.GetByUserNameOrEmail(userNameOrEmail);
            var isValid = PasswordService.IsValid(password, user.Salt, user.EncryptedPassword);
            return isValid ? user : null;
        }
        catch
        {
            return null;
        }
    }

    public class Request 
    {
        public string UserNameOrEmail { get; }
        public string Password { get; }

        public Request(string userNameOrEmail, string password)
        {
            UserNameOrEmail = userNameOrEmail;
            Password = password;
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