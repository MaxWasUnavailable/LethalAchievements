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
    ///     Reads an internally tagged JSON object.
    /// </summary>
    /// <param name="tagPropertyName">
    ///     The property name to read the type from.
    ///     This method returns null if this property cannot be found.
    /// </param>
    /// <param name="jsonObject">The JSON object to read from.</param>
    /// <param name="serializer">The serializer to use.</param>
    /// <typeparam name="TBase">The base type or interface to deserialize to.</typeparam>
    /// <exception cref="JsonException">
    ///     The type specified in the JSON object could not be found.
    /// </exception>
    public static TBase? ReadJson<TBase>(string tagPropertyName, JObject jsonObject, JsonSerializer serializer) where TBase : class
    {
        var triggerName = jsonObject[tagPropertyName]?.Value<string>();
        if (triggerName is null)
            return null;
            
        if (!TryGetType<TBase>(triggerName, out var triggerType))
            throw new JsonException($"Unknown {tagPropertyName} type: '{triggerName}'");
            
        jsonObject.Remove(tagPropertyName);

        return (TBase) jsonObject.ToObject(triggerType, serializer)!;
    }
    
    /// <summary>
    ///     Tries to find a type that inherits/implements <typeparamref name="TBase"/> by its snake_case name.
    ///     See <see cref="GetTypeDict{TBase}"/> for full naming rules.
    /// </summary>
    /// <returns>
    ///     <c>true</c> if the type was found, <c>false</c> otherwise.
    /// </returns>
    public static bool TryGetType<TBase>(string snakeCaseName, out Type type)
    {
        return GetTypeDict<TBase>().TryGetValue(snakeCaseName, out type);
    }
    
    /// <summary>
    ///     Retrieves a dictionary of types that inherit/implement <typeparamref name="TBase"/>.
    ///     The keys are the snake_case names of the types, excluding the base type name.
    ///     For example, if <typeparamref name="TBase"/> is <c>ITrigger</c>, then <c>SellScrapTrigger</c> would be
    ///     stored as <c>sell_scrap</c>.
    /// </summary>
    public static IReadOnlyDictionary<string, Type> GetTypeDict<TBase>()
    {
        var baseType = typeof(TBase);
        
        if (_typeCache.TryGetValue(baseType, out var dict))
        {
            return dict;
        }
        
        var baseTypeName = baseType.Name;
        if (baseType.IsInterface)
        {
            baseTypeName = baseTypeName.Substring(1); // remove leading "I"
        }
        
        dict = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(TBase).IsAssignableFrom(type) && !type.IsAbstract)
            .ToDictionary(GetTypeName, type => type);
        
        _typeCache[baseType] = dict;
        
        return dict;

        string GetTypeName(Type type)
        {
            var name = type.Name.Replace(baseTypeName, "");
            name = StringHelper.PascalToSnakeCase(name);
            return name;
        }
    }
}