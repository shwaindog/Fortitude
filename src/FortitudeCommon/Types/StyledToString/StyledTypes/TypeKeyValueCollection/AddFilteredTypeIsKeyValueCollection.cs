// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public partial class KeyValueCollectionBuilder
{
    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase =>
        AddFilteredEnumerate(value, filterPredicate, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase 
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

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase 
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

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase 
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

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate 
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase 
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

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate 
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : struct =>
        AddFilteredEnumerate(value, filterPredicate, valueStyler, keyFormatString);

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : struct
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
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , KeyValuePredicate<TKBase, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : struct
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
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase> (IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate 
          , CustomTypeStyler<TValue> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : struct
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
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate,  CustomTypeStyler<TValue> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : struct
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
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct =>
        AddFilteredEnumerate((IEnumerable<KeyValuePair<TKey, TValue>>?)value, filterPredicate, valueStyler, keyStyler);

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct
    {
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct
        where TValue : struct
    {
        if (value != null)
        {
            int count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate 
          , CustomTypeStyler<TValue> valueStyler
          , CustomTypeStyler<TKey> keyStyler)
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
                stb.AppendOrNull(kvp.Value, valueStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }
}
