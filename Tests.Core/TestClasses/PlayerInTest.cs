using Core.Entities;

namespace Tests.Core.TestClasses
{
    public class PlayerInTest : Player
    {
        public PlayerInTest(
            int bunchId = 0, 
            int id = 0, 
            int userId = 0, 
            string userName = null,
            string displayName = null, 
            Role role = Role.Player, 
            string color = null)
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
}