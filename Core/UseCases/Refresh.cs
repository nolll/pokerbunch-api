using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Refresh(
    IUserRepository userRepository,
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository
) : UseCase<Refresh.Request, Refresh.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await GetLoggedInUser(request.UserName);
        if (user is null)
            return Error(new LoginError("There was something wrong with your refresh token. Please try again."));

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
    
    private async Task<User?> GetLoggedInUser(string userName)
    {
        try
        {
            return await userRepository.GetByUserNameOrEmail(userName);
        }
        catch
        {
            return null;
        }
    }

    public class Request(string userName)
    {
        public string UserName { get; } = userName;
    }

    public class Result(
        string userId,
        string userName,
        string displayName,
        bool isAdmin,
        List<LoginResultBunch> bunchResults)
        : CommonLoginResult(userId, userName, displayName, isAdmin, bunchResults);
}

public class CommonLoginResult(string userId, string userName, string displayName, bool isAdmin, List<LoginResultBunch> bunchResults)
{
    public string UserId { get; } = userId;
    public string UserName { get; } = userName;
    public string DisplayName { get; } = displayName;
    public bool IsAdmin { get; } = isAdmin;
    public List<LoginResultBunch> BunchResults { get; } = bunchResults;
}

public class LoginResultBunch(string bunchId, string bunchSlug, string bunchName, string playerId, string playerName, Role role)
{
    public string BunchId { get; } = bunchId;
    public string BunchSlug { get; } = bunchSlug;
    public string BunchName { get; } = bunchName;
    public string PlayerId { get; } = playerId;
    public string PlayerName { get; } = playerName;
    public Role Role { get; } = role;
}