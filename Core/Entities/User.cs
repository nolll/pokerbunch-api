namespace Core.Entities;

public class User(
    string id,
    string userName,
    string? displayName = null,
    string? realName = null,
    string? email = null,
    Role globalRole = Role.Player,
    string? encryptedPassword = null,
    string? salt = null)
    : IEntity
{
    public string Id { get; } = id;
    public string UserName { get; } = userName;
    public string DisplayName { get; } = displayName ?? "";
    public string RealName { get; } = realName ?? "";
    public string Email { get; } = email ?? "";
    public Role GlobalRole { get; } = globalRole;
    public string EncryptedPassword { get; private set; } = encryptedPassword ?? "";
    public string Salt { get; private set; } = salt ?? "";

    public bool IsAdmin => GlobalRole == Role.Admin;

    public void SetPassword(string p, string s)
    {
        EncryptedPassword = p;
        Salt = s;
    }
}