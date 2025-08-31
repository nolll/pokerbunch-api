using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EditBunch(
    IBunchRepository bunchRepository)
    : UseCase<EditBunch.Request, EditBunch.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var timezone = GetTimezone(request.TimeZone);
        if (timezone is null)
            return Error(new ValidationError($"Invalid timezone: {request.TimeZone}"));

        var bunch = await bunchRepository.GetBySlug(request.Slug);
        
        if (!request.Principal.CanEditBunch(bunch.Id))
            return Error(new AccessDeniedError());

        var postedBunch = CreateBunch(bunch, request, timezone);
        await bunchRepository.Update(postedBunch);

        return Success(new Result(postedBunch));
    }

    private static TimeZoneInfo? GetTimezone(string id)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }
        catch
        {
            return null;
        }
    }

    private static Bunch CreateBunch(Bunch bunch, Request request, TimeZoneInfo timezone) => new(
        bunch.Id,
        bunch.Slug,
        bunch.DisplayName,
        request.Description,
        request.HouseRules,
        timezone,
        request.DefaultBuyin,
        new Currency(request.CurrencySymbol, request.CurrencyLayout));

    public class Request(
        IPrincipal principal,
        string slug,
        string description,
        string currencySymbol,
        string currencyLayout,
        string timeZone,
        string houseRules,
        int defaultBuyin)
    {
        public IPrincipal Principal { get; } = principal;
        public string Slug { get; } = slug;
        public string Description { get; } = description;

        [Required(ErrorMessage = "Currency Symbol can't be empty")]
        public string CurrencySymbol { get; } = currencySymbol;

        [Required(ErrorMessage = "Currency Layout can't be empty")]
        public string CurrencyLayout { get; } = currencyLayout;

        [Required(ErrorMessage = "Timezone can't be empty")]
        public string TimeZone { get; } = timeZone;

        public string HouseRules { get; } = houseRules;
        public int DefaultBuyin { get; } = defaultBuyin;
    }

    public class Result(Bunch b) : BunchResult(b);
}