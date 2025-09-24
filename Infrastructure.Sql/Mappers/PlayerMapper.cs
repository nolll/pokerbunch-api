using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class PlayerMapper
{
    internal static Player ToPlayer(this PlayerDto playerDto)
    {
        return new Player(
            playerDto.Bunch_Id.ToString(),
            playerDto.Bunch_Slug,
            playerDto.Player_Id.ToString(),
            playerDto.User_Id != null ? playerDto.User_Id.ToString() : null,
            playerDto.User_Name,
            playerDto.Player_Name,
            (Role)playerDto.Role_Id,
            playerDto.Color);
    }
}