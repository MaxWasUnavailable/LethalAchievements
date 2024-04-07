using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace LethalAchievements.Config;

public static class Json
{
    private static readonly JsonSerializerSettings _settings = new() {
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new CamelCaseNamingStrategy()
        },
        Converters = {
            new TaggedInterfaceConverter<ITrigger>(),
            new TaggedInterfaceConverter<ICondition>()
        }
    };

    /// <summary>
    ///     Deserializes a JSON string into an object of type <typeparamref name="T"/>.
    /// </summary>
    public static T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);
}

internal class TaggedInterfaceConverter<T> : JsonConverter where T : class
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(T).IsAssignableFrom(objectType);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        Debug.LogWarning("Reading JSON");
        
        var jsonObject = JObject.Load(reader);
        var typeName = jsonObject.GetValue("type")?.Value<string>();
        if (typeName == null)
            throw new JsonSerializationException("Type property not found.");

        var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == typeName);
        if (type == null || !typeof(T).IsAssignableFrom(type))
            throw new JsonSerializationException($"Type '{typeName}' not found or does not implement {typeof(T).Name}.");

        return jsonObject.ToObject(type, serializer);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new InvalidOperationException("This converter is only intended for deserialization.");
    }
}