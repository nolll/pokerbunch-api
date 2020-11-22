namespace Core.Entities
{
    public class Player : IEntity
    {
        public int BunchId { get; }
	    public int Id { get; }
        public int UserId { get; }
        public string UserName { get; }
        public string DisplayName { get; }
        public Role Role { get; }
        public string Color { get; }
        public bool IsUser => UserId != default;
        public const string DefaultColor = "#9e9e9e";

	    public Player(
            int bunchId,
            int id, 
            int userId, 
            string userName,
            string displayName = null, 
            Role role = Role.Player,
            string color = null)
	    {
	        BunchId = bunchId;
	        Id = id;
	        UserId = userId;
            UserName = userName;
	        DisplayName = displayName;
	        Role = role;
	        Color = color ?? DefaultColor;
	    }

        public static Player New(int bunchId, string displayName, Role role = Role.Player, string color = null)
        {
            return new Player(bunchId, 0, 0, null, displayName, role, color);
        }

        public static Player New(int bunchId, int userId, string userName, Role role = Role.Player, string color = null)
        {
            return new Player(bunchId, 0, userId, userName, null, role, color);
        }

        public bool IsInRole(Role requiredRole)
        {
            return Role >= requiredRole;
        }
	}
}