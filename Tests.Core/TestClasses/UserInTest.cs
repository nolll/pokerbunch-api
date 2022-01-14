using Core.Entities;

namespace Tests.Core.TestClasses;

public class UserInTest : User
{
    public UserInTest(
        int id = 0,
        string userName = null,
        string displayName = null,
        string realName = null,
        string email = null,
        Role globalRole = Role.Player,
        string encryptedPassword = null,
        string salt = null)
        : base(
            id, 
            userName, 
            displayName, 
            realName, 
            email, 
            globalRole, 
            encryptedPassword, 
            salt)
    {
    }
}