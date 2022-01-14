using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.CashgameModels;

[DataContract(Namespace = "", Name = "event")]
public class CashgameDetailsEventModel
{
    [DataMember(Name = "id")]
    public string Id { get; }
    [DataMember(Name = "name")]
    public string Name { get; }

    public CashgameDetailsEventModel(CashgameDetails.Result details)
    {
        Id = details.EventId.ToString();
        Name = details.EventName;
    }
}