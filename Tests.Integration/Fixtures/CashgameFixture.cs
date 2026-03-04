using Api.Models;
using Api.Models.CashgameModels;
using Tests.Common;

namespace Tests.Integration.Fixtures;

public class CashgameFixture(ApiClientForTest apiClient, TestDataFactory dataFactory, CashgameDetailsModel resultModel)
{
    public string Id { get; } = resultModel.Id;
    public string LocationId { get; } = resultModel.Location.Id;

    public async Task<CashgameFixture> Play(UserFixture manager, params PlayerFixture[] players)
    {
        foreach (var player in players)
        {
            await Buyin(manager.Token, Id, player.Id);
            await Cashout(manager.Token, Id, player.Id);
        }

        return this;
    }

    public async Task<CashgameFixture> AddToEvent(string token, EventFixture @event)
    {
        var parameters = new UpdateCashgamePostModel(LocationId, @event.Id);
        await apiClient.Cashgame.Update(token, Id, parameters);
        return this;
    }
    
    private async Task Buyin(string? token, string cashgameId, string playerId, int? buyin = null, int leftInStack = 0)
    {
        var parameters = new AddCashgameActionPostModel(ActionType.Buyin, playerId, buyin ?? dataFactory.Int(), leftInStack);
        await apiClient.Action.Add(token, cashgameId, parameters);
    }

    private async Task Cashout(string? token, string cashgameId, string playerId, int? stack = null)
    {
        var parameters = new AddCashgameActionPostModel(ActionType.Cashout, playerId, 0, stack ?? dataFactory.Int());
        await apiClient.Action.Add(token, cashgameId, parameters);
    }
}