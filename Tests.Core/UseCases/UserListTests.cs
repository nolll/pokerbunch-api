using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class UserListTests : TestBase
{
    [Test]
    public async Task UserList_ReturnsListOfUserItems()
    {
        var result = await Sut.Execute(new UserList.Request(TestData.AdminUser.UserName));

        Assert.That(result.Data.Users.Count, Is.EqualTo(4));
        Assert.That(result.Data.Users.First().DisplayName, Is.EqualTo(TestData.UserDisplayNameA));
        Assert.That(result.Data.Users.First().UserName, Is.EqualTo("user-name-a"));
    }

    private UserList Sut => new(Deps.User);
}