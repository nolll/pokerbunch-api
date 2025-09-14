using Core.Entities.Checkpoints;
using Core.Errors;
using Core.UseCases;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class EditCheckpointTests : TestBase
{
    private const int ChangedStack = 1;
    private const int ChangedAmount = 2;

    [Test]
    public async Task EditCheckpoint_InvalidStack_ReturnsError()
    {
        var request = new EditCheckpoint.Request(new AuthInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, -1, ChangedAmount);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditCheckpoint_InvalidAmount_ReturnsError()
    {
        var request = new EditCheckpoint.Request(new AuthInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, -1);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task EditCheckpoint_ValidInput_ReturnUrlIsSet()
    {
        var request = new EditCheckpoint.Request(new AuthInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        var result = await Sut.Execute(request);

        result.Data?.CashgameId.Should().Be("1");
        result.Data?.PlayerId.Should().Be("1");
    }
        
    [Test]
    public async Task EditCheckpoint_ValidInput_CheckpointIsSaved()
    {
        var request = new EditCheckpoint.Request(new AuthInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        await Sut.Execute(request);

        var updatedCheckpoint = Deps.Cashgame.Updated?.UpdatedCheckpoints.First();
        updatedCheckpoint?.Type.Should().Be(CheckpointType.Buyin);
        updatedCheckpoint?.Id.Should().Be(TestData.BuyinCheckpointId);
        updatedCheckpoint?.Stack.Should().Be(ChangedStack);
        updatedCheckpoint?.Amount.Should().Be(ChangedAmount);
    }

    private EditCheckpoint Sut => new(Deps.Cashgame);
}