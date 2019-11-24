using System.Runtime.Serialization;
using Api.Models.CommonModels;

namespace Api.Models.AdminModels
{
    [DataContract(Namespace = "", Name = "cachecleared")]
    public class ClearCacheModel : MessageModel
    {
        public override string Message => "The cache was cleared";
    }
}