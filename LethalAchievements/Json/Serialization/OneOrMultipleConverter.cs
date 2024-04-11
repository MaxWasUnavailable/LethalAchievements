using System;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Serialization;

/// <summary>
///     Converts either a JSON array or a single value into an array.
///     Use this on array fields and properties with the [JsonConverter] attribute.
/// </summary>
public class OneOrMultipleConverter : JsonConverter
{
    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
            return serializer.Deserialize(reader, objectType);

        // we can't do new[] { ... } because we need to return an array over
        // objectType specifically, not just object[]
        var elementType = objectType.GetElementType()!;
        var array = Array.CreateInstance(elementType, 1);
        array.SetValue(serializer.Deserialize(reader, elementType), 0);
        return array;
    }

    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsArray;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}