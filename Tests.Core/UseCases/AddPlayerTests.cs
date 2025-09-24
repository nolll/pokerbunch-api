using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class AddPlayerTests : TestBase
{
    private readonly IPlayerRepository _playerRepository = Substitute.For<IPlayerRepository>();

    [Fact]
    public async Task AddPlayer_EmptyName_ReturnsError()
    {
        var userBunch = Create.UserBunch();
        
        var request = CreateRequest(userBunch, "");
        var result = await Sut.Execute(request);
        
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }
    
    [Fact]
    public async Task AddPlayer_ValidNameButNameExists_ReturnsError()
    {
        var existingPlayer = Create.Player();
        var userBunch = Create.UserBunch();
        _playerRepository.List(userBunch.Slug).Returns([existingPlayer]);
        
        var request = CreateRequest(userBunch, existingPlayer.DisplayName);
        var result = await Sut.Execute(request);
        
        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task AddPlayer_ValidName_AddsPlayer()
    {
        var uniqueName = Create.String();
        var userBunch = Create.UserBunch();
        
        var request = CreateRequest(userBunch, uniqueName);
        await Sut.Execute(request);
        
        await _playerRepository.Received().Add(Arg.Is<Player>(o => o.DisplayName == uniqueName));
    }

    private static AddPlayer.Request CreateRequest(UserBunch userBunch, string uniqueName)
    {
        return new AddPlayer.Request(
            new AuthInTest(canAddPlayer: true, userBunch: userBunch), userBunch.Slug, uniqueName);
    }

    private AddPlayer Sut => new(_playerRepository);
}