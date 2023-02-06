using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Moq;
using Tests.Core.TestClasses;

namespace Tests.Core.UseCases.TestEmailTests;

public abstract class Arrange : UseCaseTest<TestEmail>
{
    protected UseCaseResult<TestEmail.Result>? Result;

    protected abstract Role Role { get; }
    protected string? To;
    protected string? Subject;
    protected string? Body;

    private const string UserName = "user-name-1";

    // todo: Move email to config
    protected readonly string Email = "henriks@gmail.com";

    protected override void Setup()
    {
        To = null;
        Subject = null;
        Body = null;

        var user = new UserInTest(globalRole: Role);

        Mock<IUserRepository>().Setup(o => o.GetByUserName(UserName)).Returns(Task.FromResult<User>(user));
        Mock<IEmailSender>().Setup(o => o.Send(It.IsAny<string>(), It.IsAny<IMessage>()))
            .Callback((string to, IMessage message) => { To = to; Subject = message.Subject; Body = message.Body; });
    }

    protected override async Task ExecuteAsync()
    {
        Result = await Sut.Execute(new TestEmail.Request(UserName));
    }
}