using Core;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
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
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();

    [Fact]
    public async Task InvitePlayer_NoAccess_ReturnsError()
    {
        var player = Create.Player();
        _playerRepository.Get(player.Id).Returns(player);
        
        var request = CreateRequest(playerId: player.Id, canInvitePlayer: false);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("a")]
    public async Task InvitePlayer_InvalidEmail_ReturnsError(string email)
    {
        var request = CreateRequest(email: email);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task InvitePlayer_ValidEmail_SendsInvitationEmail()
    {
        const string subject = "Invitation to Poker Bunch: Bunch A";
        const string body = """
                            You have been invited to join the poker game: Bunch A.

                            Use this link to accept the invitation: https://pokerbunch.com/fakejoin/bunch-a/abcdefghij. If the link doesn't work in your email client,
                            use this link instead, https://pokerbunch.com/fakejoin/bunch-a, and enter this verification code: abcdefghij

                            If you don't have an account, you can register at https://pokerbunch.com/fakeregister
                            """;

        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns("abcdefghij");

        var bunch = Create.Bunch(slug: "bunch-a", displayName: "Bunch A");
        var player = Create.Player(slug: bunch.Slug, bunchId: bunch.Id);
        _playerRepository.Get(player.Id).Returns(player);
        var email = Create.EmailAddress();
        
        var request = CreateRequest(email: email, bunchId: bunch.Id, slug: bunch.Slug, bunchName: bunch.DisplayName, playerId: player.Id);
        var result = await Sut.Execute(request);
        
        _emailSender.Received().Send(Arg.Is<string>(o => o == email),
            Arg.Is<IMessage>(o => o.Subject == subject && o.Body == body));
        
        result.Success.Should().BeTrue();
        result.Data!.PlayerId.Should().Be(player.Id);
    }

    private InvitePlayer.Request CreateRequest(
        string? bunchId = null,
        string? slug = null,
        string? bunchName = null,
        string? email = null,
        string? playerId = null,
        bool? canInvitePlayer = null)
    {
        var userBunch = Create.UserBunch(bunchId, slug, bunchName);
        return new InvitePlayer.Request(
            new AuthInTest(canInvitePlayer: canInvitePlayer ?? true, userBunch: userBunch),
            playerId ?? Create.String(), 
            email ?? Create.EmailAddress(), 
            "https://pokerbunch.com/fakeregister", 
            "https://pokerbunch.com/fakejoin/{0}", 
            "https://pokerbunch.com/fakejoin/{0}/{1}");
    }

    private InvitePlayer Sut => new(
        _playerRepository,
        _emailSender,
        _invitationCodeCreator);
}