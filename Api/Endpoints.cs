using System.Collections.Generic;
using Api.Handlers;
using Api.Models.BunchModels;
using Api.Models.CashgameModels;
using Api.Models.CommonModels;
using Api.Models.EventModels;
using Api.Models.JoinRequestModels;
using Api.Models.LocationModels;
using Api.Models.PlayerModels;
using Api.Models.UserModels;
using Api.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Api;

public static class Endpoints
{
    private static class Tags
    {
        public const string Auth = "Auth";
        public const string Users = "Users";
        public const string Bunches = "Bunches";
        public const string JoinRequests = "Join requests";
        public const string Cashgames = "Cashgames";
        public const string CashgameActions = "Cashgame actions";
        public const string Players = "Players";
        public const string Locations = "Locations";
        public const string Events = "Events";
    }
    
    public static void Map(this WebApplication app)
    {
        MapAuthEndpoints(app);
        MapUserEndpoints(app);
        MapBunchEndpoints(app);
        MapJoinRequestEndpoints(app);
        MapCashgameEndpoints(app);
        MapCashgameActionEndpoints(app);
        MapPlayerEndpoints(app);
        MapLocationEndpoints(app);
        MapEventEndpoints(app);
        MapGeneralEndpoints(app);
        MapAdminEndpoints(app);
    }

    private static void MapAuthEndpoints(WebApplication app)
    {
        app.MapPost(ApiRoutes.Auth.Login, LoginHandler.Handle)
            .WithTags(Tags.Auth)
            .Produces<LoginModel>()
            .WithSummary("Get an auth token")
            .WithDescription("Get a token that can bu used for authentication");
        
        app.MapPost(ApiRoutes.Auth.Refresh, RefreshHandler.Handle)
            .WithTags(Tags.Auth)
            .Produces<LoginModel>()
            .WithSummary("Refresh auth token")
            .WithDescription("Get a new access token with a valid refresh token");
    }

    private static void MapUserEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.User.Get, GetUserHandler.Handle)
            .WithTags(Tags.Users)
            .Produces<FullUserModel>()
            .RequireAuthorization()
            .WithSummary("GetUser");
        
        app.MapGet(ApiRoutes.User.List, GetUserListHandler.Handle)
            .WithTags(Tags.Users)
            .Produces<IEnumerable<UserItemModel>>()
            .RequireAuthorization()
            .WithSummary("List users");
        
        app.MapPut(ApiRoutes.User.Update, UpdateUserHandler.Handle)
            .WithTags(Tags.Users)
            .Produces<FullUserModel>()
            .RequireAuthorization()
            .WithSummary("Update user");
        
        app.MapPost(ApiRoutes.User.Add, AddUserHandler.Handle)
            .WithTags(Tags.Users)
            .Produces<MessageModel>()
            .WithSummary("Add user");

        app.MapGet(ApiRoutes.Profile.Get, GetCurrentUserHandler.Handle)
            .WithTags(Tags.Users)
            .Produces<FullUserModel>()
            .RequireAuthorization()
            .WithSummary("Get authenticated user");
        
        app.MapPut(ApiRoutes.Profile.ChangePassword, ChangePasswordHandler.Handle)
            .WithTags(Tags.Users)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Change password");

        app.MapPost(ApiRoutes.Profile.ResetPassword, ResetPasswordHandler.Handle)
            .WithTags(Tags.Users)
            .Produces<MessageModel>()
            .WithSummary("Reset password");
    }

    private static void MapBunchEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.Bunch.Get, GetBunchHandler.Handle)
            .WithTags(Tags.Bunches)
            .Produces<BunchModel>()
            .RequireAuthorization()
            .WithSummary("Get bunch");

        app.MapPut(ApiRoutes.Bunch.Update, UpdateBunchHandler.Handle)
            .WithTags(Tags.Bunches)
            .Produces<BunchModel>()
            .RequireAuthorization()
            .WithSummary("Update bunch");

        app.MapGet(ApiRoutes.Bunch.List, GetBunchListHandler.Handle)
            .WithTags(Tags.Bunches)
            .Produces<IEnumerable<BunchModel>>()
            .RequireAuthorization()
            .WithSummary("List bunches");

        app.MapGet(ApiRoutes.Bunch.ListForCurrentUser, GetBunchListForCurrentUserHandler.Handle)
            .WithTags(Tags.Bunches)
            .Produces<IEnumerable<BunchModel>>()
            .RequireAuthorization()
            .WithSummary("List your bunches");

        app.MapPost(ApiRoutes.Bunch.Add, AddBunchHandler.Handle)
            .WithTags(Tags.Bunches)
            .Produces<BunchModel>()
            .RequireAuthorization()
            .WithSummary("Add bunch");
    }

    private static void MapJoinRequestEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.JoinRequest.ListByBunch, ListJoinRequestHandler.Handle)
            .WithTags(Tags.JoinRequests)
            .Produces<IEnumerable<JoinRequestModel>>()
            .RequireAuthorization()
            .WithSummary("List join requests for bunch");
        
        app.MapPost(ApiRoutes.JoinRequest.Add, AddJoinRequestHandler.Handle)
            .WithTags(Tags.JoinRequests)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Send request to join bunch");
        
        app.MapPost(ApiRoutes.JoinRequest.Accept, AcceptJoinRequestHandler.Handle)
            .WithTags(Tags.JoinRequests)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Accept or deny a join request");
        
        app.MapPost(ApiRoutes.JoinRequest.Deny, DenyJoinRequestHandler.Handle)
            .WithTags(Tags.JoinRequests)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Accept or deny a join request");
    }
    
    private static void MapCashgameEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.Cashgame.Get, GetCashgameHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<CashgameDetailsModel>()
            .RequireAuthorization()
            .WithSummary("Get cashgame");

        app.MapGet(ApiRoutes.Cashgame.ListByBunch, GetCashgameListByBunchHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<IEnumerable<CashgameListItemModel>>()
            .RequireAuthorization()
            .WithSummary("List cashgames");

        app.MapGet(ApiRoutes.Cashgame.ListByBunchAndYear, GetCashgameListByBunchAndYearHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<IEnumerable<CashgameListItemModel>>()
            .RequireAuthorization()
            .WithSummary("List cashgames by year");

        app.MapGet(ApiRoutes.Cashgame.ListByEvent, GetCashgameListByEventHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<IEnumerable<CashgameListItemModel>>()
            .RequireAuthorization()
            .WithSummary("List cashgames by event");

        app.MapGet(ApiRoutes.Cashgame.ListByPlayer, GetCashgameListByPlayerHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<IEnumerable<CashgameListItemModel>>()
            .RequireAuthorization()
            .WithSummary("List cashgames by player");

        app.MapPost(ApiRoutes.Cashgame.Add, AddCashgameHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<CashgameDetailsModel>()
            .RequireAuthorization()
            .WithSummary("Add cashgame");

        app.MapPut(ApiRoutes.Cashgame.Update, UpdateCashgameHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<CashgameDetailsModel>()
            .RequireAuthorization()
            .WithSummary("Update cashgame");

        app.MapDelete(ApiRoutes.Cashgame.Delete, DeleteCashgameHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Delete cashgame");

        app.MapGet(ApiRoutes.Cashgame.ListCurrentByBunch, GetCurrentCashgamesHandler.Handle)
            .WithTags(Tags.Cashgames)
            .Produces<ApiCurrentGame>()
            .RequireAuthorization()
            .WithSummary("List running cashgames");
    }

    private static void MapCashgameActionEndpoints(WebApplication app)
    {
        app.MapPost(ApiRoutes.Action.Add, AddActionHandler.Handle)
            .WithTags(Tags.CashgameActions)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Add cashgame action")
            .WithDescription("Type can be 'buyin', 'report' or 'cashout'. The Added field is only used for buyin actions");

        app.MapPut(ApiRoutes.Action.Update, UpdateActionHandler.Handle)
            .WithTags(Tags.CashgameActions)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Update player action")
            .WithDescription("The Added field is only used for buyin actions");

        app.MapDelete(ApiRoutes.Action.Delete, DeleteActionHandler.Handle)
            .WithTags(Tags.CashgameActions)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Delete cashgame action");
    }

    private static void MapPlayerEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.Player.Get, GetPlayerHandler.Handle)
            .WithTags(Tags.Players)
            .Produces<PlayerModel>()
            .RequireAuthorization()
            .WithSummary("Get player");

        app.MapGet(ApiRoutes.Player.ListByBunch, GetPlayerListHandler.Handle)
            .WithTags(Tags.Players)
            .Produces<IEnumerable<PlayerListItemModel>>()
            .RequireAuthorization()
            .WithSummary("List bunch players");

        app.MapPost(ApiRoutes.Player.Add, AddPlayerHandler.Handle)
            .WithTags(Tags.Players)
            .Produces<PlayerModel>()
            .RequireAuthorization()
            .WithSummary("Add player to bunch");

        app.MapDelete(ApiRoutes.Player.Delete, DeletePlayerHandler.Handle)
            .WithTags(Tags.Players)
            .Produces<MessageModel>()
            .RequireAuthorization()
            .WithSummary("Delete player");
    }

    private static void MapLocationEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.Location.Get, GetLocationHandler.Handle)
            .WithTags(Tags.Locations)
            .Produces<LocationModel>()
            .RequireAuthorization()
            .WithSummary("Get location");

        app.MapGet(ApiRoutes.Location.ListByBunch, GetLocationListHandler.Handle)
            .WithTags(Tags.Locations)
            .Produces<IEnumerable<LocationModel>>()
            .RequireAuthorization()
            .WithSummary("List locations");

        app.MapPost(ApiRoutes.Location.Add, AddLocationHandler.Handle)
            .WithTags(Tags.Locations)
            .Produces<LocationModel>()
            .RequireAuthorization()
            .WithSummary("Add location");
    }

    private static void MapEventEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.Event.Get, GetEventHandler.Handle)
            .WithTags(Tags.Events)
            .Produces<EventModel>()
            .RequireAuthorization()
            .WithSummary("Get event");

        app.MapGet(ApiRoutes.Event.ListByBunch, GetEventListHandler.Handle)
            .WithTags(Tags.Events)
            .Produces<IEnumerable<EventModel>>()
            .RequireAuthorization()
            .WithSummary("List events");

        app.MapPost(ApiRoutes.Event.Add, AddEventHandler.Handle)
            .WithTags(Tags.Events)
            .Produces<EventModel>()
            .RequireAuthorization()
            .WithSummary("Add event");
    }

    private static void MapGeneralEndpoints(WebApplication app)
    {
        app.MapGet(ApiRoutes.Version, VersionHandler.Handle)
            .ExcludeFromDescription();
        
        app.Map(ApiRoutes.Error, ErrorHandler.Handle)
            .ExcludeFromDescription();

        app.MapGet(ApiRoutes.Root, RootHandler.Handle)
            .ExcludeFromDescription();
    }

    private static void MapAdminEndpoints(WebApplication app)
    {
        app.MapPost(ApiRoutes.Admin.ClearCache, ClearCacheHandler.Handle)
            .RequireAuthorization()
            .ExcludeFromDescription();

        app.MapPost(ApiRoutes.Admin.SendEmail, SendEmailHandler.Handle)
            .RequireAuthorization()
            .ExcludeFromDescription();
    }
}
