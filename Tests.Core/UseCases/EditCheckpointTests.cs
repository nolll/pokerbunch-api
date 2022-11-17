using System.Linq;
using Core.Entities.Checkpoints;
using Core.Errors;
using Core.Exceptions;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

public class EditCheckpointTests : TestBase
{
    private const int ChangedStack = 1;
    private const int ChangedAmount = 2;

    [Test]
    public void EditCheckpoint_InvalidStack_ReturnsError()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, -1, ChangedAmount);
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public void EditCheckpoint_InvalidAmount_ReturnsError()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, -1);
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public void EditCheckpoint_ValidInput_ReturnUrlIsSet()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        var result = Sut.Execute(request);

        Assert.AreEqual(1, result.Data.CashgameId);
        Assert.AreEqual(1, result.Data.PlayerId);
    }
        
    [Test]
    public void EditCheckpoint_ValidInput_CheckpointIsSaved()
    {
        var request = new EditCheckpoint.Request(TestData.ManagerUser.UserName, TestData.BuyinCheckpointId, TestData.StartTimeA, ChangedStack, ChangedAmount);

        Sut.Execute(request);

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