using System.Runtime.Serialization;
using Api.Models.CommonModels;

namespace Api.Models.AdminModels
{
    [DataContract(Namespace = "", Name = "cachecleared")]
    public class ClearCacheModel : MessageModel
    {
        private readonly int _objectCount;

        public ClearCacheModel(int objectCount)
        {
            _objectCount = objectCount;
        }

        public override string Message
        {
            get
            {
                if (_objectCount == 0)
                    return "The cache contained no objects";
                if (_objectCount == 1)
                    return "1 object was removed from the cache";
                return $"{_objectCount} objects was removed from the cache";
            }
        }
    }
}