using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.BunchModels;

[CollectionDataContract(Namespace = "", Name = "bunches", ItemName = "bunch")]
public class BunchListModel : List<BunchModel>
{
    public BunchListModel(GetBunchList.Result bunchListResult)
    {
        AddRange(bunchListResult.Bunches.Select(o => new BunchModel(o)));
    }
}