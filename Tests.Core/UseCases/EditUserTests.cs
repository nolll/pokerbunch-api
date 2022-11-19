using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditUserTests : TestBase
{
    private const string ChangedDisplayName = "changeddisplayname";
    private const string RealName = "realname";
    private const string ChangedEmail = "email@example.com";

    [Test]
    public async Task EditUser_EmptyDisplayName_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, "", RealName, ChangedEmail);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditUser_EmptyEmail_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, "");
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditUser_InvalidEmail_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, "a");
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditUser_ValidInput_ReturnUrlIsSet()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, ChangedEmail);
        var result = await Sut.Execute(request);

        Assert.AreEqual("user-name-a", result.Data.UserName);
    }

    [Test]
    public async Task EditUser_ValidInput_UserIsSaved()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, ChangedEmail);

        await Sut.Execute(request);

        Assert.AreEqual(TestData.UserNameA, Deps.User.Saved.UserName);
        Assert.AreEqual(ChangedDisplayName, Deps.User.Saved.DisplayName);
        Assert.AreEqual(ChangedEmail, Deps.User.Saved.Email);
    }

    private EditUser Sut => new(Deps.User);
}