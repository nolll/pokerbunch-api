using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services;

namespace Core.UseCases
{
    public class CashgameContext
    {
        private readonly UserService _userService;
        private readonly BunchService _bunchService;
        private readonly CashgameService _cashgameService;

        public CashgameContext(UserService userService, BunchService bunchService, CashgameService cashgameService)
        {
            _userService = userService;
            _bunchService = bunchService;
            _cashgameService = cashgameService;
        }

        public Result Execute(Request request)
        {
            var bunchContextResult = new BunchContext(_userService, _bunchService).Execute(request);
            var runningGame = _cashgameService.GetRunning(bunchContextResult.BunchId);

            var gameIsRunning = runningGame != null;
            var years = _cashgameService.GetYears(bunchContextResult.BunchId);

            var selectedYear = request.Year;
            if (request.SelectedPage == CashgamePage.Overview)
            {
                if(years.Count > 0)
                    selectedYear = years.Max(o => o);
                else
                    selectedYear = request.CurrentTime.Year; // todo: convert to local bunch time
            }

            return new Result(
                bunchContextResult,
                request.Slug,
                gameIsRunning,
                request.SelectedPage,
                years,
                selectedYear);
        }

        public class Request : BunchContext.BunchRequest
        {
            public DateTime CurrentTime { get; }
            public CashgamePage SelectedPage { get; }
            public int? Year { get; }

            public Request(string userName, string slug, DateTime currentTime, CashgamePage selectedPage = CashgamePage.Unknown, int? year = null)
                : base(userName, slug)
            {
                CurrentTime = currentTime;
                SelectedPage = selectedPage;
                Year = year;
            }
        }

        public class Result
        {
            public string Slug { get; private set; }
            public bool GameIsRunning { get; private set; }
            public CashgamePage SelectedPage { get; private set; }
            public int? SelectedYear { get; private set; }
            public IList<int> Years { get; private set; }
            public BunchContext.Result BunchContext { get; private set; }

            public Result(
                BunchContext.Result bunchContextResult,
                string slug,
                bool gameIsRunning,
                CashgamePage selectedPage,
                IList<int> years,
                int? selectedYear)
            {
                Slug = slug;
                BunchContext = bunchContextResult;
                GameIsRunning = gameIsRunning;
                SelectedPage = selectedPage;
                SelectedYear = selectedYear;
                Years = years;
            }
        }

        public enum CashgamePage
        {
            Unknown,
            Overview,
            Matrix,
            Toplist,
            Chart,
            List,
            Facts
        }
    }
}