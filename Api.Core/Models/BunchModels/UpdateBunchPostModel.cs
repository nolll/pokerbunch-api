using JetBrains.Annotations;

namespace Api.Models.BunchModels
{
    public class UpdateBunchPostModel
    {
        public string Description { get; [UsedImplicitly] set; }
        public string HouseRules { get; [UsedImplicitly] set; }
        public string Timezone { get; [UsedImplicitly] set; }
        public string CurrencySymbol { get; [UsedImplicitly] set; }
        public string CurrencyLayout { get; [UsedImplicitly] set; }
        public int DefaultBuyin { get; [UsedImplicitly] set; }
    }
}