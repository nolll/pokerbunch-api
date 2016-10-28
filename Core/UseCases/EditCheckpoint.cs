using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class EditCheckpoint
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly IUserRepository _userRepository;
        private readonly PlayerService _playerService;
        private readonly CashgameService _cashgameService;

        public EditCheckpoint(IBunchRepository bunchRepository, IUserRepository userRepository, PlayerService playerService, CashgameService cashgameService)
        {
            _bunchRepository = bunchRepository;
            _userRepository = userRepository;
            _playerService = playerService;
            _cashgameService = cashgameService;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            var cashgame = _cashgameService.GetByCheckpoint(request.CheckpointId);
            var existingCheckpoint = cashgame.GetCheckpoint(request.CheckpointId);
            //var existingCheckpoint = _cashgameService.GetCheckpoint(request.CheckpointId);
            var bunch = _bunchRepository.Get(cashgame.BunchId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.Get(bunch.Id, currentUser.Id);
            RequireRole.Manager(currentUser, currentPlayer);
            
            var postedCheckpoint = Checkpoint.Create(
                existingCheckpoint.CashgameId,
                existingCheckpoint.PlayerId,
                TimeZoneInfo.ConvertTimeToUtc(request.Timestamp, bunch.Timezone),
                existingCheckpoint.Type,
                request.Stack,
                request.Amount,
                existingCheckpoint.Id);

            cashgame.UpdateCheckpoint(postedCheckpoint);
            _cashgameService.UpdateGame(cashgame);

            return new Result(cashgame.Id, existingCheckpoint.PlayerId);
        }

        public class Request
        {
            public string UserName { get; }
            public int CheckpointId { get; }
            public DateTime Timestamp { get; }
            [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
            public int Stack { get; }
            [Range(0, int.MaxValue, ErrorMessage = "Amount can't be negative")]
            public int Amount { get; }

            public Request(string userName, int checkpointId, DateTime timestamp, int stack, int amount)
            {
                UserName = userName;
                CheckpointId = checkpointId;
                Timestamp = timestamp;
                Stack = stack;
                Amount = amount;
            }
        }

        public class Result
        {
            public int CashgameId { get; private set; }
            public int PlayerId { get; private set; }

            public Result(int cashgameId, int playerId)
            {
                CashgameId = cashgameId;
                PlayerId = playerId;
            }
        }
    }
}
