using System;
using LethalAchievements.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LethalAchievements.Config;

/// <summary>
///     Json utilities.
/// </summary>
public static class Json
{
    private static readonly JsonSerializerSettings _settings = new() {
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        },
        Converters = {
            new SnakeCaseEnumConverter()
        }
    };
    
    /// <summary>
    ///     Deserializes a JSON string into an object of type <typeparamref name="T"/>.
    ///     This is just a wrapper around <see cref="JsonConvert.DeserializeObject{T}(string, JsonSerializerSettings)"/>
    ///     with specific settings.
    /// </summary>
    public static T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);
    
    private class SnakeCaseEnumConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing enum.");

            var enumText = reader.Value!.ToString();
            var pascalCase = StringHelper.SnakeToPascalCase(enumText);
            
            return Enum.Parse(objectType, pascalCase);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
        
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}