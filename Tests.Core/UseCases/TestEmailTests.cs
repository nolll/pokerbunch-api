using Core;
using Core.Errors;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;
using Xunit;

namespace Tests.Core.UseCases;

public class TestEmailTests : TestBase
{
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    
    [Fact]
    public async Task HasAccess_EmailIsSent()
    {
        var to = CreateEmailAddress();
        var result = await ExecuteAsync(true, to);
        result.Success.Should().BeTrue();
        result.Data!.Email.Should().Be(to);
        
        _emailSender.Received().Send(Arg.Is(to), Arg.Any<IMessage>());
    }

    [Fact]
    public async Task NoAccess_ReturnsError()
    {
        var result = await ExecuteAsync(false, CreateEmailAddress());
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    private async Task<UseCaseResult<TestEmail.Result>> ExecuteAsync(bool canSendTestEmail, string to) => 
        await Sut.Execute(new TestEmail.Request(new AuthInTest(canSendTestEmail: canSendTestEmail), to));

    private TestEmail Sut => new(_emailSender);
}