using System.Runtime.Serialization;

namespace Api.Models.CommonModels;

public abstract class MessageModel
{
    [DataMember(Name = "message")]
    public abstract string Message { get; }
}