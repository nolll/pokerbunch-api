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

namespace Api.Bootstrapping;

public static class DependencyInjection
{
    public static void AddServices(
        this IServiceCollection services,
        AppSettings settings,
        ConfigurationManager configuration,
        string connectionString)
    {
        services.AddTransient(_ => settings);
        services.AddHttpContextAccessor();
        services.AddTransient<IAuth, Auth.Auth>();
        services.AddTransient<ISettings>(_ => new Core.Settings(settings.InvitationSecret));
        services.AddTransient(_ => new UrlProvider(settings.Urls.Api, settings.Urls.Site));

        services.AddTransient<ICacheProvider, MemoryCacheProvider>();
        services.AddTransient<ICache, Cache>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IBunchRepository, BunchRepository>();
        services.AddTransient<ICashgameRepository, CashgameRepository>();
        services.AddTransient<IEventRepository, EventRepository>();
        services.AddTransient<ILocationRepository, LocationRepository>();
        services.AddTransient<IPlayerRepository, PlayerRepository>();
        services.AddTransient<IJoinRequestRepository, JoinRequestRepository>();
        services.AddTransient(_ => GetEmailSender(configuration));
        services.AddTransient<IDb>(_ => new PostgresDb(connectionString));
        services.AddTransient<IRandomizer, Randomizer>();

        // Admin
        services.AddTransient<ClearCache>();
        services.AddTransient<TestEmail>();

        // Auth
        services.AddTransient<Login>();
        services.AddTransient<Refresh>();

        // User
        services.AddTransient<UserDetails>();
        services.AddTransient<UserList>();
        services.AddTransient<EditUser>();
        services.AddTransient<AddUser>();
        services.AddTransient<ChangePassword>();
        services.AddTransient<ResetPassword>();

        // Bunch
        services.AddTransient<GetBunchList>();
        services.AddTransient<GetBunchListForUser>();
        services.AddTransient<GetBunch>();
        services.AddTransient<AddBunch>();
        services.AddTransient<EditBunch>();
        
        // Join requests
        services.AddTransient<AddJoinRequest>();
        services.AddTransient<ListJoinRequests>();
        services.AddTransient<AcceptJoinRequest>();
        services.AddTransient<DenyJoinRequest>();

        // Events
        services.AddTransient<EventDetails>();
        services.AddTransient<EventList>();
        services.AddTransient<AddEvent>();

        // Locations
        services.AddTransient<GetLocationList>();
        services.AddTransient<GetLocation>();
        services.AddTransient<AddLocation>();

        // Cashgame
        services.AddTransient<CashgameList>();
        services.AddTransient<EventCashgameList>();
        services.AddTransient<PlayerCashgameList>();
        services.AddTransient<CurrentCashgames>();
        services.AddTransient<CashgameDetails>();
        services.AddTransient<Buyin>();
        services.AddTransient<Report>();
        services.AddTransient<Cashout>();
        services.AddTransient<AddCashgame>();
        services.AddTransient<EditCashgame>();
        services.AddTransient<DeleteCashgame>();
        services.AddTransient<EditCheckpoint>();
        services.AddTransient<DeleteCheckpoint>();

        // Player
        services.AddTransient<GetPlayer>();
        services.AddTransient<GetPlayerList>();
        services.AddTransient<AddPlayer>();
        services.AddTransient<DeletePlayer>();
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
}