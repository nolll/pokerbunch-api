using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

class ResetPasswordTests : TestBase
{
    private const string ValidEmail = TestData.UserEmailA;
    private const string InvalidEmail = "";
    private const string NonExistingEmail = "a@b.com";

    [Test]
    public async Task ResetPassword_WithInvalidEmail_ValidationExceptionIsThrown()
    {
        var request = CreateRequest(InvalidEmail);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task ResetPassword_UserNotFound_ReturnsError()
    {
        var result = await Sut.Execute(CreateRequest(NonExistingEmail));
        result.Error!.Type.Should().Be(ErrorType.NotFound);
    }

    [Test]
    public async Task ResetPassword_SendsPasswordEmail()
    {
        const string subject = "Poker Bunch Password Recovery";
        const string body = @"Here is your new password for Poker Bunch:
aaaaaaaa

Please sign in here: loginUrl";
        await Sut.Execute(CreateRequest());

        Deps.EmailSender.To.Should().Be(ValidEmail);
        Deps.EmailSender.Message!.Subject.Should().Be(subject);
        Deps.EmailSender.Message!.Body.Should().Be(body);
    }

    [Test]
    public async Task ResetPassword_SavesUserWithNewPassword()
    {
        await Sut.Execute(CreateRequest());

        var savedUser = Deps.User.Saved;
        savedUser!.EncryptedPassword.Should().Be("0478095c8ece0bbc11f94663ac2c4f10b29666de");
        savedUser.Salt.Should().Be("aaaaaaaaaa");
    }

    private ResetPassword.Request CreateRequest(string email = ValidEmail)
    {
        return new ResetPassword.Request(email, "loginUrl");
    }

    private ResetPassword Sut => new(
        Deps.User,
        Deps.EmailSender,
        Deps.Randomizer);
}