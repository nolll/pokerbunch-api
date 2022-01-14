using JetBrains.Annotations;

namespace Api.Models.CashgameModels;

public class AddCashgameActionPostModel
{
    public string Type { get; [UsedImplicitly] set; }
    public int PlayerId { get; [UsedImplicitly] set; }
    public int Added { get; [UsedImplicitly] set; }
    public int Stack { get; [UsedImplicitly] set; }
}