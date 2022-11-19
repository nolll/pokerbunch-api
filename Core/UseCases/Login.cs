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
        var user = await GetLoggedInUser(request.LoginName, request.Password);

        if (user == null)
            return Error(new LoginError("There was something wrong with your username or password. Please try again."));
        return Success(new Result(user.UserName));
    }
    
    private async Task<User> GetLoggedInUser(string loginName, string password)
    {
        var user = await _userRepository.Get(loginName);
        if (user == null)
            return null;

        var isValid = PasswordService.IsValid(password, user.Salt, user.EncryptedPassword);
        return isValid ? user : null;
    }

    public class Request 
    {
        public string LoginName { get; }
        public string Password { get; }

        public Request(string loginName, string password)
        {
            LoginName = loginName;
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