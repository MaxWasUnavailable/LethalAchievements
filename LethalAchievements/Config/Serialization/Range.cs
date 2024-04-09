using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LethalAchievements.Config.Serialization;

/// <summary>
///     An exclusive range of values. Can be open-ended.
///     The JSON representation can either be an object with 'min' and/or 'max' properties, or a single value.
/// </summary>
public abstract class Range<T> where T : struct, IComparable<T>
{
    /// <summary>
    ///     The exclusive minimum value, or null if open-ended.
    /// </summary>
    public T? Min { get; }
    
    /// <summary>
    ///     The exclusive maximum value, or null if open-ended.
    /// </summary>
    public T? Max { get; }
    
    /// <summary>
    ///     Creates a new range.
    /// </summary>
    /// <param name="min">The exclusive minimum value, or null if open-ended.</param>
    /// <param name="max">The exclusive maximum value, or null if open-ended.</param>
    /// <exception cref="ArgumentException">
    ///     Both <paramref name="min"/> and <paramref name="max"/> are null,
    ///     or <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public Range(T? min, T? max)
    {
        var minIsNull = min is null;
        var maxIsNull = max is null;
        
        if (minIsNull && maxIsNull)
            throw new ArgumentException("At least one of 'min' or 'max' must be specified.");
        
        if (!minIsNull && !maxIsNull && min!.Value.CompareTo(max!.Value) > 0)
            throw new ArgumentException("Min must be less than or equal to max.");
        
        Min = min;
        Max = max;
    }

    /// <summary>
    ///     Creates a new range with a single value.
    /// </summary>
    public Range(T value)
    {
        Min = value;
        Max = value;
    }
    
    /// <summary>
    ///     Checks if the range contains the specified value, exclusive.
    /// </summary>
    public bool Contains(T value)
    {
        if (Min is not null && value.CompareTo(Min.Value) < 0)
            return false;
        
        if (Max is not null && value.CompareTo(Max.Value) > 0)
            return false;

        return true;
    }

    /// <inheritdoc />
    protected abstract class Converter<TRange> : JsonConverter<TRange> where TRange : Range<T>
    {
        protected abstract T FromToken(JToken token);
        protected abstract TRange Create(T? min, T? max);
        
        public override TRange ReadJson(JsonReader reader, Type objectType, TRange? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var jsonObject = JObject.Load(reader);
                var minToken = jsonObject["min"];
                T? min = minToken is null ? null : FromToken(minToken);
                
                var maxToken = jsonObject["max"];
                T? max = maxToken is null ? null : FromToken(maxToken);
                
                return Create(min, max);
            }

            var value = FromToken(JToken.Load(reader));
            return Create(value, value);
        }
        
        public override void WriteJson(JsonWriter writer, TRange? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

/// <inheritdoc />
[JsonConverter(typeof(Converter))]
public class IntRange : Range<int>
{
    /// <inheritdoc />
    public IntRange(int? min, int? max) : base(min, max) { }
    
    /// <inheritdoc />
    public IntRange(int value) : base(value) { }
    
    private class Converter : Converter<IntRange>
    {
        protected override int FromToken(JToken token) => token.Value<int>();
        protected override IntRange Create(int? min, int? max) => new(min, max);
    }
}

/// <inheritdoc />
[JsonConverter(typeof(Converter))]
public class FloatRange : Range<float>
{
    /// <inheritdoc />
    public FloatRange(float? min, float? max) : base(min, max) { }
    
    /// <inheritdoc />
    public FloatRange(float value) : base(value) { }
    
    private class Converter : Converter<FloatRange>
    {
        protected override float FromToken(JToken token) => token.Value<float>();
        protected override FloatRange Create(float? min, float? max) => new(min, max);
    }
}
