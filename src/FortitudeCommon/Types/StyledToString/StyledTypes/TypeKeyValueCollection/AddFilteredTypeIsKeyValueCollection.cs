// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public interface IAddFilteredTypeIsKeyValueCollection : IRecyclableObject
{
    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue> value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue> (IReadOnlyList<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? keyValuePairs
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;
}

public class AddFilteredTypeIsKeyValueCollection: RecyclableObject, IAddFilteredTypeIsKeyValueCollection
{
    private IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> stb = null!;

    public AddFilteredTypeIsKeyValueCollection Initialize(IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> styledComplexTypeBuilder)
    {
        stb  = styledComplexTypeBuilder;

        return this;
    }

    
    public KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
        , KeyValuePredicate<TKey, TValue> filterPredicate 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue> (Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue> (IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
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

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
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
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (value != null)
        {
            int count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
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
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var count = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                if (!filterPredicate(count++, kvp.Key, kvp.Value))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
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
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
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
          , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
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
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue> (IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        if (value != null)
        {
            int count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate,  StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var count = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                if (!filterPredicate(count++, kvp.Key, kvp.Value))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
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
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        From((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        if (value != null)
        {
            int count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , StructStyler<TValue> valueStructStyler
          , StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct
    {
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var count = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                if (!filterPredicate(count++, kvp.Key, kvp.Value))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
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
