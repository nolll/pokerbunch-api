using System;
using System.Linq;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class CashgameList(
    IBunchRepository bunchRepository,
    ICashgameRepository cashgameRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository,
    ILocationRepository locationRepository)
    : UseCase<CashgameList.Request, CashgameList.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var user = await userRepository.GetByUserName(request.UserName);
        var player = await playerRepository.Get(bunch.Id, user.Id);

        if (!AccessControl.CanListCashgames(user, player))
            return Error(new AccessDeniedError());
        
        var cashgames = (await cashgameRepository.GetFinished(bunch.Id, request.Year)).OrderByDescending(o => o.StartTime);
        var locations = await locationRepository.List(bunch.Id);
        var players = await playerRepository.List(bunch.Id);
        var items = cashgames.Select(o => new Item(o, GetLocation(o, locations), players));

        return Success(new Result(request.Slug, items.ToList()));
    }
    
    private static Location GetLocation(Cashgame cashgame, IEnumerable<Location> locations) => 
        locations.First(o => o.Id == cashgame.LocationId);

    public record Request(string UserName, string Slug, int? Year);
    public record Result(string Slug, IList<Item> Items);

    public class Item(Cashgame cashgame, Location location, IList<Player> players)
    {
        public string LocationId { get; } = location.Id;
        public string LocationName { get; } = location.Name;
        public string CashgameId { get; } = cashgame.Id;
        public DateTime StartTime { get; } = cashgame.StartTime ?? DateTime.MinValue;
        public DateTime EndTime { get; } = cashgame.EndTime ?? DateTime.MinValue;
        public IList<ItemResult> Results { get; } = cashgame.Results.Select(o => new ItemResult(o, GetPlayerName(o.PlayerId, players))).ToList();
    }

    private static string GetPlayerName(string playerId, IEnumerable<Player> players)
    {
        var player = players.FirstOrDefault(o => o.Id == playerId);
        return player?.DisplayName ?? "";
    }
        
    public class ItemResult(CashgameResult result, string playerName)
    {
        public ItemPlayer Player { get; } = new(result.PlayerId, playerName);
        public DateTime BuyinTime { get; } = result.BuyinTime ?? DateTime.MinValue;
        public DateTime UpdatedTime { get; } = result.LastReportTime;
        public int Buyin { get; } = result.Buyin;
        public int Stack { get; } = result.Stack;
    }

    public record ItemPlayer(string Id, string Name);
}