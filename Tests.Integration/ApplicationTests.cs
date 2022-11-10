using Infrastructure.Sql;

namespace Tests.Integration;

public class ApplicationTests
{
    private WebApplicationFactoryInTest _webApplicationFactory;

    private const string UserName = "user1";
    private const string UserDisplayName = "User 1";
    private const string Email = "user1@example.org";
    private const string Password = "password";
    private const string LoginUrl = "/login";

    private const string BunchDisplayName = "Bunch 1";
    private const string BunchSlug = "bunch-1";
    private const string BunchDescription = "Bunch Description 1";
    private const string CurrencySymbol = "$";
    private const string CurrencyLayout = "{SYMBOL}{AMOUNT}";
    private const string TimeZone = "Europe/Stockholm";

    [Test]
    public async Task TestEverything()
    {
        _webApplicationFactory = new WebApplicationFactoryInTest(DatabaseHandler.ConnectionString);

        VerifyMasterData();
        await VersionReturns200();
        //Register();
        //Login();
        //CreateBunch();
    }

    private async Task VersionReturns200()
    {
        var client = _webApplicationFactory.CreateClient();
        var response = await client.GetAsync("/version");
        response.EnsureSuccessStatusCode();
    }

    private void VerifyMasterData()
    {
        var db = new PostgresStorageProvider(DatabaseHandler.ConnectionString);
        var reader = db.Query("SELECT role_id, role_name FROM pb_role ORDER BY role_id");
        var roles = reader.ReadList(RoleInTest.Create);

        Assert.That(roles[0].Id, Is.EqualTo(1));
        Assert.That(roles[0].Name, Is.EqualTo("Player"));
        Assert.That(roles[1].Id, Is.EqualTo(2));
        Assert.That(roles[1].Name, Is.EqualTo("Manager"));
        Assert.That(roles[2].Id, Is.EqualTo(3));
        Assert.That(roles[2].Name, Is.EqualTo("Admin"));
    }

    //private void Register()
    //{
    //    var addUser = new AddUser(UserRepository, Randomizer, EmailSender);

    //    addUser.Execute(new AddUser.Request(UserName, UserDisplayName, Email, Password, LoginUrl));
    //}

    //private void Login()
    //{
    //    var login = new Login(UserRepository);

    //    var result = login.Execute(new Login.Request(UserName, Password));

    //    Assert.That(result.UserName, Is.EqualTo(UserName));
    //}

    //private void CreateBunch()
    //{
    //    var addBunch = new AddBunch(UserRepository, BunchRepository, PlayerRepository);

    //    var result = addBunch.Execute(new AddBunch.Request(UserName, BunchDisplayName, BunchDescription, CurrencySymbol, CurrencyLayout, TimeZone));

    //    Assert.That(result.Name, Is.EqualTo(BunchDisplayName));
    //    Assert.That(result.Slug, Is.EqualTo(BunchSlug));
    //    Assert.That(result.DefaultBuyin, Is.EqualTo(200));
    //}
}