using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases;

public class AddBunch
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

    public BunchResult Execute(Request request)
    {
        var validator = new Validator(request);
        if(!validator.IsValid)
            throw new ValidationException(validator);

        var slug = SlugGenerator.GetSlug(request.DisplayName);

        bool bunchExists;
        try
        {
            var b = _bunchRepository.GetBySlug(slug);
            bunchExists = true;
        }
        catch (BunchNotFoundException)
        {
            bunchExists = false;
        }

        if (bunchExists)
            throw new BunchExistsException(slug);

        var bunch = CreateBunch(request);
        var id = _bunchRepository.Add(bunch);
        var user = _userRepository.Get(request.UserName);
        var player = Player.New(id, user.Id, user.UserName, Role.Manager);
        var playerId = _playerRepository.Add(player);
        var createdPlayer = _playerRepository.Get(playerId);

        return new BunchResult(bunch, createdPlayer);
    }

    private static Bunch CreateBunch(Request request)
    {
        return new Bunch(
            0,
            SlugGenerator.GetSlug(request.DisplayName),
            request.DisplayName,
            request.Description,
            string.Empty,
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
}