using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EditBunch(
    IBunchRepository bunchRepository,
    IUserRepository userRepository,
    IPlayerRepository playerRepository)
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
        var currentUser = await userRepository.GetByUserName(request.UserName);
        var currentPlayer = await playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanEditBunch(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var postedBunch = CreateBunch(bunch, request, timezone);
        await bunchRepository.Update(postedBunch);

        return Success(new Result(postedBunch, currentPlayer!));
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

    private static Bunch CreateBunch(Bunch bunch, Request request, TimeZoneInfo timezone)
    {
        return new Bunch(
            bunch.Id,
            bunch.Slug,
            bunch.DisplayName,
            request.Description,
            request.HouseRules,
            timezone,
            request.DefaultBuyin,
            new Currency(request.CurrencySymbol, request.CurrencyLayout));
    }

    public class Request(
        string userName,
        string slug,
        string description,
        string currencySymbol,
        string currencyLayout,
        string timeZone,
        string houseRules,
        int defaultBuyin)
    {
        public string UserName { get; } = userName;
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

    public class Result(Bunch b, Player p) : BunchResult(b, p);
}