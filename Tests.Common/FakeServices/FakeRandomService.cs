using Core.Services;

namespace Tests.Common.FakeServices
{
    public class FakeRandomService : IRandomService
    {
        public string GetAllowedChars()
        {
            return "a";
        }
    }
}