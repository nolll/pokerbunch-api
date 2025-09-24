using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Login(
    IUserRepository userRepository,
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository
    ) : UseCase<Login.Request, Login.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await GetLoggedInUser(request.UserNameOrEmail, request.Password);
        if (user is null)
            return Error(new LoginError("There was something wrong with your username or password. Please try again."));

        var bunchResults = new List<LoginResultBunch>();
        var bunches = await bunchRepository.List(user.Id);
        foreach (var bunch in bunches)
        {
            var player = await playerRepository.Get(bunch.Id, user.Id);
            var role = player?.Role ?? Role.None;
            var id = player?.Id ?? "";
            var name = player?.DisplayName ?? "";
            bunchResults.Add(new LoginResultBunch(bunch.Id, bunch.Slug, bunch.DisplayName, id, name, role));
        }

        return Success(new Result(user.Id, user.UserName, user.DisplayName, user.IsAdmin, bunchResults));
    }
    
    private async Task<User?> GetLoggedInUser(string userNameOrEmail, string password)
    {
        try
        {
            var user = await userRepository.GetByUserNameOrEmail(userNameOrEmail);
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
        public string UserNameOrEmail { get; } = userNameOrEmail.ToLower();
        public string Password { get; } = password;
    }

    public class Result(
        string userId,
        string userName,
        string displayName,
        bool isAdmin,
        List<LoginResultBunch> bunchResults)
        : CommonLoginResult(userId, userName, displayName, isAdmin, bunchResults);
}