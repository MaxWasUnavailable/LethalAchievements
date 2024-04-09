using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LethalAchievements.Config.Serialization;

/// <summary>
///     An internally tagged condition. See <see cref="InternalTagHelper"/> for details on what that means.
/// </summary>
[JsonConverter(typeof(Converter))]
public class TaggedCondition
{
    /// <summary>
    ///     The inner condition.
    /// </summary>
    public ICondition Value { get; }

    /// <summary>
    ///     Creates a new <see cref="TaggedCondition"/> with an inner value.
    /// </summary>
    public TaggedCondition(ICondition value)
    {
        Value = value;
    }

    private class Converter : JsonConverter<TaggedCondition>
    {
        public override TaggedCondition ReadJson(JsonReader reader, Type objectType, TaggedCondition? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var inner = InternalTagHelper.ReadJson<ICondition>("condition", jsonObject, serializer);
            if (inner is null)
                throw new JsonException("Condition type not specified");
            
            return new TaggedCondition(inner);
        }
        
        public override void WriteJson(JsonWriter writer, TaggedCondition? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}