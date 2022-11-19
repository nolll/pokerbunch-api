using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

class AddPlayerTests : TestBase
{
    private const string EmptyName = "";
    private const string UniqueName = "Unique Name";
    private const string ExistingName = TestData.PlayerNameA;

    [Test]
    public async Task AddPlayer_ReturnUrlIsSet()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, UniqueName);
        var result = await Sut.Execute(request);

        Assert.AreEqual(1, result.Data.Id);
    }

    [Test]
    public async Task AddPlayer_EmptyName_ReturnsError()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, EmptyName);
        var result = await Sut.Execute(request);
        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task AddPlayer_ValidName_AddsPlayer()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, UniqueName);
        await Sut.Execute(request);

        Assert.IsNotNull(Deps.Player.Added);
    }

    [Test]
    public async Task AddPlayer_ValidNameButNameExists_ReturnsError()
    {
        var request = new AddPlayer.Request(TestData.ManagerUser.UserName, TestData.SlugA, ExistingName);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Conflict));
    }

    private AddPlayer Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User);
}