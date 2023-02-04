using Core.Entities;
using Core.Repositories;

namespace Tests.Common.FakeRepositories;

public class FakePlayerRepository : IPlayerRepository
{
    public Player? Added { get; private set; }
    public string? Deleted { get; private set; }
    public JoinedData? Joined { get; private set; }
    private readonly IList<Player> _list;

    public FakePlayerRepository()
    {
        _list = CreateList();
    }

    public Task<IList<Player>> List(string bunchId)
    {
        return Task.FromResult<IList<Player>>(_list.Where(o => o.BunchId == bunchId).ToList());
    }

    public Task<Player> Get(string bunchId, string userId)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.BunchId == bunchId && o.UserId == userId))!;
    }

    public Task<IList<Player>> Get(IList<string> ids)
    {
        return Task.FromResult<IList<Player>>(_list.Where(o => ids.Contains(o.Id)).ToList());
    }

    public Task<Player> Get(string id)
    {
        return Task.FromResult(_list.FirstOrDefault(o => o.Id == id))!;
    }

    public Task<string> Add(Player player)
    {
        Added = player;
        return Task.FromResult("1");
    }

    public Task<bool> JoinBunch(Player player, Bunch bunch, string userId)
    {
        Joined = new JoinedData(player.Id, bunch.Id, userId);
        return Task.FromResult(true);
    }

    public Task Delete(string playerId)
    {
        Deleted = playerId;
        return Task.CompletedTask;
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
        public JoinedData(string playerId, string bunchId, string userId)
        {
            PlayerId = playerId;
            BunchId = bunchId;
            UserId = userId;
        }

        public string PlayerId { get; }
        public string BunchId { get; }
        public string UserId { get; }
    }
}