using AutoFixture;
using Core.Entities;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class UserListTests : TestBase
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private UserList Sut => new(_userRepository);

    [Fact]
    public async Task UserList_ReturnsListOfUserItems()
    {
        var user1 = Create.User();
        var user2 = Create.User();
        
        _userRepository.List().Returns([user1, user2]);

        var request = CreateRequest();
        var result = await Sut.Execute(request);

        result.Data!.Users.Count.Should().Be(2);
        result.Data!.Users.First().DisplayName.Should().Be(user1.DisplayName);
        result.Data!.Users.First().UserName.Should().Be(user1.UserName);
    }

    private UserList.Request CreateRequest()
    {
        return new UserList.Request(new AuthInTest(canListUsers: true));
    }
}