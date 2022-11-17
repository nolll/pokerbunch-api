using Core.Errors;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditUserTests : TestBase
{
    private const string ChangedDisplayName = "changeddisplayname";
    private const string RealName = "realname";
    private const string ChangedEmail = "email@example.com";

    [Test]
    public void EditUser_EmptyDisplayName_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, "", RealName, ChangedEmail);
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public void EditUser_EmptyEmail_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, "");
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public void EditUser_InvalidEmail_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, "a");
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public void EditUser_ValidInput_ReturnUrlIsSet()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, ChangedEmail);

        var result = Sut.Execute(request);

        Assert.AreEqual("user-name-a", result.Data.UserName);
    }

    [Test]
    public void EditUser_ValidInput_UserIsSaved()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, ChangedEmail);

        Sut.Execute(request);

        Assert.AreEqual(TestData.UserNameA, Deps.User.Saved.UserName);
        Assert.AreEqual(ChangedDisplayName, Deps.User.Saved.DisplayName);
        Assert.AreEqual(ChangedEmail, Deps.User.Saved.Email);
    }

    private EditUser Sut => new(Deps.User);
}