using System;
using System.Collections.Generic;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class ActionsChart
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;

        public ActionsChart(BunchService bunchService, CashgameService cashgameService, IUserRepository userRepository, PlayerService playerService)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var cashgame = _cashgameService.GetById(request.CashgameId);
            var bunch = _bunchService.Get(cashgame.BunchId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.GetByUserId(bunch.Id, currentUser.Id);
            RequireRole.Player(currentUser, currentPlayer);
            
            var result = cashgame.GetResult(request.PlayerId);

            var checkpointItems = GetCheckpointItems(bunch, cashgame, result, request.CurrentTime);

            return new Result(checkpointItems);
        }

        private static IList<CheckpointItem> GetCheckpointItems(Bunch bunch, Cashgame cashgame, CashgameResult result, DateTime now)
        {
            var checkpointItems = new List<CheckpointItem>();
            var checkpoints = GetCheckpoints(result);
            var totalBuyin = 0;
            foreach (var checkpoint in checkpoints)
            {
                var stack = checkpoint.Stack;
                var addedMoney = 0;
                if (checkpoint.Type == CheckpointType.Buyin)
                {
                    if (totalBuyin > 0)
                    {
                        addedMoney = checkpoint.Amount;
                    }
                    totalBuyin += checkpoint.Amount;
                }
                var localTime = TimeZoneInfo.ConvertTime(checkpoint.Timestamp, bunch.Timezone);
                checkpointItems.Add(new CheckpointItem(localTime, stack, totalBuyin, addedMoney));
            }
            if (cashgame.Status == GameStatus.Running)
            {
                var localTime = TimeZoneInfo.ConvertTime(now, bunch.Timezone);
                checkpointItems.Add(new CheckpointItem(localTime, result.Stack, result.Buyin));
            }
            return checkpointItems;
        }

        private static IEnumerable<Checkpoint> GetCheckpoints(CashgameResult result)
        {
            return PlayerIsInGame(result) ? result.Checkpoints : new List<Checkpoint>();
        }

        private static bool PlayerIsInGame(CashgameResult result)
        {
            return result != null;
        }

        public class Request
        {
            public string UserName { get; }
            public int CashgameId { get; }
            public int PlayerId { get; }
            public DateTime CurrentTime { get; }

            public Request(string userName, int cashgameId, int playerId, DateTime currentTime)
            {
                UserName = userName;
                CashgameId = cashgameId;
                PlayerId = playerId;
                CurrentTime = currentTime;
            }
        }

        public class Result
        {
            public IList<CheckpointItem> CheckpointItems { get; private set; }

            public Result(IList<CheckpointItem> checkpointItems)
            {
                CheckpointItems = checkpointItems;
            }
        }

        public class CheckpointItem
        {
            public DateTime Timestamp { get; private set; }
            public int Stack { get; private set; }
            public int TotalBuyin { get; private set; }
            public int AddedMoney { get; private set; }

            public CheckpointItem(DateTime timestamp, int stack, int totalBuyin, int addedMoney = 0)
            {
                Timestamp = timestamp;
                Stack = stack;
                TotalBuyin = totalBuyin;
                AddedMoney = addedMoney;
            }
        }
    }
}