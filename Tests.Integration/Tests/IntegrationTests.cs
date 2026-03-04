using Infrastructure.Sql.Models;
using Tests.Common;
using Tests.Common.FakeServices;
using Xunit;

namespace Tests.Integration.Tests;

[Collection(nameof(TestFixture))]
[TestCaseOrderer(typeof(TestSorter))]
public partial class IntegrationTests(TestFixture fixture)
{
    private PokerBunchDbContext Db { get; } = fixture.Db;
    private TestData Data { get; } = fixture.Data;
    private TestDataFactory DataFactory { get; } = fixture.DataFactory;
    private LoginHelper LoginHelper { get; } = fixture.LoginHelper;
    private ApiClientForTest ApiClient { get; } = fixture.ApiClient;
    private FakeEmailSender EmailSender { get; } = fixture.EmailSender;

    private static class ActionType
    {
        public const string Report = "report";
        public const string Buyin = "buyin";
        public const string Cashout = "cashout";
    }
}
