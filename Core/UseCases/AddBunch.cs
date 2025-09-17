using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddBunch(
    IBunchRepository bunchRepository,
    IPlayerRepository playerRepository)
    : UseCase<AddBunch.Request, AddBunch.Result>
{
    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var slug = SlugGenerator.GetSlug(request.DisplayName);
        var existingBunch = await bunchRepository.GetBySlugOrNull(slug);
        var bunchExists = existingBunch != null;

        if (bunchExists)
            return Error(new BunchExistsError(slug));

        var bunch = CreateBunch(request);
        var id = await bunchRepository.Add(bunch);
        var player = Player.New(id, request.Auth.Id, request.Auth.UserName, Role.Manager);
        await playerRepository.Add(player);

        return Success(new Result(bunch));
    }
    
    private static Bunch CreateBunch(Request request)
    {
        return new Bunch(
            "",
            SlugGenerator.GetSlug(request.DisplayName),
            request.DisplayName,
            request.Description,
            "",
            TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone),
            0,
            new Currency(request.CurrencySymbol, request.CurrencyLayout));
    }

    public class Request
    {
        public IAuth Auth { get; }
        [Required(ErrorMessage = "Display Name can't be empty")]
        public string DisplayName { get; }
        public string Description { get; }
        [Required(ErrorMessage = "Currency Symbol can't be empty")]
        public string CurrencySymbol { get; }
        [Required(ErrorMessage = "Currency Layout can't be empty")]
        public string CurrencyLayout { get; }
        [Required(ErrorMessage = "Timezone can't be empty")]
        public string TimeZone { get; }

        public Request(IAuth auth, string displayName, string description, string currencySymbol, string currencyLayout, string timeZone)
        {
            Auth = auth;
            DisplayName = displayName;
            Description = description;
            CurrencySymbol = currencySymbol;
            CurrencyLayout = currencyLayout;
            TimeZone = timeZone;
        }
    }

    public class Result(Bunch b) : BunchResult(b);
}