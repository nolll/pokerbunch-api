using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Api.Services;

public static class JsonHelper
{
    public static string Serialize(object data)
    {
        return JsonConvert.SerializeObject(data, Settings);
    }

    private static JsonSerializerSettings Settings => new JsonSerializerSettings
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    };
}