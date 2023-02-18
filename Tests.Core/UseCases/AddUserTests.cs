using Core.Entities;
using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

class AddUserTests : TestBase
{
    private const string ValidUserName = "a";
    private const string ValidDisplayName = "b";
    private const string ValidEmail = "a@b.com";
    private const string ValidPassword = "c";
    private readonly string _existingUserName = TestData.UserA.UserName;
    private readonly string _existingEmail = TestData.UserA.Email;
        
    [Test]
    public async Task AddUser_WithEmptyUserName_ReturnsError()
    {
        var request = new AddUser.Request("", ValidDisplayName, ValidEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddUser_WithEmptyDisplayName_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, "", ValidEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddUser_WithEmptyEmail_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, ValidDisplayName, "", ValidPassword, "/");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddUser_WithEmptyPAssword_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, "", "/");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddUser_UserNameAlreadyInUse_ReturnsError()
    {
        var request = new AddUser.Request(_existingUserName, ValidDisplayName, ValidEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Conflict));
    }

    [Test]
    public async Task AddUser_EmailAlreadyInUse_ReturnsError()
    {
        var request = new AddUser.Request(ValidUserName, ValidDisplayName, _existingEmail, ValidPassword, "/");
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Conflict));
    }

    [Test]
    public async Task AddUser_WithValidInput_UserWithCorrectPropertiesIsAdded()
    {
        const string expectedEncryptedPassword = "1cb313748ba4b822b78fe05de42558539efd9156";
        const string expectedSalt = "aaaaaaaaaa";

        var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, ValidPassword, "/");
        await Sut.Execute(request);

        var user = Deps.User.Added;

        Assert.That(user?.Id, Is.EqualTo(""));
        Assert.That(user?.UserName, Is.EqualTo(ValidUserName));
        Assert.That(user?.DisplayName, Is.EqualTo(ValidDisplayName));
        Assert.That(user?.RealName, Is.EqualTo(""));
        Assert.That(user?.Email, Is.EqualTo(ValidEmail));
        Assert.That(user?.GlobalRole, Is.EqualTo(Role.Player));
        Assert.That(user?.EncryptedPassword, Is.EqualTo(expectedEncryptedPassword));
        Assert.That(user?.Salt, Is.EqualTo(expectedSalt));
    }

    [Test]
    public async Task AddUser_WithValidInput_SendsRegistrationEmail()
    {
        const string subject = "Poker Bunch Registration";
        const string body = @"Thanks for registering with Poker Bunch.

Please sign in here: /loginUrl";

        var request = new AddUser.Request(ValidUserName, ValidDisplayName, ValidEmail, ValidPassword, "/loginUrl");
        await Sut.Execute(request);

        Assert.That(Deps.EmailSender.To, Is.EqualTo(ValidEmail));
        Assert.That(Deps.EmailSender.Message?.Subject, Is.EqualTo(subject));
        Assert.That(Deps.EmailSender.Message?.Body, Is.EqualTo(body));
    }

    private AddUser Sut => new(
        Deps.User,
        Deps.Randomizer,
        Deps.EmailSender);
}