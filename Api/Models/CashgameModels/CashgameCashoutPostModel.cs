using JetBrains.Annotations;

namespace Api.Models.CashgameModels
{
    public class CashgameCashoutPostModel
    {
        public int PlayerId { get; [UsedImplicitly] set; }
        public int Stack { get; [UsedImplicitly] set; }
    }
}