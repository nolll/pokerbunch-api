using Infrastructure.Sql;
using Microsoft.Data.Sqlite;
using Tests.Common.FakeServices;

namespace Tests.Integration;

[SetUpFixture]
public class TestSetup
{
    private static WebApplicationFactoryInTest _webApplicationFactory;
    private static SqliteConnection _dbConnection;
    
    public static FakeEmailSender EmailSender;
    public static IDb Db;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        const string connectionString = "Data Source=:memory:";
        _dbConnection = new SqliteConnection(connectionString);
        await _dbConnection.OpenAsync();
        Db = new SqliteDb(_dbConnection);
        EmailSender = new FakeEmailSender();
        _webApplicationFactory = new WebApplicationFactoryInTest(connectionString, EmailSender);
        await CreateTables();
        await AddMasterData();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _dbConnection.CloseAsync();
    }

    public static HttpClient GetClient(string token = null)
    {
        var client = _webApplicationFactory.CreateClient();
        if(token != null)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }
    
    private static async Task CreateTables()
    {
        await Db.Execute(CreateScript);
    }

    private static async Task AddMasterData()
    {
        await Db.Execute(GetMasterDataSql);
    }

    private static string CreateScript => ReadSqlFile("data/db-create.sql");
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