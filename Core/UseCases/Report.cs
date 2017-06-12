using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class Report
    {
        private readonly ICashgameRepository _cashgameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public Report(ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            _cashgameRepository = cashgameRepository;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);
            if(!validator.IsValid)
                throw new ValidationException(validator);

            var cashgame = _cashgameRepository.Get(request.CashgameId);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerRepository.Get(cashgame.BunchId, currentUser.Id);
            RequireRole.Me(currentUser, currentPlayer, request.PlayerId);

            var checkpoint = Checkpoint.Create(cashgame.Id, request.PlayerId, request.CurrentTime, CheckpointType.Report, request.Stack);
            cashgame.AddCheckpoint(checkpoint);
            _cashgameRepository.Update(cashgame);
        }

        public class Request
        {
            public string UserName { get; }
            public int CashgameId { get; }
            public int PlayerId { get; }
            [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
            public int Stack { get; }
            public DateTime CurrentTime { get; }

            public Request(string userName, int cashgameId, int playerId, int stack, DateTime currentTime)
            {
                UserName = userName;
                CashgameId = cashgameId;
                PlayerId = playerId;
                Stack = stack;
                CurrentTime = currentTime;
            }
        }
    }
}
