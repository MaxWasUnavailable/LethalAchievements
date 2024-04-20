using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LethalAchievements.Json;

/// <summary>
///     Json utilities.
/// </summary>
public static class Json
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    /// <summary>
    ///     Deserializes a JSON string into an object of type <typeparamref name="T" />.
    ///     This is just a wrapper around <see cref="JsonConvert.DeserializeObject{T}(string, JsonSerializerSettings)" />
    ///     with specific settings.
    /// </summary>
    public static T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, Settings);
    }
}