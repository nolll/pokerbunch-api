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

        Assert.That(result.Data.UserName, Is.EqualTo("user-name-a"));
    }

    [Test]
    public async Task EditUser_ValidInput_UserIsSaved()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, ChangedEmail);

        await Sut.Execute(request);

        Assert.That(Deps.User.Saved?.UserName, Is.EqualTo(TestData.UserNameA));
        Assert.That(Deps.User.Saved?.DisplayName, Is.EqualTo(ChangedDisplayName));
        Assert.That(Deps.User.Saved?.Email, Is.EqualTo(ChangedEmail));
    }

    private EditUser Sut => new(Deps.User);
}