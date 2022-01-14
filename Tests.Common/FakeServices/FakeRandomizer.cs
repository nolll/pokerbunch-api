using Core.Services;

namespace Tests.Common.FakeServices;

public class FakeRandomizer : IRandomizer
{
    public string GetAllowedChars()
    {
        return "a";
    }
}