using Core.Entities;

namespace Core.Services;

public interface IInvitationCodeCreator
{
    string GetCode(Player player);
}