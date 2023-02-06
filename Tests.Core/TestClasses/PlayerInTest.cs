using Core.Entities;

namespace Tests.Core.TestClasses;

public class PlayerInTest : Player
{
    public PlayerInTest(
        string bunchId = "",
        string id = "",
        string userId = "", 
        string userName = "",
        string? displayName = null, 
        Role role = Role.Player, 
        string? color = null)
        : base(
            bunchId, 
            id, 
            userId, 
            userName,
            displayName, 
            role, 
            color)
    {
    }
}