using Infrastructure.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Common.FakeServices;
using Tests.Integration.Fixtures;
using Xunit;

namespace Tests.Integration.Tests;

[Collection(nameof(TestFixture))]
[TestCaseOrderer(typeof(TestSorter))]
public partial class IntegrationTests : IAsyncLifetime
{
    public IntegrationTests(TestFixture fixture)
    {
        Fixture = fixture;
        Db = fixture.Db;
        Data = fixture.Data;
        DataFactory = fixture.DataFactory;
        LoginHelper = fixture.LoginHelper;
        ApiClient = fixture.ApiClient;
        EmailSender = fixture.EmailSender;
    }

    public TestFixture Fixture { get; }
    private PokerBunchDbContext Db { get; }
    private TestData Data { get; }
    private TestDataFactory DataFactory { get; }
    private LoginHelper LoginHelper { get; }
    private ApiClientForTest ApiClient { get; }
    private FakeEmailSender EmailSender { get; }

    private static class ActionType
    {
        public const string Report = "report";
        public const string Buyin = "buyin";
        public const string Cashout = "cashout";
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

    public async ValueTask InitializeAsync()
    {
        await Task.CompletedTask;
    }
}
