using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class EditBunch : UseCase<EditBunch.Request, EditBunch.Result>
{
    private readonly IBunchRepository _bunchRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPlayerRepository _playerRepository;

    public EditBunch(IBunchRepository bunchRepository, IUserRepository userRepository, IPlayerRepository playerRepository)
    {
        _bunchRepository = bunchRepository;
        _userRepository = userRepository;
        _playerRepository = playerRepository;
    }

    protected override UseCaseResult<Result> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var bunch = _bunchRepository.GetBySlug(request.Slug);
        var currentUser = _userRepository.Get(request.UserName);
        var currentPlayer = _playerRepository.Get(bunch.Id, currentUser.Id);

        if (!AccessControl.CanEditBunch(currentUser, currentPlayer))
            return Error(new AccessDeniedError());

        var postedHomegame = CreateBunch(bunch, request);
        _bunchRepository.Update(postedHomegame);

        return Success(new Result(bunch, currentPlayer));
    }
    
    private static Bunch CreateBunch(Bunch bunch, Request request)
    {
        return new Bunch(
            bunch.Id,
            bunch.Slug,
            bunch.DisplayName,
            request.Description,
            request.HouseRules,
            TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone),
            request.DefaultBuyin,
            new Currency(request.CurrencySymbol, request.CurrencyLayout));
    }

    public class Request
    {
        public string UserName { get; }
        public string Slug { get; }
        public string Description { get; }
        [Required(ErrorMessage = "Currency Symbol can't be empty")]
        public string CurrencySymbol { get; }
        [Required(ErrorMessage = "Currency Layout can't be empty")]
        public string CurrencyLayout { get; }
        [Required(ErrorMessage = "Timezone can't be empty")]
        public string TimeZone { get; }
        public string HouseRules { get; }
        public int DefaultBuyin { get; }

        public Request(string userName, string slug, string description, string currencySymbol, string currencyLayout, string timeZone, string houseRules, int defaultBuyin)
        {
            UserName = userName;
            Slug = slug;
            Description = description;
            CurrencySymbol = currencySymbol;
            CurrencyLayout = currencyLayout;
            TimeZone = timeZone;
            HouseRules = houseRules;
            DefaultBuyin = defaultBuyin;
        }
    }

    public class Result : BunchResult
    {
        public Result(Bunch b, Player p) : base(b, p)
        {
        }
    }
}