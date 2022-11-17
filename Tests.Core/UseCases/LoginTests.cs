using Core.Errors;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class LoginTests : TestBase
{
    [Test]
    public void Login_UserNotFound_ReturnsError()
    {
        var request = new Login.Request("username-that-does-not-exist", "");
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.AccessDenied));
    }

    [Test]
    public void Login_UserFoundButPasswordIsWrong_ReturnsError()
    {
        var request = new Login.Request(TestData.UserA.UserName, "wrong password");
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.AccessDenied));
    }

    [Test]
    public void Login_UserFoundAndPasswordIsCorrect_UserNameIsSet()
    {
        var result = Sut.Execute(CreateRequest());

        Assert.AreEqual(TestData.UserA.UserName, result.Data.UserName);
    }
      
    private static Login.Request CreateRequest()
    {
        return new Login.Request(TestData.UserA.UserName, TestData.UserPasswordA);
    }

    private Login Sut => new(Deps.User);
}