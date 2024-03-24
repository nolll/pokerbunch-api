using System.Text.Json.Serialization;

namespace Api.Models.BunchModels;

[method: JsonConstructor]
public class JoinBunchPostModel(string code)
{
    public string Code { get; } = code;
}