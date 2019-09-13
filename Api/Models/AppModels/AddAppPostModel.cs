using JetBrains.Annotations;

namespace Api.Models.AppModels
{
    public class AddAppPostModel
    {
        public string Name { get; [UsedImplicitly] set; }
    }
}