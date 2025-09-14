using Core.Errors;
using Core.UseCases;
using Tests.Common;

namespace Tests.Core.UseCases;

public class JoinBunchTests : TestBase
{
    private const string ValidCode = "abcdefghij";

    [Test]
    public async Task JoinBunch_EmptyCode_ReturnsError()
    {
        const string code = "";
        var request = new JoinBunch.Request(TestData.SlugA, TestData.UserNameA, code);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task JoinBunch_InvalidCode_ReturnsError()
    {
        const string code = "abc";
        var request = new JoinBunch.Request(TestData.UserNameA, TestData.SlugA, code);
        var result = await Sut.Execute(request);

        result.Error?.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task JoinBunch_ValidCode_JoinsBunch()
    {
        var request = new JoinBunch.Request(TestData.UserNameA, TestData.SlugA, ValidCode);

        var result = await Sut.Execute(request);
        result.Data?.Slug.Should().Be("bunch-a");
    }

    [Test]
    public async Task JoinBunch_ValidCode_ReturnsConfirmationUrl()
    {
        var request = new JoinBunch.Request(TestData.UserNameA, TestData.SlugA, ValidCode);

        await Sut.Execute(request);
        Deps.Player.Joined?.PlayerId.Should().Be(TestData.PlayerA.Id);
        Deps.Player.Joined?.BunchId.Should().Be(TestData.BunchA.Id);
        Deps.Player.Joined?.UserId.Should().Be(TestData.UserA.Id);
    }

    private JoinBunch Sut => new(
        Deps.Bunch,
        Deps.Player,
        Deps.User,
        Deps.InvitationCodeCreator);
}