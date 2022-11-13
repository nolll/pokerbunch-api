using System.Collections.Generic;
using System.Linq;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class UserList
{
    private readonly IUserRepository _userRepository;

    public UserList(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Result Execute(Request request)
    {
        var user = _userRepository.Get(request.UserName);
        if (!AccessControl.CanListUsers(user))
            throw new AccessDeniedException();
            
        var users = _userRepository.List();
        var userItems = users.Select(o => new UserListItem(o.DisplayName, o.UserName)).ToList();

        return new Result(userItems);
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
        public IList<UserListItem> Users { get; private set; }

        public Result(IList<UserListItem> userItems)
        {
            Users = userItems;
        }
    }

    public class UserListItem
    {
        public string DisplayName { get; private set; }
        public string UserName { get; private set; }

        public UserListItem(string displayName, string userName)
        {
            DisplayName = displayName;
            UserName = userName;
        }
    }
}