using System;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Serialization;

/// <summary>
///     Converts transparent types, i.e. instead of:
///     <code>
///     {
///       "a": {
///         "b": 42,
///         "c": "hello"
///       }
///     }
///     </code>
///     where "a" is our transparent type, we can have:
///     <code>
///     {
///       "b": 42,
///       "c": "hello"
///     }
///     </code>
///     This is useful for wrappers when we want to have a more concise JSON representation.
/// </summary>
/// <typeparam name="T">The transparent type. Must have a constructor which takes only TInner as an argument.</typeparam>
/// <typeparam name="TInner">The inner type, which is actually getting deserialized from the JSON.</typeparam>
internal class TransparentConverter<T, TInner> : JsonConverter<T>
{
    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var inner = serializer.Deserialize<TInner>(reader);
        var result = Activator.CreateInstance(typeof(T), inner);
        return (T?) result;
    }
    
    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}