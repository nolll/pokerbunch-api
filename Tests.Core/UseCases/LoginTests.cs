using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class LoginTests : TestBase
{
    [Test]
    public async Task Login_UserNotFound_ReturnsError()
    {
        var request = new Login.Request("username-that-does-not-exist", "");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.AccessDenied));
    }

    [Test]
    public async Task Login_UserFoundButPasswordIsWrong_ReturnsError()
    {
        var request = new Login.Request(TestData.UserA.UserName, "wrong password");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.AccessDenied));
    }

    [Test]
    public async Task Login_UserFoundAndPasswordIsCorrect_UserNameIsSet()
    {
        var result = await Sut.Execute(CreateRequest());

        Assert.That(result.Data?.UserName, Is.EqualTo(TestData.UserA.UserName));
    }
      
    private static Login.Request CreateRequest()
    {
        return new Login.Request(TestData.UserA.UserName, TestData.UserPasswordA);
    }

    private Login Sut => new(Deps.User, Deps.Bunch, Deps.Player);
}