using System;
using AutoFixture;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class ReportTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();

    [Fact]
    public async Task ReturnsValidationError()
    {
        const int invalidStack = -1;
        
        var result = await ExecuteAsync(false, stack: invalidStack);
        
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddsCheckpoint()
    {
        const int stack = 5;
        var cashgame = Create.Cashgame();
        _cashgameRepository.Get(cashgame.Id).Returns(Task.FromResult(cashgame));
        
        var result = await ExecuteAsync(true, cashgame.Id, stack);
        
        result.Success.Should().BeTrue();
        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o => o.AddedCheckpoints.First().Stack == stack));
    }

    private async Task<UseCaseResult<Report.Result>> ExecuteAsync(bool canEditCashgameActionsFor, string? cashgameId = null, int? stack = null)
    {
        var request = new Report.Request(
            new AuthInTest(canEditCashgameActionsFor: canEditCashgameActionsFor),
            cashgameId ?? Create.String(),
            Create.String(),
            stack ?? Create.Int(),
            Create.DateTime());
        
        return await Sut.Execute(request);
    }
    
    private Report Sut => new(_cashgameRepository);
}