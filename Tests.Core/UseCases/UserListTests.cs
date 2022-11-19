﻿using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class UserListTests : TestBase
{
    [Test]
    public async Task UserList_ReturnsListOfUserItems()
    {
        var result = await Sut.Execute(new UserList.Request(TestData.AdminUser.UserName));

        Assert.AreEqual(4, result.Data.Users.Count);
        Assert.AreEqual(TestData.UserDisplayNameA, result.Data.Users.First().DisplayName);
        Assert.AreEqual("user-name-a", result.Data.Users.First().UserName);
    }

    private UserList Sut => new(Deps.User);
}