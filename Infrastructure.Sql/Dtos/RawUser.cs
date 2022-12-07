using Core.Entities;
using JetBrains.Annotations;

namespace Infrastructure.Sql.Classes;

[UsedImplicitly]
public class RawUser
{
    [UsedImplicitly] public int User_Id { get; set; }
    [UsedImplicitly] public string User_Name { get; set; }
    [UsedImplicitly] public string Display_Name { get; set; }
    [UsedImplicitly] public string Real_Name { get; set; }
    [UsedImplicitly] public string Email { get; set; }
    [UsedImplicitly] public int Role_Id { get; set; }
    [UsedImplicitly] public string Password { get; set; }
    [UsedImplicitly] public string Salt { get; set; }

    public static User CreateReal(RawUser rawUser)
    {
        return new User(
            rawUser.User_Id.ToString(),
            rawUser.User_Name,
            rawUser.Display_Name,
            rawUser.Real_Name,
            rawUser.Email,
            (Role)rawUser.Role_Id,
            rawUser.Password,
            rawUser.Salt);
    }
}