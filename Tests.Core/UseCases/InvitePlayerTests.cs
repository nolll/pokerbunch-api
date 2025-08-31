using Core.Entities;
using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class InvitePlayerTests : TestBase
{
    [Test]
    public async Task InvitePlayer_ReturnUrlIsSet()
    {
        var request = CreateRequest();
        var result = await Sut.Execute(request);

        Assert.That(result.Data?.PlayerId, Is.EqualTo("1"));
    }

    [TestCase("")]
    [TestCase("a")]
    public async Task InvitePlayer_InvalidEmail_ReturnsError(string email)
    {
        var request = CreateRequest(email);
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task InvitePlayer_ValidEmail_SendsInvitationEmail()
    {
        const string subject = "Invitation to Poker Bunch: Bunch A";
        const string body = @"You have been invited to join the poker game: Bunch A.

Use this link to accept the invitation: https://pokerbunch.com/fakejoin/bunch-a/abcdefghij. If the link doesn't work in your email client,
use this link instead, https://pokerbunch.com/fakejoin/bunch-a, and enter this verification code: abcdefghij

If you don't have an account, you can register at https://pokerbunch.com/test";
        var request = CreateRequest();

        await Sut.Execute(request);

        Assert.That(Deps.EmailSender.To, Is.EqualTo(TestData.UserEmailA));
        Assert.That(Deps.EmailSender.Message?.Subject, Is.EqualTo(subject));
        Assert.That(Deps.EmailSender.Message?.Body, Is.EqualTo(body));
    }

    private static InvitePlayer.Request CreateRequest(string email = TestData.UserEmailA)
    {
        var currentBunch = new CurrentBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName, "", "", Role.None);
        return new InvitePlayer.Request(new PrincipalInTest(canInvitePlayer: true, currentBunch: currentBunch), TestData.PlayerIdA, email, TestData.TestUrl, "https://pokerbunch.com/fakejoin/{0}", "https://pokerbunch.com/fakejoin/{0}/{1}");
    }

    private InvitePlayer Sut => new(
        Deps.Player,
        Deps.EmailSender,
        Deps.InvitationCodeCreator);
}