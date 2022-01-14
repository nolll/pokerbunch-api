using System.Runtime.Serialization;
using Core.UseCases;

namespace Api.Models.LocationModels;

[DataContract(Namespace = "", Name = "timezone")]
public class TimezoneModel
{
    [DataMember(Name = "id")]
    public string Id { get; }
    [DataMember(Name = "name")]
    public string Name { get; }

    public TimezoneModel(GetTimezoneList.Timezone timezone)
        : this(timezone.Id, timezone.Name)
    {
    }

    private TimezoneModel(string id, string name)
    {
        Id = id;
        Name = name;
    }
}