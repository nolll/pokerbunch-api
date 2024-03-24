using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class TestEmail(IEmailSender emailSender, IUserRepository userRepository)
    : UseCase<TestEmail.Request, TestEmail.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await userRepository.GetByUserName(request.UserName);
        if (!AccessControl.CanSendTestEmail(user))
            return Error(new AccessDeniedError());

        // todo: Move email to config
        const string email = "henriks@gmail.com";
        var message = new TestMessage();
        emailSender.Send(email, message);

        return Success(new Result(email));
    }
    
    public class Request(string userName)
    {
        public string UserName { get; } = userName;
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