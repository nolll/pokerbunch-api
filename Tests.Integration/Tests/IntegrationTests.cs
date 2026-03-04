using Infrastructure.Sql.Models;
using Tests.Common;
using Tests.Common.FakeServices;
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

    public TestFixture Fixture { get; set; }
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
        Db.PbCashgameCheckpoint.RemoveRange(Db.PbCashgameCheckpoint);
        Db.PbEvent.RemoveRange(Db.PbEvent);
        Db.PbCashgame.RemoveRange(Db.PbCashgame);
        Db.PbLocation.RemoveRange(Db.PbLocation);
        Db.PbPlayer.RemoveRange(Db.PbPlayer);
        Db.PbJoinRequest.RemoveRange(Db.PbJoinRequest);
        Db.PbBunch.RemoveRange(Db.PbBunch);
        Db.PbUser.RemoveRange(Db.PbUser);
        await Db.SaveChangesAsync();
    }

    public async ValueTask InitializeAsync()
    {
        await Task.CompletedTask;
    }
}
