// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public partial class KeyValueCollectionBuilder
{
    public KeyValueCollectionBuilder AddAll<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue> (Dictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue> (IDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (value != null)
        {
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue> (ICollection<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder AddAll<TKey, TValue> (IEnumerable<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        if (value != null)
        {
            foreach (var kvp in value)
            {
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        AddAll((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        if (value != null)
        {
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddAll<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.AppendOrNull(kvp.Value, valueStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

}
