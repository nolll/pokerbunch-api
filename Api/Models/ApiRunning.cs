using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "running")]
    public class ApiRunning
    {
        [DataMember(Name = "location")]
        public string Location { get; set; }
        [DataMember(Name = "results")]
        public IList<ApiRunningResult> Results { get; set; }

        public ApiRunning(RunningCashgame.Result runningCashgame)
        {
            Location = runningCashgame.LocationName;
            Results = runningCashgame.PlayerItems.Select(o => new ApiRunningResult(o)).ToList();
        }
    }
}