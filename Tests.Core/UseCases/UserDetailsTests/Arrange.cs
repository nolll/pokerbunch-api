using Core.Entities;
using Core.Repositories;
using Core.UseCases;

namespace Tests.Core.UseCases.UserDetailsTests;

public abstract class Arrange : UseCaseTest<UserDetails>
{
    protected UseCaseResult<UserDetails.Result> Result;

    private const int CurrentUserId = 1;
    private const int ViewUserId = 2;
    private string _currentUserName = "currentusername";
    protected const string ViewUserName = "viewusername";
    protected const string DisplayName = "displayname";
    protected const string RealName = "realname";
    protected const string Email = "email";
    protected virtual Role Role => Role.Player;
    protected virtual bool ViewingOwnUser => false; 
        
    protected override void Setup()
    {
        if (ViewingOwnUser)
        {
            Mock<IUserRepository>().Setup(s => s.Get(ViewUserName)).Returns(Task.FromResult(new User(ViewUserId, ViewUserName, DisplayName, RealName, Email, Role)));
            _currentUserName = ViewUserName;
        }
        else
        {
            Mock<IUserRepository>().Setup(s => s.Get(_currentUserName)).Returns(Task.FromResult(new User(CurrentUserId, _currentUserName, globalRole: Role)));
            Mock<IUserRepository>().Setup(s => s.Get(ViewUserName)).Returns(Task.FromResult(new User(ViewUserId, ViewUserName, DisplayName, RealName, Email, Role)));
        }
    }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new UserDetails.Request(_currentUserName, ViewUserName));
    }
}