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
    }
}
