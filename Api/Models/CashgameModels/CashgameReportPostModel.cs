using JetBrains.Annotations;

namespace Api.Models.CashgameModels
{
    public class CashgameReportPostModel
    {
        public int PlayerId { get; [UsedImplicitly] set; }
        public int Stack { get; [UsedImplicitly] set; }
    }
}