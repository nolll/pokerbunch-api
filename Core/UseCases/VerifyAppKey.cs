using Core.Exceptions;
using Core.Services;

namespace Core.UseCases
{
    public class VerifyAppKey
    {
        private readonly AppService _appService;

        public VerifyAppKey(AppService appService)
        {
            _appService = appService;
        }

        public Result Execute(Request request)
        {
            try
            {
                _appService.Get(request.AppKey);
                return new ValidResult();
            }
            catch (AppNotFoundException)
            {
                return new InvalidResult();
            }
        }

        public class Request
        {
            public string AppKey { get; }

            public Request(string appKey)
            {
                AppKey = appKey;
            }
        }

        public abstract class Result
        {
            public abstract bool IsValid { get; }
        }

        private class ValidResult : Result
        {
            public override bool IsValid
            {
                get { return true; }
            }
        }

        private class InvalidResult : Result
        {
            public override bool IsValid
            {
                get { return false; }
            }
        }
    }
}