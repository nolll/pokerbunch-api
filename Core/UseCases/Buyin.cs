using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class Buyin
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly PlayerService _playerService;
        private readonly CashgameService _cashgameService;
        private readonly IUserRepository _userRepository;

        public Buyin(IBunchRepository bunchRepository, PlayerService playerService, CashgameService cashgameService, IUserRepository userRepository)
        {
            _bunchRepository = bunchRepository;
            _playerService = playerService;
            _cashgameService = cashgameService;
            _userRepository = userRepository;
        }

        public void Execute(Request request)
        {
            var validator = new Validator(request);

            if (!validator.IsValid)
                throw new ValidationException(validator);

            var bunch = _bunchRepository.GetBySlug(request.Slug);
            var currentUser = _userRepository.Get(request.UserName);
            var currentPlayer = _playerService.Get(bunch.Id, currentUser.Id);
            RequireRole.Me(currentUser, currentPlayer, request.PlayerId);
            var cashgame = _cashgameService.GetRunning(bunch.Id);

            var stackAfterBuyin = request.StackAmount + request.BuyinAmount;
            var checkpoint = new BuyinCheckpoint(cashgame.Id, request.PlayerId, request.CurrentTime, stackAfterBuyin, request.BuyinAmount);
            cashgame.AddCheckpoint(checkpoint);
            _cashgameService.UpdateGame(cashgame);
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }
            public int PlayerId { get; }
            [Range(1, int.MaxValue, ErrorMessage = "Amount needs to be positive")]
            public int BuyinAmount { get; }
            [Range(0, int.MaxValue, ErrorMessage = "Stack can't be negative")]
            public int StackAmount { get; }
            public DateTime CurrentTime { get; }

            public Request(string userName, string slug, int playerId, int buyinAmount, int stackAmount, DateTime currentTime)
            {
                UserName = userName;
                Slug = slug;
                PlayerId = playerId;
                BuyinAmount = buyinAmount;
                StackAmount = stackAmount;
                CurrentTime = currentTime;
            }
        }
    }
}