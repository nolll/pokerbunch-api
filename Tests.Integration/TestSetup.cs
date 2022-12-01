using Infrastructure.Sql;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using Tests.Common.FakeServices;

namespace Tests.Integration;

[SetUpFixture]
public class TestSetup
{
    private static WebApplicationFactoryInTest _webApplicationFactory;
    
    public static FakeEmailSender EmailSender;
    public static IDb Db;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        const string connectionString = "DataSource=IntegrationTests;Mode=Memory;Cache=Shared";
        Db = new SqliteDb(connectionString);
        EmailSender = new FakeEmailSender();
        _webApplicationFactory = new WebApplicationFactoryInTest(EmailSender, Db);
        await DropTables();
        await CreateTables();
        await ReadUsers(connectionString); 
        await AddMasterData();
    }
    
    public static HttpClient GetClient(string token = null)
    {
        var client = _webApplicationFactory.CreateClient();
        if(token != null)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }

    private static async Task DropTables()
    {
        await Db.Execute(DropScript);
    }

    private static async Task CreateTables()
    {
        await Db.Execute(CreateScript);
    }

    private static async Task AddMasterData()
    {
        await Db.Execute(GetMasterDataSql);
    }

    private async Task ReadUsers(string connectionString)
    {
        await using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var command = new SqliteCommand("SELECT * FROM pb_user", connection);
        var reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            var ordinal = reader.GetOrdinal("user_name");
            var value = reader.IsDBNull(ordinal) ? default : reader.GetString(ordinal);
            Console.WriteLine(value);
        }
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        Db.Dispose();
    }

    private static string DropScript => ReadSqlFile("data/db-drop.sql");
    private static string CreateScript => ReadSqlFile("data/db-create-sqlite.sql");
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