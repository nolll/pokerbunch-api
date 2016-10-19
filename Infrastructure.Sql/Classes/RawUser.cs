using Core.Entities;

namespace Infrastructure.Sql.Classes
{
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

	    public RawUser(int id, string userName, string displayName, string realName, string email, int globalRole, string encryptedPassword, string salt)
	    {
	        Id = id;
	        UserName = userName;
	        DisplayName = displayName;
	        RealName = realName;
	        Email = email;
	        GlobalRole = globalRole;
	        EncryptedPassword = encryptedPassword;
	        Salt = salt;
	    }

	    public static User CreateReal(RawUser rawUser)
        {
            return new User(
                rawUser.Id,
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
}