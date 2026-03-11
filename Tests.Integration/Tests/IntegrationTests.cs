using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Common.FakeServices;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

[Collection(nameof(TestFixture))]
public abstract class IntegrationTests(TestFixture fixture) : IAsyncLifetime
{
    protected TestFixture Fixture { get; } = fixture;
    protected PokerBunchDbContext Db { get; } = fixture.Db;
    protected TestDataFactory Data { get; } = fixture.DataFactory;
    protected ApiClientForTest ApiClient { get; } = fixture.ApiClient;
    protected FakeEmailSender EmailSender { get; } = fixture.EmailSender;

    protected static class ActionType
    {
        public const string Report = "report";
        public const string Buyin = "buyin";
        public const string Cashout = "cashout";
    }
    
    public async ValueTask InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await ClearDatabase();
    }

    private async Task ClearDatabase()
    {
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_cashgame_checkpoint");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_event_cashgame");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_event");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_cashgame");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_location");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_player");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_join_request");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_bunch");
        await Db.Database.ExecuteSqlRawAsync("DELETE FROM pb_user");
    }
}
