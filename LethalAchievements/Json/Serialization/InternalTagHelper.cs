using System;
using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LethalAchievements.Config.Serialization;

/// <summary>
///     Provides helper methods for reading internally tagged JSON objects.
///     This is used for deserializing to an interface or base type, where the
///     actual type is determined by a property in the JSON object. For example:
///     <code>
///         {
///             "trigger": "sell_scrap",
///             "amount": 100
///         }
///     </code>
///     Where the "trigger" property tells the deserializer to deserialize the object
///     as a <c>SellScrapTrigger</c>.
/// </summary>
public static class InternalTagHelper
{
    private static readonly Dictionary<Type, Dictionary<string, Type>> _typeCache = new();

    /// <summary>
    ///     Typed version of <see cref="ReadJson"/>.
    /// </summary>
    public static T? ReadJson<T>(string tagPropertyName, JObject jsonObject, JsonSerializer serializer)
    {
        return (T?) ReadJson(tagPropertyName, typeof(T), jsonObject, serializer);
    }
    
    /// <summary>
    ///     Reads an internally tagged JSON object.
    /// </summary>
    /// <param name="tagPropertyName">
    ///     The property name to read the type from.
    ///     This method returns null if this property cannot be found.
    /// </param>
    /// <param name="jsonObject">The JSON object to read from.</param>
    /// <param name="serializer">The serializer to use.</param>
    /// <param name="baseType">The base type or interface to deserialize to.</param>
    /// <returns>An object that inherits/implements <paramref name="baseType"/>, or null.</returns>
    /// <exception cref="JsonException">
    ///     The type specified in the JSON object could not be found.
    /// </exception>
    public static object? ReadJson(string tagPropertyName, Type baseType, JObject jsonObject, JsonSerializer serializer)
    {
        var triggerName = jsonObject[tagPropertyName]?.Value<string>();
        if (triggerName is null)
            return null;
            
        if (!TryGetType(triggerName, baseType, out var triggerType))
            throw new JsonException($"Unknown {tagPropertyName} type: '{triggerName}'");
            
        jsonObject.Remove(tagPropertyName);

        return jsonObject.ToObject(triggerType, serializer)!;
    }
 
    /// <summary>
    ///     Typed version of <see cref="TryGetType{TBase}"/>.
    /// </summary>
    public static bool TryGetType<TBase>(string snakeCaseName, out Type type)
    {
        return TryGetType(snakeCaseName, typeof(TBase), out type);
    }
    
    /// <summary>
    ///     Tries to find a type that inherits/implements <paramref name="baseType"/> by its snake_case name.
    ///     See <see cref="GetTypeDict"/> for full naming rules.
    /// </summary>
    /// <returns>
    ///     <c>true</c> if the type was found, <c>false</c> otherwise.
    /// </returns>
    public static bool TryGetType(string snakeCaseName, Type baseType, out Type type)
    {
        return GetTypeDict(baseType).TryGetValue(snakeCaseName, out type);
    }
    
    /// <summary>
    ///     Retrieves a dictionary of types that inherit/implement <paramref name="baseType"/>.
    ///     The keys are the snake_case names of the types, excluding the base type name.
    ///     For example, if <paramref name="baseType"/> is <c>ITrigger</c>, then <c>SellScrapTrigger</c> would be
    ///     stored as <c>sell_scrap</c>.
    /// </summary>
    public static IReadOnlyDictionary<string, Type> GetTypeDict(Type baseType)
    {
        if (_typeCache.TryGetValue(baseType, out var dict))
        {
            return dict;
        }
        
        dict = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
            .ToDictionary(type => GetTypeName(type, baseType), type => type);
        
        _typeCache[baseType] = dict;
        
        return dict;
    }
    
    public static string GetBaseTypeName(Type baseType)
    {
        var baseTypeName = baseType.Name;
        if (baseType.IsInterface) {
            baseTypeName = baseTypeName.Substring(1); // remove leading "I"
        }
        
        return baseTypeName;
    }
    
    public static string GetTypeName(Type type, Type baseType)
    {
        var baseTypeName = GetBaseTypeName(baseType);
        return StringHelper.PascalToSnakeCase(type.Name.Replace(baseTypeName, ""));
    }
}

public class InternalTagConverter : JsonConverter
{
    /// <inheritdoc />
    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var baseTypeName = InternalTagHelper.GetBaseTypeName(objectType);
        var tagPropertyName = StringHelper.PascalToSnakeCase(baseTypeName);
        
        var value = InternalTagHelper.ReadJson(tagPropertyName, objectType, jsonObject, serializer);
        if (value is null)
            throw new JsonException($"{tagPropertyName} type not specified");
            
        return value;
    }

    /// <inheritdoc />
    public override bool CanConvert(Type objectType)
    {
        return objectType.IsInterface || objectType.IsAbstract;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}