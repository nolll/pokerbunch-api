using System;
using System.Collections.Generic;
using AutoFixture;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class DeleteCashgameTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();
    
    [Fact]
    public async Task DeleteCashgame_GameHasResults_ReturnsError()
    {
        var cashgameId = Create.String();
        var cashgame = Create.Cashgame(id: cashgameId);
        cashgame.SetCheckpoints([new BuyinCheckpoint(cashgameId, "", DateTime.Now, 0, 200, "")]);
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);
        
        var result = await ExecuteAsync(cashgame.Id);

        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }
    
    [Fact]
    public async Task DeleteCashgame_GameIsPartOfEvent_ReturnsError()
    {
        var cashgame = Create.Cashgame(eventId: Create.String());
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);
        
        var result = await ExecuteAsync(cashgame.Id);

        result.Error!.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task DeleteCashgame_GameHasNoResults_DeletesGame()
    {
        var cashgame = Create.Cashgame();
        _cashgameRepository.Get(cashgame.Id).Returns(cashgame);

        var result = await ExecuteAsync(cashgame.Id);

        await _cashgameRepository.Received().DeleteGame(cashgame.Id);
        result.Success.Should().BeTrue();
    }

    private async Task<UseCaseResult<DeleteCashgame.Result>> ExecuteAsync(string? cashgameId = null)
    {
        var request = new DeleteCashgame.Request(
            new AuthInTest(canDeleteCashgame: true),
            cashgameId ?? Create.String());
        
        return await Sut.Execute(request);
    }

    private DeleteCashgame Sut => new(_cashgameRepository);
}