using System;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Serialization;

public class OneOrMultipleConverter<T> : JsonConverter<T[]> 
{
    /// <inheritdoc />
    public override T[]? ReadJson(JsonReader reader, Type objectType, T[]? existingValue, bool hasExistingValue, JsonSerializer serializer) 
    {
        return reader.TokenType == JsonToken.StartArray ? 
            serializer.Deserialize<T[]>(reader) : 
            new[] { serializer.Deserialize<T>(reader)! };
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, T[]? value, JsonSerializer serializer) 
    {
        throw new NotImplementedException();
    }
}