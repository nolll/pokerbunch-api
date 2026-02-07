using Api.Settings;
using Api.Urls.ApiUrls;
using Core;
using Core.Cache;
using Core.Repositories;
using Core.Services;
using Core.UseCases;
using Infrastructure.Cache;
using Infrastructure.Email;
using Infrastructure.Sql;
using Infrastructure.Sql.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Api.Bootstrapping;

public static class DependencyInjection
{
    public static void AddServices(
        this IServiceCollection services,
        AppSettings settings,
        ConfigurationManager configuration)
    {
        var connectionString = GetConnectionString(configuration);
        
        services.AddSingleton(settings);
        services.AddHttpContextAccessor();
        services.AddTransient<IAuth, Auth.Auth>();
        services.AddSingleton<ISettings>(new Core.Settings(settings.InvitationSecret));
        services.AddSingleton(new UrlProvider(settings.Urls.Api, settings.Urls.Site));

        services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
        services.AddSingleton<ICache, Cache>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IBunchRepository, BunchRepository>();
        services.AddSingleton<ICashgameRepository, CashgameRepository>();
        services.AddSingleton<IEventRepository, EventRepository>();
        services.AddSingleton<ILocationRepository, LocationRepository>();
        services.AddSingleton<IPlayerRepository, PlayerRepository>();
        services.AddSingleton<IJoinRequestRepository, JoinRequestRepository>();
        services.AddSingleton(GetEmailSender(configuration));
        services.AddSingleton<IDb>(new PostgresDb(connectionString));
        services.AddSingleton<IRandomizer, Randomizer>();
        services.AddSingleton<IInvitationCodeCreator, InvitationCodeCreator>();

        // Admin
        services.AddSingleton<ClearCache>();
        services.AddSingleton<TestEmail>();

        // Auth
        services.AddSingleton<Login>();
        services.AddSingleton<Refresh>();

        // User
        services.AddSingleton<UserDetails>();
        services.AddSingleton<UserList>();
        services.AddSingleton<EditUser>();
        services.AddSingleton<AddUser>();
        services.AddSingleton<ChangePassword>();
        services.AddSingleton<ResetPassword>();

        // Bunch
        services.AddSingleton<GetBunchList>();
        services.AddSingleton<GetBunchListForUser>();
        services.AddSingleton<GetBunch>();
        services.AddSingleton<AddBunch>();
        services.AddSingleton<EditBunch>();
        
        // Join requests
        services.AddSingleton<AddJoinRequest>();
        services.AddSingleton<ListJoinRequests>();

        // Events
        services.AddSingleton<EventDetails>();
        services.AddSingleton<EventList>();
        services.AddSingleton<AddEvent>();

        // Locations
        services.AddSingleton<GetLocationList>();
        services.AddSingleton<GetLocation>();
        services.AddSingleton<AddLocation>();

        // Cashgame
        services.AddSingleton<CashgameList>();
        services.AddSingleton<EventCashgameList>();
        services.AddSingleton<PlayerCashgameList>();
        services.AddSingleton<CurrentCashgames>();
        services.AddSingleton<CashgameDetails>();
        services.AddSingleton<Buyin>();
        services.AddSingleton<Report>();
        services.AddSingleton<Cashout>();
        services.AddSingleton<AddCashgame>();
        services.AddSingleton<EditCashgame>();
        services.AddSingleton<DeleteCashgame>();
        services.AddSingleton<EditCheckpoint>();
        services.AddSingleton<DeleteCheckpoint>();

        // Player
        services.AddSingleton<GetPlayer>();
        services.AddSingleton<GetPlayerList>();
        services.AddSingleton<AddPlayer>();
        services.AddSingleton<DeletePlayer>();
        services.AddSingleton<InvitePlayer>();
        services.AddSingleton<JoinBunch>();
    }

    private static IEmailSender GetEmailSender(IConfiguration configuration)
    {
        var host = configuration.GetValue<string>("SMTP_SERVER") ?? "localhost";
        var strPort = configuration.GetValue<string>("SMTP_PORT");
        var port = strPort != null ? int.Parse(strPort) : 25;
        var login = configuration.GetValue<string>("SMTP_LOGIN");
        var password = configuration.GetValue<string>("SMTP_PASSWORD");
        return new SmtpEmailSender(host, port, login, password);
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var databaseUrl = configuration.GetValue<string>("DATABASE_URL");
        if (string.IsNullOrEmpty(databaseUrl))
            return "";

        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/')
        };

        return connectionStringBuilder.ToString();
    }
}