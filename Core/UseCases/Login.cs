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

    public class Request(string userNameOrEmail, string password)
    {
        public string UserNameOrEmail { get; } = userNameOrEmail;
        public string Password { get; } = password;
    }

    public class Result(string userName)
    {
        public string UserName { get; } = userName;
    }
}