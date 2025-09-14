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

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_EmptyEmail_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, "");
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_InvalidEmail_ReturnsError()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, "a");
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditUser_ValidInput_ReturnUrlIsSet()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, ChangedEmail);
        var result = await Sut.Execute(request);

        result.Data?.UserName.Should().Be("user-name-a");
    }

    [Test]
    public async Task EditUser_ValidInput_UserIsSaved()
    {
        var request = new EditUser.Request(TestData.UserNameA, ChangedDisplayName, RealName, ChangedEmail);

        await Sut.Execute(request);

        Deps.User.Saved?.UserName.Should().Be(TestData.UserNameA);
        Deps.User.Saved?.DisplayName.Should().Be(ChangedDisplayName);
        Deps.User.Saved?.Email.Should().Be(ChangedEmail);
    }

    private EditUser Sut => new(Deps.User);
}