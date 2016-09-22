namespace Core.Entities
{
    public class Location : IEntity
    {
        public int Id { get; }
        public string Name { get; private set; }
        public int BunchId { get; private set; }

        public Location(int id, string name, int bunchId)
        {
            Id = id;
            Name = name;
            BunchId = bunchId;
        }
    }
}