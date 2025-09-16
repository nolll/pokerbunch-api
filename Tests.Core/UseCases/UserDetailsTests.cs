using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;
using Xunit;

namespace Tests.Core.UseCases;

public class UserDetailsTests : TestBase
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    
    [Fact]
    public async Task StandardUser_UserIsReturned()
    {
        var currentUser = CreateUser();
        var viewUser = CreateUser(email: "hello@example.org");

        _userRepository.GetByUserName(viewUser.UserName).Returns(viewUser);
        
        var result = await ExecuteAsync(currentUser.UserName, false, viewUser.UserName);
        result.Success.Should().BeTrue();
        result.Data!.UserName.Should().Be(viewUser.UserName);
        result.Data!.DisplayName.Should().Be(viewUser.DisplayName);
        result.Data!.RealName.Should().Be(viewUser.RealName);
        result.Data!.Email.Should().Be(viewUser.Email);
        result.Data!.CanViewAll.Should().BeFalse();
        result.Data!.AvatarUrl.Should().Be("https://gravatar.com/avatar/76d6c4d514b53a7aa38b21e5d71f5ed1?s=100&d=blank");
    }

    [Fact]
    public async Task IsAdmin_CanViewAllIsTrue()
    {
        var currentUser = CreateUser();
        var viewUser = CreateUser();
        
        _userRepository.GetByUserName(viewUser.UserName).Returns(viewUser);
        
        var result = await ExecuteAsync(currentUser.UserName, true, viewUser.UserName);
        result.Success.Should().BeTrue();
        result.Data!.CanViewAll.Should().BeTrue();
    }

    [Fact]
    public async Task OwnUser_CanViewAllIsTrue()
    {
        var currentUser = CreateUser();
        
        _userRepository.GetByUserName(currentUser.UserName).Returns(currentUser);
        
        var result = await ExecuteAsync(currentUser.UserName, false);
        result.Success.Should().BeTrue();
        result.Data!.CanViewAll.Should().BeTrue();
    }

    private async Task<UseCaseResult<UserDetails.Result>> ExecuteAsync(
        string currentUserName, 
        bool canViewAllData,
        string? viewUserName = null)
    {
        var auth = new AuthInTest(userName: currentUserName, canViewFullUserData: canViewAllData);
        return await Sut.Execute(new UserDetails.Request(auth, viewUserName));
    }

    private UserDetails Sut => new(_userRepository);
}