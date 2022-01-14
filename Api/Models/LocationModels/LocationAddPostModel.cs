using JetBrains.Annotations;

namespace Api.Models.LocationModels;

public class LocationAddPostModel
{
    public string Name { get; [UsedImplicitly] set; }
}