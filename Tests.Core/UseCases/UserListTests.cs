using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class UserListTests : TestBase
{
    [Test]
    public async Task UserList_ReturnsListOfUserItems()
    {
        var result = await Sut.Execute(new UserList.Request(new AuthInTest(canListUsers: true)));

        result.Data?.Users.Count.Should().Be(4);
        result.Data?.Users.First().DisplayName.Should().Be(TestData.UserDisplayNameA);
        result.Data?.Users.First().UserName.Should().Be("user-name-a");
    }

    private UserList Sut => new(Deps.User);
}