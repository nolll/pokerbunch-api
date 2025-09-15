using AutoFixture;
using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;
using Xunit;

namespace Tests.Core.UseCases;

public class UserListTests : TestBase
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private UserList Sut => new(_userRepository);

    [Fact]
    public async Task UserList_ReturnsListOfUserItems()
    {
        var user1 = Fixture.Create<User>();
        var user2 = Fixture.Create<User>();
        
        _userRepository.List().Returns([user1, user2]);
        
        var result = await Sut.Execute(new UserList.Request(new AuthInTest(canListUsers: true)));

        result.Data!.Users.Count.Should().Be(2);
        result.Data!.Users.First().DisplayName.Should().Be(user1.DisplayName);
        result.Data!.Users.First().UserName.Should().Be(user1.UserName);
    }
}