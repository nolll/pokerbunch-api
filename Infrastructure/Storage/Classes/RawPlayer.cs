namespace Infrastructure.Storage.Classes
{
	public class RawPlayer
    {
	    public int BunchId { get; private set; }
        public int Id { get; private set; }
        public int UserId { get; private set; }
	    public string DisplayName { get; private set; }
	    public int Role { get; private set; }
	    public string Color { get; private set; }

	    public RawPlayer(int bunchId, int id, int userId, string displayName, int role, string color)
	    {
	        BunchId = bunchId;
	        Id = id;
	        UserId = userId;
	        DisplayName = displayName;
	        Role = role;
	        Color = color;
	    }
	}
}