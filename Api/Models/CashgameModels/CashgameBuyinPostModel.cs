using JetBrains.Annotations;

namespace Api.Models.CashgameModels
{
    public class CashgameBuyinPostModel
    {
        public int PlayerId { get; [UsedImplicitly] set; }
        public int Amount { get; [UsedImplicitly] set; }
        public int Stack { get; [UsedImplicitly] set; }
    }
}