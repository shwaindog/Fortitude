// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public interface IAddAllTypeIsKeyValueCollection : IRecyclableObject
{
    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? keyValuePairs, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? keyValuePairs, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? keyValuePairs, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? keyValuePairs, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? keyValuePairs, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? keyValuePairs
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue> (IReadOnlyList<KeyValuePair<TKey, TValue>>? keyValuePairs
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? keyValuePairs
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? keyValuePairs
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? keyValuePairs
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;
}

public class AddAllTypeIsKeyValueCollection : RecyclableObject, IAddAllTypeIsKeyValueCollection
{
    private IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> stb = null!;

    public AddAllTypeIsKeyValueCollection Initialize(IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> styledComplexTypeBuilder)
    {
        stb  = styledComplexTypeBuilder;

        return this;
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue> (Dictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue> (IDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
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

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>
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

    public KeyValueCollectionBuilder From<TKey, TValue> (ICollection<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue> (IEnumerable<KeyValuePair<TKey, TValue>>? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>
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

    public KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? value
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
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

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
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

    public override void StateReset()
    {
        stb  = null!;

        base.StateReset();
    }
}
