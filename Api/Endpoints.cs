using Api.Handlers;
using Api.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Api;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapPost(ApiRoutes.Admin.ClearCache, ClearCacheHandler.Handle)
            .RequireAuthorization()
            .ExcludeFromDescription();
        
        app.MapPost(ApiRoutes.Admin.SendEmail, SendEmailHandler.Handle)
            .RequireAuthorization()
            .ExcludeFromDescription();
        
        app.MapGet(ApiRoutes.Version, VersionHandler.Handle)
            .ExcludeFromDescription();
        
        app.MapGet(ApiRoutes.Settings, SettingsHandler.Handle)
            .RequireAuthorization()
            .ExcludeFromDescription();

        app.MapGet(ApiRoutes.User.Get, GetUserHandler.Handle)
            .RequireAuthorization()
            .WithSummary("GetUser");

        app.MapPost(ApiRoutes.Action.Add, AddActionHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Add cashgame action")
            .WithDescription("Add a player action to a cashgame");

        app.MapPut(ApiRoutes.Action.Update, UpdateActionHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Update player action");

        app.MapDelete(ApiRoutes.Action.Delete, DeleteActionHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Delete cashgame action")
            .WithDescription("Remove a player action from a cashgame");

        app.Map(ApiRoutes.Error, ErrorHandler.Handle)
            .ExcludeFromDescription();

        app.MapGet(ApiRoutes.Root, RootHandler.Handle)
            .ExcludeFromDescription();

        app.MapGet(ApiRoutes.Bunch.Get, GetBunchHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Get bunch");

        app.MapPut(ApiRoutes.Bunch.Update, UpdateBunchHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Update bunch");

        app.MapGet(ApiRoutes.Bunch.List, GetBunchListHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List bunches");

        app.MapGet(ApiRoutes.Bunch.ListForCurrentUser, GetBunchListForCurrentUserHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List your bunches");

        app.MapPost(ApiRoutes.Bunch.Add, AddBunchHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Add bunch");

        app.MapPost(ApiRoutes.Bunch.Join, JoinBunchHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Join bunch");

        app.MapGet(ApiRoutes.Cashgame.Get, GetCashgameHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Get cashgame");

        app.MapGet(ApiRoutes.Cashgame.ListByBunch, GetCashgameListByBunchHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List cashgames");

        app.MapGet(ApiRoutes.Cashgame.ListByBunchAndYear, GetCashgameListByBunchAndYearHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List cashgames by year");

        app.MapGet(ApiRoutes.Cashgame.ListByEvent, GetCashgameListByEventHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List cashgames by event");

        app.MapGet(ApiRoutes.Cashgame.ListByPlayer, GetCashgameListByPlayerHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List cashgames by player");

        app.MapPost(ApiRoutes.Cashgame.Add, AddCashgameHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Add cashgame");

        app.MapPut(ApiRoutes.Cashgame.Update, UpdateCashgameHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Update cashgame");

        app.MapDelete(ApiRoutes.Cashgame.Delete, DeleteCashgameHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Delete cashgame");

        app.MapGet(ApiRoutes.Cashgame.ListCurrentByBunch, GetCurrentCashgamesHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List running cashgames");

        app.MapGet(ApiRoutes.Event.Get, GetEventHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Get event");

        app.MapGet(ApiRoutes.Event.ListByBunch, GetEventListHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List events");

        app.MapPost(ApiRoutes.Event.Add, AddEventHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Add event");
        
        
        app.MapGet(ApiRoutes.Location.Get, GetLocationHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Get location");

        app.MapGet(ApiRoutes.Location.ListByBunch, GetLocationListHandler.Handle)
            .RequireAuthorization()
            .WithSummary("List locations");

        app.MapPost(ApiRoutes.Location.Add, AddLocationHandler.Handle)
            .RequireAuthorization()
            .WithSummary("Add location");
    }
}
