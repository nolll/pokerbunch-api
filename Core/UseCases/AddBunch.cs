using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Errors;
using Core.Repositories;
using Core.Services;

namespace Core.UseCases;

public class AddBunch : UseCase<AddBunch.Request, AddBunch.Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IBunchRepository _bunchRepository;
    private readonly IPlayerRepository _playerRepository;

    public AddBunch(IUserRepository userRepository, IBunchRepository bunchRepository, IPlayerRepository playerRepository)
    {
        _userRepository = userRepository;
        _bunchRepository = bunchRepository;
        _playerRepository = playerRepository;
    }

    protected override async Task<UseCaseResult<Result>> Work(Request request)
    {
        var validator = new Validator(request);
        if (!validator.IsValid)
            return Error(new ValidationError(validator));

        var slug = SlugGenerator.GetSlug(request.DisplayName);
        var existingBunch = await _bunchRepository.GetBySlugOrNull(slug);
        var bunchExists = existingBunch != null;

        if (bunchExists)
            return Error(new BunchExistsError(slug));

        var bunch = CreateBunch(request);
        var id = await _bunchRepository.Add(bunch);
        var user = await _userRepository.GetByUserName(request.UserName);
        var player = Player.New(id, user.Id, user.UserName, Role.Manager);
        var playerId = await _playerRepository.Add(player);
        var createdPlayer = await _playerRepository.Get(playerId);

        return Success(new Result(bunch, createdPlayer));
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
        public string UserName { get; }
        [Required(ErrorMessage = "Display Name can't be empty")]
        public string DisplayName { get; }
        public string Description { get; }
        [Required(ErrorMessage = "Currency Symbol can't be empty")]
        public string CurrencySymbol { get; }
        [Required(ErrorMessage = "Currency Layout can't be empty")]
        public string CurrencyLayout { get; }
        [Required(ErrorMessage = "Timezone can't be empty")]
        public string TimeZone { get; }

        public Request(string userName, string displayName, string description, string currencySymbol, string currencyLayout, string timeZone)
        {
            UserName = userName;
            DisplayName = displayName;
            Description = description;
            CurrencySymbol = currencySymbol;
            CurrencyLayout = currencyLayout;
            TimeZone = timeZone;
        }
    }

    public class Result : BunchResult
    {
        public Result(Bunch b, Player p) : base(b, p)
        {
        }
    }
}