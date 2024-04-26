using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LethalAchievements.Json.Serialization;

/// <summary>
///     An inclusive range of values. Can be open-ended.
///     The JSON representation can either be an object with 'min' and/or 'max' properties, or a single value.
/// </summary>
public abstract class Range<T> where T : struct, IComparable<T>
{
    /// <summary>
    ///     Creates a new range.
    /// </summary>
    /// <param name="min">The inclusive minimum value, or null if open-ended.</param>
    /// <param name="max">The inclusive maximum value, or null if open-ended.</param>
    /// <exception cref="ArgumentException">
    ///     Both <paramref name="min" /> and <paramref name="max" /> are null,
    ///     or <paramref name="min" /> is greater than <paramref name="max" />.
    /// </exception>
    protected Range(T? min, T? max)
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
    protected Range(T value)
    {
        Min = value;
        Max = value;
    }

    /// <summary>
    ///     The inclusive minimum value, or null if open-ended.
    /// </summary>
    public T? Min { get; }

    /// <summary>
    ///     The inclusive maximum value, or null if open-ended.
    /// </summary>
    public T? Max { get; }

    /// <summary>
    ///     Checks if the range contains the specified value, inclusive.
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
        /// <summary>
        ///     Converts a JSON token to a value of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="token"> The token to convert. </param>
        /// <returns> The converted value. </returns>
        protected abstract T FromToken(JToken token);

        /// <summary>
        ///     Creates a new typed range with the specified minimum and maximum values.
        /// </summary>
        /// <param name="min"> The minimum value </param>
        /// <param name="max"> The maximum value </param>
        /// <returns></returns>
        protected abstract TRange Create(T? min, T? max);

        /// <summary>
        ///     Reads a JSON object or value and converts it to a range.
        /// </summary>
        /// <returns> The deserialized range. </returns>
        public override TRange ReadJson(JsonReader reader, Type objectType, TRange? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
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

        /// <summary>
        ///     Writes a range to JSON.
        /// </summary>
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
    public IntRange(int? min, int? max) : base(min, max)
    {
    }

    /// <inheritdoc />
    public IntRange(int value) : base(value)
    {
    }

    private class Converter : Converter<IntRange>
    {
        protected override int FromToken(JToken token)
        {
            return token.Value<int>();
        }

        protected override IntRange Create(int? min, int? max)
        {
            return new IntRange(min, max);
        }
    }
}

/// <inheritdoc />
[JsonConverter(typeof(Converter))]
public class FloatRange : Range<float>
{
    /// <inheritdoc />
    public FloatRange(float? min, float? max) : base(min, max)
    {
    }

    /// <inheritdoc />
    public FloatRange(float value) : base(value)
    {
    }

    private class Converter : Converter<FloatRange>
    {
        protected override float FromToken(JToken token)
        {
            return token.Value<float>();
        }

        protected override FloatRange Create(float? min, float? max)
        {
            return new FloatRange(min, max);
        }
    }
}