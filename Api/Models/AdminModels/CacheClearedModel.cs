using Api.Models.CommonModels;

namespace Api.Models.AdminModels;

public class CacheClearedModel : MessageModel
{
    public override string Message => "The cache was cleared";
}