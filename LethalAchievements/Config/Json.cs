using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LethalAchievements.Config;

/// <summary>
///     JSON utilities.
/// </summary>
public static class Json
{
    private static readonly JsonSerializerSettings _settings = new() {
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };
    
    /// <summary>
    ///     Deserializes a JSON string into an object of type <typeparamref name="T"/>.
    ///     This is just a wrapper around <see cref="JsonConvert.DeserializeObject{T}(string, JsonSerializerSettings)"/>
    ///     with specific settings.
    /// </summary>
    public static T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);
}