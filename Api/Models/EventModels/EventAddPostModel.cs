using JetBrains.Annotations;

namespace Api.Models.EventModels;

public class EventAddPostModel
{
    public string Name { get; [UsedImplicitly] set; }
}