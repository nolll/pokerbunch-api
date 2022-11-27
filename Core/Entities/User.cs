namespace Core.Entities;

public class User : IEntity
{
    public string Id { get; }
    public string UserName { get; }
    public string DisplayName { get; }
    public string RealName { get; }
    public string Email { get; }
    public Role GlobalRole { get; }
    public string EncryptedPassword { get; private set; }
    public string Salt { get; private set; }

    public User(
        string id, 
        string userName, 
        string displayName = null, 
        string realName = null, 
        string email = null, 
        Role globalRole = Role.Player,
        string encryptedPassword = null,
        string salt = null)
    {
        Id = id;
        UserName = userName;
        DisplayName = displayName ?? string.Empty;
        RealName = realName ?? string.Empty;
        Email = email ?? string.Empty;
        GlobalRole = globalRole;
        EncryptedPassword = encryptedPassword ?? string.Empty;
        Salt = salt ?? string.Empty;
    }

    public bool IsAdmin => GlobalRole == Role.Admin;

    public void SetPassword(string encryptedPassword, string salt)
    {
        EncryptedPassword = encryptedPassword;
        Salt = salt;
    }
}