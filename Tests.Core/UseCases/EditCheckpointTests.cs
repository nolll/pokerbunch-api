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
        var request = new EditCheckpoint.Request(new PrincipalInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, -1, ChangedAmount);
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditCheckpoint_InvalidAmount_ReturnsError()
    {
        var request = new EditCheckpoint.Request(new PrincipalInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, -1);
        var result = await Sut.Execute(request);

        Assert.That(result.Error?.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditCheckpoint_ValidInput_ReturnUrlIsSet()
    {
        var request = new EditCheckpoint.Request(new PrincipalInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        var result = await Sut.Execute(request);

        Assert.That(result.Data?.CashgameId, Is.EqualTo("1"));
        Assert.That(result.Data?.PlayerId, Is.EqualTo("1"));
    }
        
    [Test]
    public async Task EditCheckpoint_ValidInput_CheckpointIsSaved()
    {
        var request = new EditCheckpoint.Request(new PrincipalInTest(canEditCashgameAction: true), TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        await Sut.Execute(request);

        var updatedCheckpoint = Deps.Cashgame.Updated?.UpdatedCheckpoints.First();
        Assert.That(updatedCheckpoint?.Type, Is.EqualTo(CheckpointType.Buyin));
        Assert.That(updatedCheckpoint?.Id, Is.EqualTo(TestData.BuyinCheckpointId));
        Assert.That(updatedCheckpoint?.Stack, Is.EqualTo(ChangedStack));
        Assert.That(updatedCheckpoint?.Amount, Is.EqualTo(ChangedAmount));
    }

    private EditCheckpoint Sut => new(Deps.Cashgame);
}