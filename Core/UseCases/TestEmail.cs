using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class TestEmail(IEmailSender emailSender)
    : UseCase<TestEmail.Request, TestEmail.Result>
{
    protected override Task<UseCaseResult<Result>> Work(Request request)
    {
        if (!AccessControl.CanSendTestEmail(request.CurrentUser))
            return Task.FromResult(Error(new AccessDeniedError()));

        // todo: Move email to config
        const string email = "henriks@gmail.com";
        var message = new TestMessage();
        emailSender.Send(email, message);

        return Task.FromResult(Success(new Result(email)));
    }
    
    public class Request(CurrentUser currentUser)
    {
        public CurrentUser CurrentUser { get; } = currentUser;
    }

    public class Result(string email)
    {
        public string Email { get; } = email;
        public string Message { get; } = $"An email was sent to {email}";
    }

    private class TestMessage : IMessage
    {
        public string Subject => "Test Email";
        public string Body => "This is a test email from pokerbunch.com";
    }
}