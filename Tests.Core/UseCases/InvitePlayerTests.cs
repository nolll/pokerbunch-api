using Core;
using Core.Entities;
using Core.Errors;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class InvitePlayerTests : TestBase
{
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    private readonly IInvitationCodeCreator _invitationCodeCreator = Substitute.For<IInvitationCodeCreator>();

    [Test]
    public async Task InvitePlayer_ReturnUrlIsSet()
    {
        var request = CreateRequest();
        var result = await Sut.Execute(request);

        result.Data!.PlayerId.Should().Be("1");
    }

    [TestCase("")]
    [TestCase("a")]
    public async Task InvitePlayer_InvalidEmail_ReturnsError(string email)
    {
        var request = CreateRequest(email);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task InvitePlayer_ValidEmail_SendsInvitationEmail()
    {
        const string subject = "Invitation to Poker Bunch: Bunch A";
        const string body = @"You have been invited to join the poker game: Bunch A.

Use this link to accept the invitation: https://pokerbunch.com/fakejoin/bunch-a/abcdefghij. If the link doesn't work in your email client,
use this link instead, https://pokerbunch.com/fakejoin/bunch-a, and enter this verification code: abcdefghij

If you don't have an account, you can register at https://pokerbunch.com/test";

        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns("abcdefghij");
        
        var request = CreateRequest();
        await Sut.Execute(request);

        _emailSender.Received().Send(Arg.Is<string>(o => o == TestData.UserEmailA),
            Arg.Is<IMessage>(o => o.Subject == subject && o.Body == body));
    }

    private InvitePlayer.Request CreateRequest(string email = TestData.UserEmailA)
    {
        var userBunch = Create.UserBunch(TestData.BunchA.Id, TestData.BunchA.Slug, TestData.BunchA.DisplayName, "", "", Role.None);
        return new InvitePlayer.Request(new AuthInTest(canInvitePlayer: true, userBunch: userBunch), TestData.PlayerIdA, email, TestData.TestUrl, "https://pokerbunch.com/fakejoin/{0}", "https://pokerbunch.com/fakejoin/{0}/{1}");
    }

    private InvitePlayer Sut => new(
        Deps.Player,
        _emailSender,
        _invitationCodeCreator);
}