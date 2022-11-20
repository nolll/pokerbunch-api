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
        Assert.That(result.Data.Slug, Is.EqualTo("bunch-a"));
    }

    [Test]
    public async Task JoinBunch_ValidCode_ReturnsConfirmationUrl()
    {
        var request = new JoinBunch.Request(TestData.UserNameA, TestData.SlugA, ValidCode);

        await Sut.Execute(request);
        Assert.That(Deps.Player.Joined.PlayerId, Is.EqualTo(TestData.PlayerA.Id));
        Assert.That(Deps.Player.Joined.BunchId, Is.EqualTo(TestData.BunchA.Id));
        Assert.That(Deps.Player.Joined.UserId, Is.EqualTo(TestData.UserA.Id));
    }

    private JoinBunch Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User);
}