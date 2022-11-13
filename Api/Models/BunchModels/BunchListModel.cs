using System.Collections.Generic;
using System.Linq;
using Core.UseCases;

namespace Api.Models.BunchModels;

public class BunchListModel : List<BunchModel>
{
    public BunchListModel(GetBunchList.Result bunchListResult)
    {
        AddRange(bunchListResult.Bunches.Select(o => new BunchModel(o)));
    }
}