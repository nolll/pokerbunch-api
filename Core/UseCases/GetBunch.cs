using System;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class GetBunch(IBunchRepository bunchRepository)
    : UseCase<GetBunch.Request, GetBunch.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var bunch = await bunchRepository.GetBySlug(request.Slug);
        
        return !request.Auth.CanGetBunch(bunch.Id) ? 
            Error(new AccessDeniedError()) : 
            Success(new Result(bunch));
    }

    public class Request(IAuth auth, string slug)
    {
        public IAuth Auth { get; } = auth;
        public string Slug { get; } = slug;
    }

    public class Result(Bunch b) : BunchResult(b);
}

public class BunchResult(Bunch b)
{
    public string Slug { get; } = b.Slug;
    public string Name { get; } = b.DisplayName;
    public string Description { get; } = b.Description;
    public string HouseRules { get; } = b.HouseRules;
    public TimeZoneInfo Timezone { get; } = b.Timezone;
    public Currency Currency { get; } = b.Currency;
    public int DefaultBuyin { get; } = b.DefaultBuyin;
}
