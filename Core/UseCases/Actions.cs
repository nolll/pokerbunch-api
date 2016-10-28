using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases
{
    public class Actions
    {
        private readonly IBunchRepository _bunchRepository;
        private readonly CashgameService _cashgameService;
        private readonly IPlayerRepository _playerRepository;
        private readonly IUserRepository _userRepository;

        public Actions(IBunchRepository bunchRepository, CashgameService cashgameService, IPlayerRepository playerRepository, IUserRepository userRepository)
        {
            _bunchRepository = bunchRepository;
            _cashgameService = cashgameService;
            _playerRepository = playerRepository;
            _userRepository = userRepository;
        }

        public Result Execute(Request request)
        {
            var player = _playerRepository.Get(request.PlayerId);
            var user = _userRepository.Get(request.CurrentUserName);
            var bunch = _bunchRepository.Get(player.BunchId);
            var cashgame = _cashgameService.GetById(request.CashgameId);
            
            RequireRole.Player(user, player);
            var playerResult = cashgame.GetResult(player.Id);
            var currentPlayer = _playerRepository.Get(bunch.Id, user.Id);
            var isManager = RoleHandler.IsInRole(user, currentPlayer, Role.Manager);

            var date = cashgame.StartTime.HasValue ? cashgame.StartTime.Value : DateTime.MinValue;
            var playerName = player.DisplayName;
            var checkpointItems = playerResult.Checkpoints.Select(o => CreateCheckpointItem(bunch, isManager, o)).ToList();

            return new Result(date, playerName, bunch.Slug, checkpointItems);
        }

        private static CheckpointItem CreateCheckpointItem(Bunch bunch, bool isManager, Checkpoint checkpoint)
        {
            var type = checkpoint.Description;
            var displayAmount = new Money(GetDisplayAmount(checkpoint), bunch.Currency);
            var time = TimeZoneInfo.ConvertTime(checkpoint.Timestamp, bunch.Timezone);
            var canEdit = isManager;

            return new CheckpointItem(time, checkpoint.Id, type, displayAmount, canEdit);
        }

        private static int GetDisplayAmount(Checkpoint checkpoint)
        {
            if (checkpoint.Type == CheckpointType.Buyin)
                return checkpoint.Amount;
            return checkpoint.Stack;
        }

        public class Request
        {
            public string CurrentUserName { get; }
            public int CashgameId { get; }
            public int PlayerId { get; }

            public Request(string currentUserName, int cashgameId, int playerId)
            {
                CurrentUserName = currentUserName;
                CashgameId = cashgameId;
                PlayerId = playerId;
            }
        }

        public class Result
        {
            public DateTime Date { get; private set; }
            public string PlayerName { get; private set; }
            public string Slug { get; private set; }
            public IList<CheckpointItem> CheckpointItems { get; private set; }

            public Result(DateTime date, string playerName, string slug, List<CheckpointItem> checkpointItems)
            {
                Date = date;
                PlayerName = playerName;
                Slug = slug;
                CheckpointItems = checkpointItems;
            }
        }

        public class CheckpointItem
        {
            public DateTime Time { get; private set; }
            public int CheckpointId { get; private set; }
            public string Type { get; private set; }
            public Money DisplayAmount { get; private set; }
            public bool CanEdit { get; private set; }

            public CheckpointItem(DateTime time, int checkpointId, string type, Money displayAmount, bool canEdit)
            {
                Time = time;
                CheckpointId = checkpointId;
                Type = type;
                DisplayAmount = displayAmount;
                CanEdit = canEdit;
            }
        }
    }
}