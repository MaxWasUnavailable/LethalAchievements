using System;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Serialization;

/// <summary>
///     Converts transparent types.
///     This is useful for wrappers to make the JSON structure simpler.
///     Currently only supports types that have a single field.
/// </summary>
public class TransparentConverter : JsonConverter
{
    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var fields = objectType.GetFields();
        if (fields.Length != 1)
            throw new InvalidOperationException("TransparentConverter only supports types with exactly one field.");
        
        var inner = serializer.Deserialize(reader, fields[0].FieldType);
        var result = Activator.CreateInstance(objectType);
        fields[0].SetValue(result, inner);
        
        return result;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}