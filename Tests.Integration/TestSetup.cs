using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infrastructure.Sql;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Common.FakeServices;

namespace Tests.Integration;

[SetUpFixture]
public class TestSetup
{
    private static readonly TestcontainerDatabase Testcontainers = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "db",
            Username = "postgres",
            Password = "postgres",
            Port = 49262
        })
        .Build();

    public static string ConnectionString => Testcontainers.ConnectionString;
    public static FakeEmailSender EmailSender;
    private static WebApplicationFactoryInTest _webApplicationFactory;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        await Testcontainers.StartAsync();
        EmailSender = new FakeEmailSender();
        _webApplicationFactory = new WebApplicationFactoryInTest(ConnectionString, EmailSender);
        CreateTables();
        AddMasterData();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await Testcontainers.DisposeAsync().AsTask();
    }

    public static HttpClient GetClient(string token = null)
    {
        var client = _webApplicationFactory.CreateClient();
        if(token != null)
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
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