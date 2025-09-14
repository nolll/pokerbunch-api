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
    [Fact]
    public async Task UserList_ReturnsListOfUserItems()
    {
        var fixture = new Fixture();
        var user1 = fixture.Create<User>();
        var user2 = fixture.Create<User>();
        
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.List().Returns([user1, user2]);

        var sut = new UserList(userRepository);
        
        var result = await sut.Execute(new UserList.Request(new AuthInTest(canListUsers: true)));

        result.Data?.Users.Count.Should().Be(2);
        result.Data?.Users.First().DisplayName.Should().Be(user1.DisplayName);
        result.Data?.Users.First().UserName.Should().Be(user1.UserName);
    }
}