using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "players", ItemName = "player")]
    public class PlayerListModel : List<PlayerModel>
    {
        public PlayerListModel(GetPlayerList.Result playerListResult)
        {
            AddRange(playerListResult.Players.Select(o => new PlayerModel(o)));
        }

        public PlayerListModel()
        {
        }
    }
}