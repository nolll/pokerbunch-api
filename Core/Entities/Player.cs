namespace Core.Entities
{
    public class Player : IEntity
    {
        public int BunchId { get; private set; }
	    public int Id { get; }
        public int UserId { get; }
        public string DisplayName { get; private set; }
        public Role Role { get; }
        public string Color { get; private set; }
        public bool IsUser => UserId != default(int);
        public const string DefaultColor = "#9e9e9e";

	    public Player(
            int bunchId,
            int id, 
            int userId, 
            string displayName = null, 
            Role role = Role.Player,
            string color = null)
	    {
	        BunchId = bunchId;
	        Id = id;
	        UserId = userId;
	        DisplayName = displayName;
	        Role = role;
	        Color = color ?? DefaultColor;
	    }

        public static Player New(int bunchId, string displayName, Role role = Role.Player, string color = null)
        {
            return new Player(bunchId, 0, 0, displayName, role, color);
        }

        public static Player New(int bunchId, int userId, Role role = Role.Player, string color = null)
        {
            return new Player(bunchId, 0, userId, null, role, color);
        }

        public bool IsInRole(Role requiredRole)
        {
            return Role >= requiredRole;
        }
	}
}