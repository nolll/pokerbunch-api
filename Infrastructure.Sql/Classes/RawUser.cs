using Core.Entities;

namespace Infrastructure.Sql.Classes;

public class RawUser
{
    public int Id { get; }
    public string UserName { get; }
    public string DisplayName { get; }
    public string RealName { get; }
    public string Email { get; }
    public int GlobalRole { get; }
    public string EncryptedPassword { get; }
    public string Salt { get; }

    public RawUser(int user_id, string user_name, string display_name, string real_name, string email, string password, string salt, int role_id)
    {
        Id = user_id;
        UserName = user_name;
        DisplayName = display_name;
        RealName = real_name;
        Email = email;
        GlobalRole = role_id;
        EncryptedPassword = password;
        Salt = salt;
    }

    public static User CreateReal(RawUser rawUser)
    {
        return new User(
            rawUser.Id.ToString(),
            rawUser.UserName,
            rawUser.DisplayName,
            rawUser.RealName,
            rawUser.Email,
            (Role)rawUser.GlobalRole,
            rawUser.EncryptedPassword,
            rawUser.Salt);
    }

    public static string ToStringId(int id)
    {
        return string.Concat("RawUsers/", id);
    }
}