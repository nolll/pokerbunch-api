using Core;
using Core.Entities;
using Core.Errors;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;

namespace Tests.Core.UseCases;

public class AddUserTests : TestBase
{
    private const string ValidUserName = "a";
    private const string ValidDisplayName = "b";
    private const string ValidEmail = "a@b.com";
    private const string ValidPassword = "c";
    private readonly string _existingUserName = TestData.UserA.UserName;
    private readonly string _existingEmail = TestData.UserA.Email;

    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    private readonly IRandomizer _randomizer = Substitute.For<IRandomizer>();

    [Test]
    public async Task AddUser_WithEmptyUserName_ReturnsError()
    {
        var request = new AddUser.Request("", ValidDisplayName, ValidEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddUser_WithEmptyDisplayName_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, "", ValidEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddUser_WithEmptyEmail_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, ValidDisplayName, "", ValidPassword, "/");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddUser_WithEmptyPAssword_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, "", "/");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task AddUser_UserNameAlreadyInUse_ReturnsError()
    {
        var request = new AddUser.Request(_existingUserName, ValidDisplayName, ValidEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Test]
    public async Task AddUser_EmailAlreadyInUse_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, ValidDisplayName, _existingEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Test]
    public async Task AddUser_WithValidInput_UserWithCorrectPropertiesIsAdded()
    {
        const string expectedEncryptedPassword = "1cb313748ba4b822b78fe05de42558539efd9156";
        const string expectedSalt = "aaaaaaaaaa";

        _randomizer.GetAllowedChars().Returns("a");
        
        var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, ValidPassword, "/");
        await Sut.Execute(request);

        var user = Deps.User.Added;

        user!.Id.Should().Be("");
        user.UserName.Should().Be(ValidUserName);
        user.DisplayName.Should().Be(ValidDisplayName);
        user.RealName.Should().Be("");
        user.Email.Should().Be(ValidEmail);
        user.GlobalRole.Should().Be(Role.Player);
        user.EncryptedPassword.Should().Be(expectedEncryptedPassword);
        user.Salt.Should().Be(expectedSalt);
    }

    [Test]
    public async Task AddUser_WithValidInput_SendsRegistrationEmail()
    {
        const string subject = "Poker Bunch Registration";
        const string body = @"Thanks for registering with Poker Bunch.

Please sign in here: /loginUrl";

        var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, ValidPassword, "/loginUrl");
        await Sut.Execute(request);

        _emailSender.Received().Send(Arg.Is<string>(o => o == ValidEmail),
            Arg.Is<IMessage>(o => o.Subject == subject && o.Body == body));
    }

    private AddUser Sut => new(
        Deps.User,
        _randomizer,
        _emailSender);
}