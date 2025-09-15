using System;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;
using Xunit;

namespace Tests.Core.UseCases;

public class ReportTests : TestBase
{
    private const string CashgameId = "2";
    private const string PlayerId = "3";
    private const string BunchId = "4";
    private static DateTime CurrentTime => DateTime.MinValue;
    
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();

    public ReportTests()
    {
        var cashgame = new CashgameInTest(bunchId: BunchId);

        _cashgameRepository.Get(CashgameId).Returns(Task.FromResult<Cashgame>(cashgame));
    }

    [Fact]
    public async Task ReturnsValidationError()
    {
        const int invalidStack = -1;
        var request = new Report.Request(new AuthInTest(canEditCashgameActionsFor: true), CashgameId, PlayerId, invalidStack, CurrentTime);
        var result = await Sut.Execute(request);
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task AddsCheckpoint()
    {
        const int stack = 5;
        var request = new Report.Request(new AuthInTest(canEditCashgameActionsFor: true), CashgameId, PlayerId, stack, CurrentTime);
        await Sut.Execute(request);
        
        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o => o.AddedCheckpoints.First().Stack == stack));
    }
    
    private Report Sut => new(_cashgameRepository);
}