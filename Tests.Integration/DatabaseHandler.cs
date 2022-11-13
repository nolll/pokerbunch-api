using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infrastructure.Sql;

namespace Tests.Integration;

[SetUpFixture]
public class DatabaseHandler
{
    private static readonly TestcontainerDatabase Testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Username = "postgres",
            Password = "postgres",
            Port = 49162
        })
        .Build();

    public static string ConnectionString => Testcontainers.ConnectionString;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        await Testcontainers.StartAsync();
        CreateTables();
        AddMasterData();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await Testcontainers.DisposeAsync().AsTask();
    }

    private static void CreateTables()
    {
        var db = new PostgresStorageProvider(ConnectionString);
        db.Execute(CreateScript);
    }

    private static void AddMasterData()
    {
        var db = new PostgresStorageProvider(ConnectionString);
        db.Execute(GetMasterDataSql);
    }

    private static string CreateScript => ReadSqlFile("db-create.sql");
    private static string GetMasterDataSql => ReadSqlFile("db-add-master-data.sql");

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