using JetBrains.Annotations;

namespace Api.Models.CashgameModels
{
    public class AddCashgamePostModel
    {
        public int LocationId { get; [UsedImplicitly] set; }
        public int EventId { get; [UsedImplicitly] set; }
    }
}