using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "players", ItemName = "player")]
    public class ApiPlayerList : List<ApiPlayer>
    {
        public ApiPlayerList(PlayerList.Result playerListResult)
        {
            AddRange(playerListResult.Players.Select(o => new ApiPlayer(o.Name)));
        }

        public ApiPlayerList()
        {
        }
    }
}