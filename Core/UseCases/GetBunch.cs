using System;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetBunch(
    IBunchRepository bunchRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository)
    : UseCase<GetBunch.Request, GetBunch.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        var user = await userRepository.GetByUserName(request.UserName);
        var player = await playerRepository.Get(bunch.Id, user.Id);
        
        return !AccessControl.CanGetBunch(user, player) ? 
            Error(new AccessDeniedError()) : 
            Success(new Result(bunch, player!));
    }

    public class Request(string userName, string slug)
    {
        public string UserName { get; } = userName;
        public string Slug { get; } = slug;
    }

    public class Result(Bunch b, Player p) : BunchResult(b, p);
}

public class BunchResult(Bunch b, Player? p)
{
    public string Slug { get; } = b.Slug;
    public string Name { get; } = b.DisplayName;
    public string Description { get; } = b.Description;
    public string HouseRules { get; } = b.HouseRules;
    public TimeZoneInfo Timezone { get; } = b.Timezone;
    public Currency Currency { get; } = b.Currency;
    public int DefaultBuyin { get; } = b.DefaultBuyin;
    public BunchPlayerResult? Player { get; } = p != null ? new BunchPlayerResult(p.Id, p.DisplayName) : null;
    public Role Role { get; } = p?.Role ?? Role.Admin;
}

public class BunchPlayerResult(string id, string name)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
}