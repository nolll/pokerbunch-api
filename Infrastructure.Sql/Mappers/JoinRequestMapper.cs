using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class JoinRequestMapper
{
    internal static JoinRequest ToJoinRequest(this JoinRequestDto joinRequestDto) => new(
        joinRequestDto.Join_Request_Id.ToString(),
        joinRequestDto.Bunch_Id.ToString(),
        joinRequestDto.User_Id.ToString(),
        joinRequestDto.User_Name);
}