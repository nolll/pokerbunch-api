namespace Core.Entities;

public class CurrentUser(string id, string userName, string displayName, bool isAdmin)
{
    public string Id { get; } = id;
    public string UserName { get; } = userName;
    public string DisplayName { get; } = displayName;
    public bool IsAdmin { get; } = isAdmin;
}