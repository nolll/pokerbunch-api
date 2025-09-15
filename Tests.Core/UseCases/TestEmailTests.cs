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

    // todo: Move email to config
    private const string Email = "henriks@gmail.com";
    
    [Fact]
    public async Task HasAccess_EmailIsSet()
    {
        var result = await ExecuteAsync(true);
        result.Success.Should().BeTrue();
        result.Data!.Email.Should().Be(Email);
    }

    [Fact]
    public async Task NoAccess_ReturnsError()
    {
        var result = await ExecuteAsync(false);
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    private async Task<UseCaseResult<TestEmail.Result>> ExecuteAsync(bool canSendTestEmail) => 
        await Sut.Execute(new TestEmail.Request(new AuthInTest(canSendTestEmail: canSendTestEmail)));

    private TestEmail Sut => new(_emailSender);
}