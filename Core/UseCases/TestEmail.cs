using Core.Errors;
using Core.Messages;
using Core.Services;

namespace Core.UseCases;

public class TestEmail(IEmailSender emailSender)
    : UseCase<TestEmail.Request, TestEmail.Result>
{
    protected override Task<UseCaseResult<Result>> Work(Request request)
    {
        if (!request.Auth.CanSendTestEmail)
            return Task.FromResult(Error(new AccessDeniedError()));
        
        var message = new TestMessage();
        emailSender.Send(request.To, message);

        return Task.FromResult(Success(new Result(request.To)));
    }
    
    public class Request(IAuth auth, string to)
    {
        public IAuth Auth { get; } = auth;
        public string To { get; } = to;
    }

    public class Result(string email)
    {
        public string Email { get; } = email;
        public string Message { get; } = $"An email was sent to {email}";
    }
}