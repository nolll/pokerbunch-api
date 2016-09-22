using Core.Services;

namespace Core.UseCases
{
    public class TestEmail
    {
        private readonly IMessageSender _messageSender;
        private readonly UserService _userService;

        public TestEmail(IMessageSender messageSender, UserService userService)
        {
            _messageSender = messageSender;
            _userService = userService;
        }

        public Result Execute(Request request)
        {
            var user = _userService.GetByNameOrEmail(request.UserName);
            RequireRole.Admin(user);
            const string email = "henriks@gmail.com";
            var message = new TestMessage();
            _messageSender.Send(email, message);

            return new Result(email);
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
            public string Email { get; private set; }

            public Result(string email)
            {
                Email = email;
            }
        }

        private class TestMessage : IMessage
        {
            public string Subject
            {
                get { return "Test Email"; }
            }

            public string Body
            {
                get { return "This is a test email from pokerbunch.com"; }
            }
        }
    }
}