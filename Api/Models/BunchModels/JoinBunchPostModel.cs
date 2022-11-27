using System.Text.Json.Serialization;

namespace Api.Models.BunchModels;

public class JoinBunchPostModel
{
    public string Code { get; }

    [JsonConstructor]
    public JoinBunchPostModel(string code)
    {
        Code = code;
    }
}