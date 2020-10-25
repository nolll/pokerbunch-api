namespace Core.Entities
{
    public class Location : IEntity
    {
        public int Id { get; }
        public string Name { get; }
        public int BunchId { get; }

        public Location(int id, string name, int bunchId)
        {
            Id = id;
            Name = name;
            BunchId = bunchId;
        }
    }
}