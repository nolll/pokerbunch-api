using JetBrains.Annotations;

namespace Api.Models.CashgameModels;

public class UpdateCashgamePostModel
{
    public int LocationId { get; [UsedImplicitly] set; }
    public int EventId { get; [UsedImplicitly] set; }
}