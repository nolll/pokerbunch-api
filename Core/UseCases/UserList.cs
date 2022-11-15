using System.Collections.Generic;
using System.Linq;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class UserList : UseCase<UserList.Request, UserList.Result>
{
    private readonly IUserRepository _userRepository;

    public UserList(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var user = _userRepository.Get(request.UserName);
        if (!AccessControl.CanListUsers(user))
            return Error(new AccessDeniedError());

        var users = _userRepository.List();
        var userItems = users.Select(o => new UserListItem(o.DisplayName, o.UserName)).ToList();

        return Success(new Result(userItems));
    }

    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }

    public class Result
    {
        public IList<UserListItem> Users { get; }

        public Result(IList<UserListItem> userItems)
        {
            Users = userItems;
        }
    }

    public class UserListItem
    {
        public string DisplayName { get; }
        public string UserName { get; }

        public UserListItem(string displayName, string userName)
        {
            DisplayName = displayName;
            UserName = userName;
        }
    }
}