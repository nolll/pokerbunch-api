using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class JoinRequestMapper
{
    internal static JoinRequest ToJoinRequest(this JoinRequestDto joinRequestDto) => new(
        joinRequestDto.JoinRequestId.ToString(),
        joinRequestDto.BunchId.ToString(),
        joinRequestDto.UserId.ToString(),
        joinRequestDto.UserName);
}