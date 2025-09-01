using Core;
using Core.Services;
using Core.UseCases;
using Moq;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.TestEmailTests;

public abstract class Arrange : UseCaseTest<TestEmail>
{
    protected UseCaseResult<TestEmail.Result>? Result;

    protected abstract bool CanSendTestEmail { get; }
    
    protected string? To;
    protected string? Subject;
    protected string? Body;

    // todo: Move email to config
    protected readonly string Email = "henriks@gmail.com";

    protected override void Setup()
    {
        To = null;
        Subject = null;
        Body = null;

        Mock<IEmailSender>().Setup(o => o.Send(It.IsAny<string>(), It.IsAny<IMessage>()))
            .Callback((string to, IMessage message) => { To = to; Subject = message.Subject; Body = message.Body; });
    }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new TestEmail.Request(new PrincipalInTest(canSendTestEmail: CanSendTestEmail)));
    }
}