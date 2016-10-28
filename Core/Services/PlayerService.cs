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

        public IList<Player> List(int bunchId)
        {
            return _playerRepository.List(bunchId);
        }

        public IList<Player> Get(IList<int> ids)
        {
            return _playerRepository.Get(ids);
        }

        public Player Get(int id)
        {
            return _playerRepository.Get(id);
        }

        public Player Get(int bunchId, int userId)
        {
            return _playerRepository.Get(bunchId, userId);
        }

        public int Add(Player player)
        {
            return _playerRepository.Add(player);
        }

        public bool JoinBunch(Player player, Bunch bunch, int userId)
        {
            return _playerRepository.JoinBunch(player, bunch, userId);
        }

        public void Delete(int playerId)
        {
            _playerRepository.Delete(playerId);
        }
    }
}