using System.Collections.Generic;
using System.Linq;
using Core.UseCases;

namespace Api.Models.PlayerModels;

public class PlayerListModel : List<PlayerListItemModel>
{
    public PlayerListModel(GetPlayerList.Result playerListResult)
    {
        AddRange(playerListResult.Players.Select(o => new PlayerListItemModel(o)));
    }
}