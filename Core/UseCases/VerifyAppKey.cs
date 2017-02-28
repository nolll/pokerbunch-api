using Core.Exceptions;
using Core.Repositories;

namespace Core.UseCases
{
    public class VerifyAppKey
    {
        private readonly IAppRepository _appRepository;

        public VerifyAppKey(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public Result Execute(Request request)
        {
            try
            {
                _appRepository.Get(request.AppKey);
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
            public override bool IsValid => true;
        }

        private class InvalidResult : Result
        {
            public override bool IsValid => false;
        }
    }
}