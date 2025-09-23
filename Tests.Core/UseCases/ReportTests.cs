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
        
        var request = CreateRequest(false, stack: invalidStack);
        var result = await Sut.Execute(request);
        
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddsCheckpoint()
    {
        const int stack = 5;
        var cashgame = Create.Cashgame();
        _cashgameRepository.Get(cashgame.Id).Returns(Task.FromResult(cashgame));

        var request = CreateRequest(true, cashgame.Id, stack);
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeTrue();
        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o => o.AddedCheckpoints.First().Stack == stack));
    }

    private Report.Request CreateRequest(bool canEditCashgameActionsFor, string? cashgameId = null, int? stack = null) => new(
        new AuthInTest(canEditCashgameActionsFor: canEditCashgameActionsFor),
        cashgameId ?? Create.String(),
        Create.String(),
        stack ?? Create.Int(),
        Create.DateTime());

    private Report Sut => new(_cashgameRepository);
}