using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakePlayerRepository : IPlayerRepository
{
    public Player Added { get; private set; }
    public int Deleted { get; private set; }
    public JoinedData Joined { get; private set; }
    private readonly IList<Player> _list;

    public FakePlayerRepository()
    {
        _list = CreateList();
    }

    public IList<Player> List(int bunchId)
    {
        return _list.Where(o => o.BunchId == bunchId).ToList();
    }

    public Player Get(int bunchId, int userId)
    {
        return _list.FirstOrDefault(o => o.BunchId == bunchId && o.UserId == userId);
    }

    public IList<Player> Get(IList<int> ids)
    {
        return _list.Where(o => ids.Contains(o.Id)).ToList();
    }

    public Player Get(int id)
    {
        return _list.FirstOrDefault(o => o.Id == id);
    }

    public int Add(Player player)
    {
        Added = player;
        return 1;
    }

    public bool JoinBunch(Player player, Bunch bunch, int userId)
    {
        Joined = new JoinedData(player.Id, bunch.Id, userId);
        return true;
    }

    public void Delete(int playerId)
    {
        Deleted = playerId;
    }

    private IList<Player> CreateList()
    {
        return new List<Player>
        {
            TestData.PlayerA,
            TestData.PlayerB,
            TestData.PlayerC,
            TestData.PlayerD
        };
    }

    public class JoinedData
    {
        public JoinedData(int playerId, int bunchId, int userId)
        {
            PlayerId = playerId;
            BunchId = bunchId;
            UserId = userId;
        }

        public int PlayerId { get; private set; }
        public int BunchId { get; private set; }
        public int UserId { get; private set; }
    }
}