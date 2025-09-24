using System;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Repositories;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EditCheckpointTests : TestBase
{
    private readonly ICashgameRepository _cashgameRepository = Substitute.For<ICashgameRepository>();

    [Fact]
    public async Task EditCheckpoint_NoAccess_ReturnsError()
    {
        var cashgame = Create.Cashgame();
        var action = Create.BuyinAction(cashgameId: cashgame.Id);
        cashgame.SetCheckpoints([action]);
        _cashgameRepository.GetByCheckpoint(action.Id).Returns(cashgame);
        
        var request = CreateRequest(actionId: action.Id, canEditCashgameAction: false);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }
    
    [Fact]
    public async Task EditCheckpoint_InvalidStack_ReturnsError()
    {
        var request = CreateRequest(stack: -1);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task EditCheckpoint_InvalidAdded_ReturnsError()
    {
        var request = CreateRequest(added: -1);
        var result = await Sut.Execute(request);

        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.Validation);
    }
        
    [Fact]
    public async Task EditCheckpoint_ValidInput_CheckpointIsSaved()
    {
        var cashgame = Create.Cashgame();
        var action = Create.BuyinAction(cashgameId: cashgame.Id);
        cashgame.SetCheckpoints([action]);
        _cashgameRepository.GetByCheckpoint(action.Id).Returns(cashgame);
        var stack = Create.Int();
        var added = Create.Int();
        
        var request = CreateRequest(actionId: action.Id, stack: stack, added: added);
        var result = await Sut.Execute(request);

        await _cashgameRepository.Received().Update(Arg.Is<Cashgame>(o =>
            o.UpdatedCheckpoints.First().Id == action.Id && 
            o.UpdatedCheckpoints.First().Type == CheckpointType.Buyin &&
            o.UpdatedCheckpoints.First().Stack == stack && 
            o.UpdatedCheckpoints.First().Amount == added));
        
        result.Success.Should().BeTrue();
        result.Data!.CashgameId.Should().Be(cashgame.Id);
        result.Data!.PlayerId.Should().Be(action.PlayerId);
    }
    
    private EditCheckpoint.Request CreateRequest(
        bool? canEditCashgameAction = null,
        string? actionId = null,
        DateTime? timestamp = null,
        int? stack = null,
        int? added = null) =>
        new(
            new AuthInTest(canEditCashgameAction: canEditCashgameAction ?? true),
            actionId ?? Create.String(),
            timestamp ?? Create.DateTime(),
            stack ?? Create.Int(),
            added ?? Create.Int());

    private EditCheckpoint Sut => new(_cashgameRepository);
}