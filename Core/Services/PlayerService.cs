using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public IList<Player> GetList(int bunchId)
        {
            var ids = _playerRepository.Find(bunchId);
            return _playerRepository.Get(ids);
        }

        public IList<Player> Get(IList<int> ids)
        {
            return _playerRepository.Get(ids);
        }

        public Player Get(int id)
        {
            return _playerRepository.Get(id);
        }

        public Player GetByUserId(int bunchId, int userId)
        {
            var ids = _playerRepository.Find(bunchId, userId);
            if (!ids.Any())
                return null;
            return _playerRepository.Get(ids).First();
        }

        public int Add(Player player)
        {
            return _playerRepository.Add(player);
        }

        public bool JoinHomegame(Player player, Bunch bunch, int userId)
        {
            return _playerRepository.JoinHomegame(player, bunch, userId);
        }

        public void Delete(int playerId)
        {
            _playerRepository.Delete(playerId);
        }
    }
}