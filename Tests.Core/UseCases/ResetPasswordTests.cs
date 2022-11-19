﻿using Core.Errors;
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

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task ResetPassword_UserNotFound_ReturnsError()
    {
        var result = await Sut.Execute(CreateRequest(NonExistingEmail));
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.NotFound));
    }

    [Test]
    public async Task ResetPassword_SendsPasswordEmail()
    {
        const string subject = "Poker Bunch Password Recovery";
        const string body = @"Here is your new password for Poker Bunch:
aaaaaaaa

Please sign in here: loginUrl";
        await Sut.Execute(CreateRequest());

        Assert.AreEqual(ValidEmail, Deps.EmailSender.To);
        Assert.AreEqual(subject, Deps.EmailSender.Message.Subject);
        Assert.AreEqual(body, Deps.EmailSender.Message.Body);
    }

    [Test]
    public async Task ResetPassword_SavesUserWithNewPassword()
    {
        await Sut.Execute(CreateRequest());

        var savedUser = Deps.User.Saved;
        Assert.AreEqual("0478095c8ece0bbc11f94663ac2c4f10b29666de", savedUser.EncryptedPassword);
        Assert.AreEqual("aaaaaaaaaa", savedUser.Salt);
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