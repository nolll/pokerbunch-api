using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class TestEmail : UseCase<TestEmail.Request, TestEmail.Result>
{
    private readonly IEmailSender _emailSender;
    private readonly IUserRepository _userRepository;

    public TestEmail(IEmailSender emailSender, IUserRepository userRepository)
    {
        _emailSender = emailSender;
        _userRepository = userRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var user = await _userRepository.GetByUserName(request.UserName);
        if (!AccessControl.CanSendTestEmail(user))
            return Error(new AccessDeniedError());

        // todo: Move email to config
        const string email = "henriks@gmail.com";
        var message = new TestMessage();
        _emailSender.Send(email, message);

        return Success(new Result(email));
    }
    
    public class Request
    {
        public string UserName { get; }

        public Request(string userName)
        {
            UserName = userName;
        }
    }

    public class Result
    {
        public string Email { get; }
        public string Message { get; }

        public Result(string email)
        {
            Email = email;
            Message = $"An email was sent to {email}";
    }
    }

    private class TestMessage : IMessage
    {
        public string Subject => "Test Email";
        public string Body => "This is a test email from pokerbunch.com";
    }
}