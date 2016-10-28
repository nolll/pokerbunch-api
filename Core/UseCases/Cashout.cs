using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class Cashout
    {
        private readonly IBunchService _bunchService;
        private readonly ICashgameService _cashgameService;
        private readonly IPlayerService _playerService;
        private readonly IUserRepository _userRepository;

        public Cashout(IBunchService bunchService, ICashgameService cashgameService, IPlayerService playerService, IUserRepository userRepository)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _playerService = playerService;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            var bunch = _bunchService.GetBySlug(request.Slug);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.GetByUserId(bunch.Id, currentUser.Id);
            RequireRole.Me(currentUser, currentPlayer, request.PlayerId);
            var cashgame = _cashgameService.GetRunning(bunch.Id);
            var result = cashgame.GetResult(request.PlayerId);

            var existingCashoutCheckpoint = result.CashoutCheckpoint;
            var postedCheckpoint = Checkpoint.Create(
                cashgame.Id,
                request.PlayerId,
                request.CurrentTime,
                CheckpointType.Cashout,
                request.Stack,
                0,
                existingCashoutCheckpoint != null ? existingCashoutCheckpoint.Id : 0);

            if (existingCashoutCheckpoint != null)
                cashgame.UpdateCheckpoint(postedCheckpoint);
            else
                cashgame.AddCheckpoint(postedCheckpoint);
            _cashgameService.UpdateGame(cashgame);

            return new Result(cashgame.Id);
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }
            public int PlayerId { get; }
            [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
            public int Stack { get; }
            public DateTime CurrentTime { get; }

            public Request(string userName, string slug, int playerId, int stack, DateTime currentTime)
            {
                UserName = userName;
                Slug = slug;
                PlayerId = playerId;
                Stack = stack;
                CurrentTime = currentTime;
            }
        }

        public class Result
        {
            public int CashgameId { get; private set; }

            public Result(int cashgameId)
            {
                CashgameId = cashgameId;
            }
        }
    }
}
