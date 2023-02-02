using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class UserMapper
{
    internal static User ToUser(this UserDto userDto)
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