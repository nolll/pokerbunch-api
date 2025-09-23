using Core;
using Core.Errors;
using Core.Services;
using Core.UseCases;
using NSubstitute;
using Tests.Common;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases;

public class TestEmailTests : TestBase
{
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    
    [Fact]
    public async Task HasAccess_EmailIsSent()
    {
        var to = Create.EmailAddress();
        
        var request = CreateRequest(to);
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeTrue();
        result.Data!.Email.Should().Be(to);
        
        _emailSender.Received().Send(Arg.Is(to), Arg.Any<IMessage>());
    }

    [Fact]
    public async Task NoAccess_ReturnsError()
    {
        var to = Create.EmailAddress();
        
        var request = CreateRequest(to, false);
        var result = await Sut.Execute(request);
        
        result.Success.Should().BeFalse();
        result.Error!.Type.Should().Be(ErrorType.AccessDenied);
    }

    private TestEmail.Request CreateRequest(string to, bool? canSendTestEmail = null) => 
        new(new AuthInTest(canSendTestEmail: canSendTestEmail ?? true), to);

    private TestEmail Sut => new(_emailSender);
}