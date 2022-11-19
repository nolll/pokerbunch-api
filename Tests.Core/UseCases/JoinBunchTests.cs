using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class JoinBunchTests : TestBase
{
    private const string ValidCode = "d643c7857f8c3bffb1e9e7017a5448d09ef59d33";

    [Test]
    public async Task JoinBunch_EmptyCode_ReturnsError()
    {
        const string code = "";
        var request = new JoinBunch.Request(TestData.SlugA, TestData.UserNameA, code);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task JoinBunch_InvalidCode_ReturnsError()
    {
        const string code = "abc";
        var request = new JoinBunch.Request(TestData.UserNameA, TestData.SlugA, code);
        var result = await Sut.Execute(request);

        Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
    }

    [Test]
    public async Task JoinBunch_ValidCode_JoinsBunch()
    {
        var request = new JoinBunch.Request(TestData.UserNameA, TestData.SlugA, ValidCode);

        var result = await Sut.Execute(request);
        Assert.AreEqual("bunch-a", result.Data.Slug);
    }

    [Test]
    public async Task JoinBunch_ValidCode_ReturnsConfirmationUrl()
    {
        var request = new JoinBunch.Request(TestData.UserNameA, TestData.SlugA, ValidCode);

        await Sut.Execute(request);
        Assert.AreEqual(TestData.PlayerA.Id, Deps.Player.Joined.PlayerId);
        Assert.AreEqual(TestData.BunchA.Id, Deps.Player.Joined.BunchId);
        Assert.AreEqual(TestData.UserA.Id, Deps.Player.Joined.UserId);
    }

    private JoinBunch Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User);
}