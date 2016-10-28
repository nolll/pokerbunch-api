using System;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class EditCheckpointForm
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;

        public EditCheckpointForm(IBunchRepository bunchRepository, CashgameService cashgameService, IUserRepository userRepository, PlayerService playerService)
        {
            _bunchRepository = bunchRepository;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var cashgame = _cashgameService.GetByCheckpoint(request.CheckpointId);
            var checkpoint = cashgame.GetCheckpoint(request.CheckpointId);
            var bunch = _bunchRepository.Get(cashgame.BunchId);
            var user = _userRepository.Get(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Manager(user, player);
            var stack = checkpoint.Stack;
            var amount = checkpoint.Amount;
            var timestamp = TimeZoneInfo.ConvertTime(checkpoint.Timestamp, bunch.Timezone);
            var canEditAmount = checkpoint.Type == CheckpointType.Buyin;

            return new Result(stack, amount, timestamp, checkpoint.Id, cashgame.Id, player.Id, bunch.Slug, canEditAmount);
        }

        public class Request
        {
            public string UserName { get; }
            public int CheckpointId { get; }

            public Request(string userName, int checkpointId)
            {
                UserName = userName;
                CheckpointId = checkpointId;
            }
        }

        public class Result
        {
            public int Stack { get; private set; }
            public int Amount { get; private set; }
            public DateTime TimeStamp { get; private set; }
            public int CheckpointId { get; private set; }
            public int CashgameId { get; private set; }
            public int PlayerId { get; private set; }
            public string Slug { get; private set; }
            public bool CanEditAmount { get; private set; }

            public Result(int stack, int amount, DateTime timeStamp, int checkpointId, int cashgameId, int playerId, string slug, bool canEditAmount)
            {
                TimeStamp = timeStamp;
                CheckpointId = checkpointId;
                Stack = stack;
                Amount = amount;
                CashgameId = cashgameId;
                PlayerId = playerId;
                Slug = slug;
                CanEditAmount = canEditAmount;
            }
        }
    }
}