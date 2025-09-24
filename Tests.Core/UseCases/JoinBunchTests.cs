using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class JoinBunchTests : TestBase
{
    private readonly IInvitationCodeCreator _invitationCodeCreator = Substitute.For<IInvitationCodeCreator>();
    private readonly IBunchRepository _bunchRepository = Substitute.For<IBunchRepository>();
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();

    [Fact]
    public async Task JoinBunch_EmptyCode_ReturnsError()
    {
        var request = CreateRequest(code: "");
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task JoinBunch_InvalidCode_ReturnsError()
    {
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);
        var player = Create.Player(bunchId: bunch.Id);
        _playerRepository.List(bunch.Slug).Returns([player]);
        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns(Create.String());
        
        var request = CreateRequest(slug: bunch.Slug, code: Create.String());
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task JoinBunch_ValidCode_JoinsBunch()
    {
        var user = Create.User();
        var bunch = Create.Bunch();
        _bunchRepository.GetBySlug(bunch.Slug).Returns(bunch);
        var player = Create.Player(bunchId: bunch.Id);
        _playerRepository.List(bunch.Slug).Returns([player]);
        var code = Create.String();
        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns(code);

        var request = CreateRequest(userId: user.Id, slug: bunch.Slug, code: code);
        var result = await Sut.Execute(request);
        
        await _playerRepository.Received().JoinBunch(
            Arg.Is<Player>(o => o.Id == player.Id),
            Arg.Is<Bunch>(o => o.Id == bunch.Id),
            Arg.Is<string>(o => o == user.Id));
        result.Data!.Slug.Should().Be(bunch.Slug);
    }

    private JoinBunch.Request CreateRequest(string? userId = null, string? slug = null, string? code = null)
    {
        var auth = new AuthInTest(id: userId ?? Create.String(), userName: Create.String());
        return new JoinBunch.Request(
            auth, 
            slug ?? Create.String(), 
            code ?? Create.String());
    }

    private JoinBunch Sut => new(
        _bunchRepository,
        _playerRepository,
        _invitationCodeCreator);
}