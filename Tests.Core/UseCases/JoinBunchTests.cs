using Core.Entities;
using Core.Errors;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using NUnit.Framework;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class JoinBunchTests : TestBase
{
    private readonly IInvitationCodeCreator _invitationCodeCreator = Substitute.For<IInvitationCodeCreator>();
    private const string ValidCode = "abcdefghij";

    [Test]
    public async Task JoinBunch_EmptyCode_ReturnsError()
    {
        var auth = new AuthInTest(id: TestData.UserIdA, userName: TestData.UserNameA);
        const string code = "";
        
        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns(ValidCode);
        
        var request = new JoinBunch.Request(auth, TestData.SlugA, code);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task JoinBunch_InvalidCode_ReturnsError()
    {
        var auth = new AuthInTest(id: TestData.UserIdA, userName: TestData.UserNameA);
        const string code = "abc";
        
        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns(ValidCode);
        
        var request = new JoinBunch.Request(auth, TestData.SlugA, code);
        var result = await Sut.Execute(request);

        result.Error!.Type.Should().Be(ErrorType.Validation);
    }

    [Test]
    public async Task JoinBunch_ValidCode_JoinsBunch()
    {
        var auth = new AuthInTest(id: TestData.UserIdA, userName: TestData.UserNameA);
        var request = new JoinBunch.Request(auth, TestData.SlugA, ValidCode);
        
        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns(ValidCode);

        var result = await Sut.Execute(request);
        result.Data!.Slug.Should().Be("bunch-a");
    }

    [Test]
    public async Task JoinBunch_ValidCode_ReturnsConfirmationUrl()
    {
        var auth = new AuthInTest(id: TestData.UserIdA, userName: TestData.UserNameA);
        var request = new JoinBunch.Request(auth, TestData.SlugA, ValidCode);

        _invitationCodeCreator.GetCode(Arg.Any<Player>()).Returns(ValidCode);

        await Sut.Execute(request);
        Deps.Player.Joined!.PlayerId.Should().Be(TestData.PlayerA.Id);
        Deps.Player.Joined!.BunchId.Should().Be(TestData.BunchA.Id);
        Deps.Player.Joined!.UserId.Should().Be(TestData.UserA.Id);
    }

    private JoinBunch Sut => new(
        Deps.Bunch,
        Deps.Player,
        _invitationCodeCreator);
}