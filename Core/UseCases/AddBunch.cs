using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Core.Exceptions;
using Core.Services;
using ValidationException = Core.Exceptions.ValidationException;

namespace Core.UseCases
{
    public class AddBunch
    {
        private readonly UserService _userService;
        private readonly BunchService _bunchService;
        private readonly PlayerService _playerService;

        public AddBunch(UserService userService, BunchService bunchService, PlayerService playerService)
        {
            _userService = userService;
            _bunchService = bunchService;
            _playerService = playerService;
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
                var b = _bunchService.GetBySlug(slug);
                bunchExists = true;
            }
            catch (BunchNotFoundException)
            {
                bunchExists = false;
            }

            if (bunchExists)
                throw new BunchExistsException(slug);

            var bunch = CreateBunch(request);
            var id = _bunchService.Add(bunch);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = Player.New(id, user.Id, Role.Manager);
            _playerService.Add(player);

            return new BunchResult(bunch, Role.Manager);
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
                200,
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
}
