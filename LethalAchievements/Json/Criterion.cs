using System;
using System.Linq;
using LethalAchievements.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LethalAchievements.Json;

/// <summary>
///     A criterion.
/// </summary>
[JsonConverter(typeof(Converter))]
public class Criterion
{
    private Action<Criterion, Context>? _callback;

    /// <summary>
    ///     Creates a new criterion.
    /// </summary>
    /// <param name="trigger"> The trigger that will trigger this criterion. </param>
    /// <param name="requiredCount">
    ///     The number of times the trigger must be triggered for this criterion to be considered
    ///     complete.
    /// </param>
    /// <param name="conditions"> The conditions that must be met for this criterion to be considered complete. </param>
    public Criterion(ITrigger trigger, int requiredCount, params ICondition[]? conditions)
    {
        Trigger = trigger;
        RequiredCount = requiredCount;
        Conditions = conditions;
    }

    /// <summary>
    ///     The trigger that will trigger this criterion.
    /// </summary>
    public ITrigger Trigger { get; }

    /// <summary>
    ///     The conditions that must be met for this criterion to be considered complete.
    /// </summary>
    public ICondition[]? Conditions { get; }

    /// <summary>
    ///     The number of times the trigger must be triggered for this criterion to be considered complete.
    /// </summary>
    public int RequiredCount { get; }

    /// <summary>
    ///     Adds a callback to be invoked when the trigger is triggered.
    ///     Note: this does not check the conditions!
    /// </summary>
    public void Subscribe(Action<Criterion, Context> callback)
    {
        if (_callback is null)
            // do this only the first time Subscribe is called
            Trigger.OnTriggered += OnTriggered;
        _callback += callback;
    }

    /// <summary>
    ///     Removes a callback from being invoked when the trigger is triggered.
    /// </summary>
    public void Unsubscribe(Action<Criterion, Context> callback)
    {
        _callback -= callback;
        if (_callback is null)
            // do this only the last time callback is removed
            Trigger.OnTriggered -= OnTriggered;
    }

    private void OnTriggered(Context context)
    {
        _callback?.Invoke(this, context);
    }

    private class Converter : JsonConverter<Criterion>
    {
        public override bool CanWrite => false;

        public override Criterion ReadJson(JsonReader reader, Type objectType, Criterion? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);

            var conditions = ReadConditions(obj, serializer); // do this first to get rid of the "conditions" property
            var count = ReadCount(obj);
            var trigger = ReadTrigger(obj, serializer);

            return new Criterion(trigger, count, conditions);
        }

        private static ICondition[]? ReadConditions(JObject obj, JsonSerializer serializer)
        {
            var token = obj["conditions"];
            if (token is null)
                return null; // conditions are optional

            if (token.Type != JTokenType.Object)
                throw new JsonException("Conditions must be a JSON object");

            var conditions = ((JObject)token).Properties()
                .Select(property => ReadCondition(property, serializer))
                .ToArray();

            obj.Remove("conditions");

            return conditions;
        }

        private static ICondition ReadCondition(JProperty property, JsonSerializer serializer)
        {
            var conditionName = property.Name;
            if (!InternalTagHelper.TryGetType<ICondition>(conditionName, out var conditionType))
                throw new JsonException($"Unknown condition type: '{conditionName}'");

            return (ICondition)property.Value.ToObject(conditionType, serializer)!;
        }

        private static int ReadCount(JObject obj)
        {
            var property = obj.Property("required_count");
            if (property is null) return 1;

            if (property.Value.Type != JTokenType.Integer)
                throw new JsonException("Criterion 'required_count' must be an integer");

            obj.Remove("required_count");

            return property.Value.Value<int>();
        }

        private static ITrigger ReadTrigger(JObject obj, JsonSerializer serializer)
        {
            var trigger = InternalTagHelper.ReadJson<ITrigger>("trigger", obj, serializer);
            if (trigger == null)
                throw new JsonException("Condition is missing required property 'trigger'");

            return trigger;
        }

        public override void WriteJson(JsonWriter writer, Criterion? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}