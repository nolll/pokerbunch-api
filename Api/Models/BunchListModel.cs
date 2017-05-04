using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models
{
    [CollectionDataContract(Namespace = "", Name = "bunches", ItemName = "bunch")]
    public class BunchListModel : List<BunchModel>
    {
        public BunchListModel(GetBunchList.Result bunchListResult)
        {
            AddRange(bunchListResult.Bunches.Select(o => new BunchModel(o)));
        }

        public BunchListModel()
        {
        }
    }

    [CollectionDataContract(Namespace = "", Name = "users", ItemName = "user")]
    public class UserListModel : List<UserItemModel>
    {
        public UserListModel(UserList.Result userList)
        {
            AddRange(userList.Users.Select(o => new UserItemModel(o)));
        }

        public UserListModel()
        {
        }
    }
}