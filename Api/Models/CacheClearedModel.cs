using System.Runtime.Serialization;

namespace Api.Models
{
    [DataContract(Namespace = "", Name = "cachecleared")]
    public class CacheClearedModel
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public CacheClearedModel(int objectCount)
        {
            Message = GetMessage(objectCount);
        }

        public CacheClearedModel()
        {
        }

        private string GetMessage(int objectCount)
        {
            if (objectCount == 0)
                return "The cache contained no objects";
            if (objectCount == 1)
                return "1 object was removed from the cache";
            return $"{objectCount} objects was removed from the cache";
        }
    }
}