using Core.Services;

namespace Core.UseCases
{
    public class BaseContext
    {
        public Result Execute()
        {
            var version = new ApplicationVersion().DisplayVersion;

            return new Result(version);
        }

        public class Result
        {
            public string Version { get; }

            public Result(string version)
            {
                Version = version;
            }
        }
    }
}