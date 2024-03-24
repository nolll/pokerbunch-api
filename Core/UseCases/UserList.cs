using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class UserList(IUserRepository userRepository) : UseCase<UserList.Request, UserList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await userRepository.GetByUserName(request.UserName);
        if (!AccessControl.CanListUsers(user))
            return Error(new AccessDeniedError());

        var users = await userRepository.List();
        var userItems = users.Select(o => new UserListItem(o.DisplayName, o.UserName)).ToList();

        return Success(new Result(userItems));
    }

    public class Request(string userName)
    {
        public string UserName { get; } = userName;
    }

    public class Result(IList<UserListItem> userItems)
    {
        public IList<UserListItem> Users { get; } = userItems;
    }

    public class UserListItem(string displayName, string userName)
    {
        public string DisplayName { get; } = displayName;
        public string UserName { get; } = userName;
    }
}