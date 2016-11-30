using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "apps", ItemName = "app")]
    public class AppListModel : List<AppModel>
    {
        public AppListModel(AppList.Result appListResult)
        {
            AddRange(appListResult.Items.Select(o => new AppModel(o)));
        }

        public AppListModel()
        {
        }
    }
}