using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class PlayerMapper
{
    internal static Player ToPlayer(this PlayerDto playerDto) => new(
        playerDto.BunchSlug,
        playerDto.PlayerId.ToString(),
        playerDto.UserId != null ? playerDto.UserId.ToString() : null,
        playerDto.UserName,
        playerDto.PlayerName,
        (Role)playerDto.RoleId,
        playerDto.Color);
}