using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infrastructure.Sql;
using Tests.Common.FakeServices;

namespace Tests.Integration;

[SetUpFixture]
public class TestSetup
{
    private const DbEngine Engine = DbEngine.Sqlite;

    private const string SqliteConnectionString = "DataSource=IntegrationTests;Mode=Memory;Cache=Shared";
    
    private TestcontainerDatabase _testcontainers;
    private static WebApplicationFactoryInTest _webApplicationFactory;

    public static FakeEmailSender EmailSender;
    public static IDb Db;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        Db = await InitDbEngine();
        EmailSender = new FakeEmailSender();
        _webApplicationFactory = new WebApplicationFactoryInTest(EmailSender, Db);
        await CreateTables();
        await AddMasterData();
    }

    private async Task<IDb> InitDbEngine()
    {
        return Engine == DbEngine.Postgres
            ? await InitPostgresEngine()
            : InitSqliteEngine();
    }

    private async Task<IDb> InitPostgresEngine()
    {
        _testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "db",
                Username = "postgres",
                Password = "postgres",
                Port = 49262
            })
            .Build();
        await _testcontainers.StartAsync();
        return new PostgresDb(_testcontainers.ConnectionString);
    }

    private IDb InitSqliteEngine() => new SqliteDb(SqliteConnectionString);

    private async Task DestroyDbEngine()
    {
        Db.Dispose();
        if(Engine == DbEngine.Postgres)
            await DestroyPostgresEngine();
    }

    private async Task DestroyPostgresEngine() => await _testcontainers.DisposeAsync().AsTask();

    public static HttpClient GetClient(string token = null)
    {
        var client = _webApplicationFactory.CreateClient();
        if(token != null)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }

    private static async Task CreateTables() => await Db.Execute(CreateScript);
    private static async Task AddMasterData() => await Db.Execute(GetMasterDataSql);

    [OneTimeTearDown]
    public async Task TearDown() => await DestroyDbEngine();

    private static string CreateScriptName => Engine == DbEngine.Postgres
        ? "db-create-postgres.sql"
        : "db-create-sqlite.sql";
    private static string CreateScript => ReadSqlFile($"data/{CreateScriptName}");
    private static string GetMasterDataSql => ReadSqlFile("data/db-add-master-data.sql");

    private static string ReadSqlFile(string fileName)
    {
        var path = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            fileName);
        return File.ReadAllText(path);
    }
}