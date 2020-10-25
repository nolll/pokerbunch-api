using Api.Models.LocationModels;
using Api.Routes;
using Api.Settings;
using Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TimezoneController : BaseController
    {
        private readonly GetTimezoneList _getTimezoneList;

        public TimezoneController(
            AppSettings appSettings,
            GetTimezoneList getTimezoneList)
            : base(appSettings)
        {
            _getTimezoneList = getTimezoneList;
        }

        [Route(ApiRoutes.Misc.Timezones)]
        [HttpGet]
        public TimezoneListModel GetList()
        {
            var timezoneListResult = _getTimezoneList.Execute();
            return new TimezoneListModel(timezoneListResult);
        }
    }
}