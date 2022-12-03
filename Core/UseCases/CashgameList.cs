using System;
using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CashgameList : UseCase<CashgameList.Request, CashgameList.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly ICashgameRepository _cashgameRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILocationRepository _locationRepository;

    public CashgameList(IBunchRepository bunchRepository, ICashgameRepository cashgameRepository, IUserRepository userRepository, IPlayerRepository playerRepository, ILocationRepository locationRepository)
    {
        _bunchRepository = bunchRepository;
        _cashgameRepository = cashgameRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
        _locationRepository = locationRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await _bunchRepository.GetBySlug(request.Slug);
        var user = await _userRepository.GetByUserName(request.UserName);
        var player = await _playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanListCashgames(user, player))
            return Error(new AccessDeniedError());
        
        var cashgames = (await _cashgameRepository.GetFinished(bunch.Id, request.Year)).OrderByDescending(o => o.StartTime);
        var locations = await _locationRepository.List(bunch.Id);
        var players = await _playerRepository.List(bunch.Id);
        var items = cashgames.Select(o => new Item(o, GetLocation(o, locations), players));

        return Success(new Result(request.Slug, items.ToList()));
    }
    
    private Location GetLocation(Cashgame cashgame, IEnumerable<Location> locations)
    {
        return locations.First(o => o.Id == cashgame.LocationId);
    }
    
    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }
        public int? Year { get; }

        public Request(string userName, string slug, int? year)
        {
            UserName = userName;
            Slug = slug;
            Year = year;
        }
    }

    public class Result
    {
        public IList<Item> Items { get; }
        public string Slug { get; }

        public Result(string slug, IList<Item> items)
        {
            Slug = slug;
            Items = items;
        }
    }

    public class Item
    {
        public string LocationId { get; }
        public string LocationName { get; }
        public string CashgameId { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public IList<ItemResult> Results { get; }

        public Item(Cashgame cashgame, Location location, IList<Player> players)
        {
            LocationId = location.Id;
            LocationName = location.Name;
            CashgameId = cashgame.Id;
            StartTime = cashgame.StartTime ?? DateTime.MinValue;
            EndTime = cashgame.EndTime ?? DateTime.MinValue;
            Results = cashgame.Results.Select(o => new ItemResult(o, GetPlayerName(o.PlayerId, players))).ToList();
        }
    }

    private static string GetPlayerName(string playerId, IList<Player> players)
    {
        var player = players.FirstOrDefault(o => o.Id == playerId);
        return player?.DisplayName ?? "";
    }
        
    public class ItemResult
    {
        public ItemPlayer Player { get; }
        public DateTime BuyinTime { get; }
        public DateTime UpdatedTime { get; }
        public int Buyin { get; }
        public int Stack { get; }

        public ItemResult(CashgameResult result, string playerName)
        {
            Player = new ItemPlayer(result.PlayerId, playerName);
            BuyinTime = result.BuyinTime ?? DateTime.MinValue;
            UpdatedTime = result.LastReportTime;
            Buyin = result.Buyin;
            Stack = result.Stack;
        }
    }

    public class ItemPlayer
    {
        public string Id { get; }
        public string Name { get; }

        public ItemPlayer(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}