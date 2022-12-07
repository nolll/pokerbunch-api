using Core.Entities;
using JetBrains.Annotations;

namespace Infrastructure.Sql.Dtos;

[UsedImplicitly]
public class UserDto
{
    [UsedImplicitly] public int User_Id { get; set; }
    [UsedImplicitly] public string User_Name { get; set; }
    [UsedImplicitly] public string Display_Name { get; set; }
    [UsedImplicitly] public string Real_Name { get; set; }
    [UsedImplicitly] public string Email { get; set; }
    [UsedImplicitly] public int Role_Id { get; set; }
    [UsedImplicitly] public string Password { get; set; }
    [UsedImplicitly] public string Salt { get; set; }

    public static User CreateReal(UserDto userDto)
    {
        return new User(
            userDto.User_Id.ToString(),
            userDto.User_Name,
            userDto.Display_Name,
            userDto.Real_Name,
            userDto.Email,
            (Role)userDto.Role_Id,
            userDto.Password,
            userDto.Salt);
    }
}