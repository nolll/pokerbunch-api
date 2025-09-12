using Api.Handlers;
using Api.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Api;

public static class Endpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
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
    }
}
