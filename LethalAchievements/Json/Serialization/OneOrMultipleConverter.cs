using System;
using Newtonsoft.Json;

namespace LethalAchievements.Json.Serialization;

/// <summary>
///     Converts either a JSON array or a single value into a C# array.
///     Use this on fields and properties of type <typeparamref name="T" />[].
/// </summary>
/// <typeparam name="T">The element type of the array.</typeparam>
public class OneOrMultipleConverter<T> : JsonConverter<T[]>
{
    /// <inheritdoc />
    public override T[]? ReadJson(JsonReader reader, Type objectType, T[]? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return reader.TokenType == JsonToken.StartArray
            ? serializer.Deserialize<T[]>(reader)
            : [serializer.Deserialize<T>(reader)!];
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, T[]? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}