namespace Core.UseCases
{
    public class LoginForm
    {
        public Result Execute(Request request)
        {
            var returnUrl = string.IsNullOrEmpty(request.ReturnUrl) ? request.DefaultUrl : request.ReturnUrl;
            return new Result(returnUrl);
        }

        public class Request
        {
            public string DefaultUrl { get; set; }
            public string ReturnUrl { get; }

            public Request(string defaultUrl, string returnUrl)
            {
                DefaultUrl = defaultUrl;
                ReturnUrl = returnUrl;
            }
        }

        public class Result
        {
            public string ReturnUrl { get; private set; }

            public Result(string returnUrl)
            {
                ReturnUrl = returnUrl;
            }
        }
    }
}