using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class Actions
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IUserRepository _userRepository;

    public Actions(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IPlayerRepository playerRepository, IUserRepository userRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _playerRepository = playerRepository;
        _userRepository = userRepository;
    }

    public Result Execute(Request request)
    {
        var player = _playerRepository.Get(request.PlayerId);
        var user = _userRepository.Get(request.CurrentUserName);
        var bunch = _bunchRepository.Get(player.BunchId);
        var cashgame = _cashgameRepository.Get(request.CashgameId);
            
        RequireRole.Player(user, player);
        var playerResult = cashgame.GetResult(player.Id);
        var currentPlayer = _playerRepository.Get(bunch.Id, user.Id);
        var canEdit = RoleHandler.IsInRole(user, currentPlayer, Role.Manager);

        var date = cashgame.StartTime ?? DateTime.MinValue;
        var playerName = player.DisplayName;
        var checkpointItems = playerResult.Checkpoints.Select(o => CreateCheckpointItem(bunch, canEdit, o)).ToList();

        return new Result(date, playerName, bunch.Slug, checkpointItems);
    }

    private static CheckpointItem CreateCheckpointItem(Bunch bunch, bool canEdit, Checkpoint checkpoint)
    {
        var type = checkpoint.Description;
        var displayAmount = new Money(GetDisplayAmount(checkpoint), bunch.Currency);
        var time = TimeZoneInfo.ConvertTime(checkpoint.Timestamp, bunch.Timezone);
        
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
        public DateTime Date { get; }
        public string PlayerName { get; }
        public string Slug { get; }
        public IList<CheckpointItem> CheckpointItems { get; }

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
        public DateTime Time { get; }
        public int CheckpointId { get; }
        public string Type { get; }
        public Money DisplayAmount { get; }
        public bool CanEdit { get; }

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