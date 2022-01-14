using System.Runtime.Serialization;

namespace Api.Models.CommonModels;

[DataContract(Namespace = "", Name = "ok")]
public class OkModel : MessageModel
{
    public override string Message => "Ok";
}