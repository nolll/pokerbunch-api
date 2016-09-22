using System.Collections.Generic;
using Core.Services;

namespace Core.UseCases
{
    public class EditBunchForm
    {
        private readonly BunchService _bunchService;
        private readonly UserService _userService;
        private readonly PlayerService _playerService;

        public EditBunchForm(BunchService bunchService, UserService userService, PlayerService playerService)
        {
            _bunchService = bunchService;
            _userService = userService;
            _playerService = playerService;
        }

        public Result Execute(Request request)
        {
            var bunch = _bunchService.GetBySlug(request.Slug);
            var user = _userService.GetByNameOrEmail(request.UserName);
            var player = _playerService.GetByUserId(bunch.Id, user.Id);
            RequireRole.Manager(user, player);
            var heading = string.Format("{0} Settings", bunch.DisplayName);
            var description = bunch.Description;
            var houseRules = bunch.HouseRules;
            var defaultBuyin = bunch.DefaultBuyin;
            var timeZoneId = bunch.Timezone.Id;
            var currencySymbol = bunch.Currency.Symbol;
            var currencyLayout = bunch.Currency.Layout;
            var timeZones = TimeZoneService.GetTimeZones();
            var currencyLayouts = Globalization.GetCurrencyLayouts();
            
            return new Result(heading, bunch.Slug, description, houseRules, defaultBuyin, timeZoneId, currencySymbol, currencyLayout, timeZones, currencyLayouts);
        }

        public class Request
        {
            public string UserName { get; }
            public string Slug { get; }

            public Request(string userName, string slug)
            {
                UserName = userName;
                Slug = slug;
            }
        }

        public class Result
        {
            public string Heading { get; private set; }
            public string Slug { get; private set; }
            public string Description { get; private set; }
            public string HouseRules { get; private set; }
            public int DefaultBuyin { get; private set; }
            public string TimeZoneId { get; private set; }
            public string CurrencySymbol { get; private set; }
            public string CurrencyLayout { get; private set; }
            public IList<AddBunchForm.TimeZoneItem> TimeZones { get; private set; }
            public IList<string> CurrencyLayouts { get; private set; }

            public Result(string heading, string slug, string description, string houseRules, int defaultBuyin, string timeZoneId, string currencySymbol, string currencyLayout, IList<AddBunchForm.TimeZoneItem> timeZones, IList<string> currencyLayouts)
            {
                Heading = heading;
                Slug = slug;
                Description = description;
                HouseRules = houseRules;
                DefaultBuyin = defaultBuyin;
                TimeZoneId = timeZoneId;
                CurrencySymbol = currencySymbol;
                CurrencyLayout = currencyLayout;
                TimeZones = timeZones;
                CurrencyLayouts = currencyLayouts;
            }
        }
    }
}