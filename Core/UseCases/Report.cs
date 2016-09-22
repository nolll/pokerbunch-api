using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class Report
    {
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public Report(BunchService bunchService, CashgameService cashgameService, PlayerService playerService, UserService userService)
        {
            _bunchService = bunchService;
            _cashgameService = cashgameService;
            _playerService = playerService;
            _userService = userService;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            var bunch = _bunchService.GetBySlug(request.Slug);
            var cashgame = _cashgameService.GetRunning(bunch.Id);
            var currentUser = _userService.GetByNameOrEmail(request.UserName);
            var currentPlayer = _playerService.GetByUserId(bunch.Id, currentUser.Id);
            RequireRole.Me(currentUser, currentPlayer, request.PlayerId);

            var checkpoint = Checkpoint.Create(cashgame.Id, request.PlayerId, request.CurrentTime, CheckpointType.Report, request.Stack);
            cashgame.AddCheckpoint(checkpoint);
            _cashgameService.UpdateGame(cashgame);
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
    }
}
