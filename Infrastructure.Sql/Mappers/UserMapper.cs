using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class UserMapper
{
    internal static User ToUser(this UserDto userDto) => new(
        userDto.UserId.ToString(),
        userDto.UserName,
        userDto.DisplayName,
        userDto.RealName,
        userDto.Email,
        (Role)userDto.RoleId,
        userDto.Password,
        userDto.Salt);
}