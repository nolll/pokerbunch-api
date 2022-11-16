using Core.Errors;
using Core.UseCases;
using NUnit.Framework;
using Tests.Common;

namespace Tests.Core.UseCases;

class AddPlayerTests : TestBase
{
    private const string EmptyName = "";
    private const string UniqueName = "Unique Name";
    private const string ExistingName = TestData.PlayerNameA;

    [Test]
    public void AddPlayer_ReturnUrlIsSet()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, UniqueName);
        var result = Sut.Execute(request);

        Assert.AreEqual(1, result.Data.Id);
    }

    [Test]
    public void AddPlayer_EmptyName_ReturnsError()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, EmptyName);
        var result = Sut.Execute(request);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public void AddPlayer_ValidName_AddsPlayer()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, UniqueName);
        Sut.Execute(request);

        Assert.IsNotNull(Deps.Player.Added);
    }

    [Test]
    public void AddPlayer_ValidNameButNameExists_ReturnsError()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, ExistingName);
        var result = Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
    }

    private AddPlayer Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User);
}