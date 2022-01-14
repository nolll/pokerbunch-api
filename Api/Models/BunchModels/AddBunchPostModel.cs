using JetBrains.Annotations;

namespace Api.Models.BunchModels;

public class AddBunchPostModel
{
    public string Name { get; [UsedImplicitly] set; }
    public string Description { get; [UsedImplicitly] set; }
    public string Timezone { get; [UsedImplicitly] set; }
    public string CurrencySymbol { get; [UsedImplicitly] set; }
    public string CurrencyLayout { get; [UsedImplicitly] set; }
}