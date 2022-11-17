using Core.Errors;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class AppContextTests : TestBase
{
    [Test]
    public void AppContext_WithoutUserName_AllPropertiesAreSet()
    {
        var result = Sut.Execute(new CoreContext.Request(null));

        Assert.IsFalse(result.Data.IsLoggedIn);
        Assert.IsEmpty(result.Data.UserDisplayName);
    }

    [Test]
    public void AppContext_WithUserName_LoggedInPropertiesAreSet()
    {
        var result = Sut.Execute(new CoreContext.Request(TestData.UserA.UserName));

        Assert.IsTrue(result.Data.IsLoggedIn);
        Assert.AreEqual(TestData.UserDisplayNameA, result.Data.UserDisplayName);
        Assert.AreEqual("user-name-a", result.Data.UserName);
    }

    [Test]
    public void AppContext_WithInvalidUserName_LoggedInPropertiesAreSet()
    {
        var request = new CoreContext.Request("1");
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Auth));
    }

    private CoreContext Sut => new(Deps.User);
}