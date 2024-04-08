using System;
using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LethalAchievements.Config;

[JsonConverter(typeof(Converter))]
public class Criterion
{
    public ITrigger Trigger { get; }
    public ICondition[]? Conditions { get; }

    private Action<Criterion, Context>? _callback;

    public Criterion(ITrigger trigger, params ICondition[]? conditions)
    {
        Trigger = trigger;
        Conditions = conditions;
    }
    
    /// <summary>
    ///     Adds a callback to be invoked when the trigger is triggered.
    ///     Note: this does not check the conditions!
    /// </summary>
    public void Subscribe(Action<Criterion, Context> callback)
    {
        if (_callback is null)
        {
            // do this only the first time Subscribe is called
            Trigger.OnTriggered += OnTriggered;
        }
        _callback += callback;
    }
    
    /// <summary>
    ///    Removes a callback from being invoked when the trigger is triggered.
    /// </summary>
    public void Unsubscribe(Action<Criterion, Context> callback)
    {
        _callback -= callback;
        if (_callback is null)
        {
            // do this only the last time callback is removed
            Trigger.OnTriggered -= OnTriggered;
        }
    }

    private void OnTriggered(Context context)
    {
        _callback?.Invoke(this, context);
    }

    private class Converter : JsonConverter<Criterion>
    {
        private static readonly Dictionary<string, Type> _triggerTypes;
        private static readonly Dictionary<string, Type> _conditionTypes;
        
        static Converter()
        {
            _triggerTypes = Json.CreateTypeDict<ITrigger>();
            _conditionTypes = Json.CreateTypeDict<ICondition>();
        }
        
        public override bool CanWrite => false;
        
        public override Criterion ReadJson(JsonReader reader, Type objectType, Criterion? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            
            var conditions = ReadConditions(obj, serializer); // do this first to get rid of the "conditions" property
            var trigger = ReadTrigger(obj, serializer);
            
            return new Criterion(trigger, conditions);
        }

        private static ITrigger ReadTrigger(JObject obj, JsonSerializer serializer)
        {
            var triggerName = obj["trigger"]?.Value<string>();
            if (triggerName is null)
                throw new JsonException("Criterion must have a trigger");
            
            if (!_triggerTypes.TryGetValue(triggerName, out var triggerType))
                throw new JsonException($"Unknown trigger type: '{triggerName}'");
            
            obj.Remove("trigger");

            return (ITrigger) obj.ToObject(triggerType, serializer)!;
        }

        private static ICondition[]? ReadConditions(JObject obj, JsonSerializer serializer)
        {
            var token = obj["conditions"];
            if (token is null)
                return null; // conditions are optional

            if (token.Type != JTokenType.Object)
                throw new JsonException("Conditions must be a JSON object");
            
            var conditions = ((JObject) token).Properties()
                .Select(property => ReadCondition(property, serializer))
                .ToArray();
            
            obj.Remove("conditions");
            
            return conditions;
        }

        private static ICondition ReadCondition(JProperty property, JsonSerializer serializer)
        {
            var conditionName = property.Name;
            if (!_conditionTypes.TryGetValue(conditionName, out var conditionType))
                throw new JsonException($"Unknown condition type: '{conditionName}'");

            return (ICondition) property.Value.ToObject(conditionType, serializer)!;
        }
        
        public override void WriteJson(JsonWriter writer, Criterion? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}