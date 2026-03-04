namespace Tests.Integration.Fixtures;

public class PlayerFixture(string id, string? name, string? userId, string bunchId)
{
    public string Id { get; } = id;
    public string? Name { get; } = name;
    public string? UserId { get; set; } = userId;
    public string BunchId { get; } = bunchId;
}