using Core.Entities.Checkpoints;
using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditCheckpointTests : TestBase
{
    private const int ChangedStack = 1;
    private const int ChangedAmount = 2;

    [Test]
    public async Task EditCheckpoint_InvalidStack_ReturnsError()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, -1, ChangedAmount);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditCheckpoint_InvalidAmount_ReturnsError()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, -1);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task EditCheckpoint_ValidInput_ReturnUrlIsSet()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        var result = await Sut.Execute(request);

        Assert.AreEqual(1, result.Data.CashgameId);
        Assert.AreEqual(1, result.Data.PlayerId);
    }
        
    [Test]
    public async Task EditCheckpoint_ValidInput_CheckpointIsSaved()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        await Sut.Execute(request);

        var updatedCheckpoint = Deps.Cashgame.Updated.UpdatedCheckpoints.First();
        Assert.AreEqual(CheckpointType.Buyin, updatedCheckpoint.Type);
        Assert.AreEqual(TestData.BuyinCheckpointId, updatedCheckpoint.Id);
        Assert.AreEqual(ChangedStack, updatedCheckpoint.Stack);
        Assert.AreEqual(ChangedAmount, updatedCheckpoint.Amount);
    }

    private EditCheckpoint Sut => new(
        Deps.Bunch,
        Deps.User,
        Deps.Player,
        Deps.Cashgame);
}