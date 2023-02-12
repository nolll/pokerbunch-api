using Core.Entities;
using Core.Services;

namespace Tests.Common.FakeServices;

public class FakeInvitationCodeCreator : IInvitationCodeCreator
{
    public string GetCode(Player player)
    {
        return "abcdefghij";
    }
}