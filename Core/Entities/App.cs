namespace Core.Entities
{
    public class App : IEntity
    {
        public int Id { get; }
        public string AppKey { get; private set; }
        public string Name { get; private set; }
        public int UserId { get; private set; }

        public App(int id, string appKey, string name, int userId)
        {
            Id = id;
            AppKey = appKey;
            Name = name;
            UserId = userId;
        }
    }
}