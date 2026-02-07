namespace Core.Entities;

public class JoinRequest(string id, string bunchId, string userId, string userName) : IEntity
{
    public string Id { get; } = id;
    public string BunchId { get; } = bunchId;
    public string UserId { get; } = userId;
    public string UserName { get; } = userName;
}