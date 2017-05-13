using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.UserModels
{
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