using System;
using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LethalAchievements.Config;

public static class Json
{
    internal static readonly JsonSerializerSettings Settings = new() {
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    /// <summary>
    ///     Deserializes a JSON string into an object of type <typeparamref name="T"/>.
    ///     This is just a wrapper around <see cref="JsonConvert.DeserializeObject{T}(string, JsonSerializerSettings)"/>
    ///     with specific settings.
    /// </summary>
    public static T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, Settings);
    
    internal static Dictionary<string, Type> CreateTypeDict<T>()
    {
        var baseType = typeof(T);
        var baseTypeName = baseType.Name;
        if (baseType.IsInterface)
        {
            baseTypeName = baseTypeName.Substring(1); // remove starting "I"
        }
        
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsAbstract)
            .ToDictionary(GetTypeName, type => type);

        string GetTypeName(Type type)
        {
            var name = type.Name.Replace(baseTypeName, "");
            name = StringHelper.PascalToSnakeCase(name);
            return name;
        }
    }
}