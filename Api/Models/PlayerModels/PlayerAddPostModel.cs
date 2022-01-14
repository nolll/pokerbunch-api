using JetBrains.Annotations;

namespace Api.Models.PlayerModels;

public class PlayerAddPostModel
{
    public string Name { get; [UsedImplicitly] set; }
}