using Core.Entities;

namespace Core.Services;

public class CashgameService
{
    public static bool SpansMultipleYears(IEnumerable<Cashgame> cashgames)
    {
        var years = new List<int>();
        foreach (var cashgame in cashgames)
        {
            if (cashgame.StartTime.HasValue)
            {
                var year = cashgame.StartTime.Value.Year;
                if (!years.Contains(year))
                {
                    years.Add(year);
                }
            }
        }
        return years.Count > 1;
    }
}