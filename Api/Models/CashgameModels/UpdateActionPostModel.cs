using System;
using JetBrains.Annotations;

namespace Api.Models.CashgameModels
{
    public class UpdateActionPostModel
    {
        public DateTimeOffset Timestamp { get; [UsedImplicitly] set; }
        public int Stack { get; [UsedImplicitly] set; }
        public int Added { get; [UsedImplicitly] set; }
    }
}