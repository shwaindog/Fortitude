// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

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
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
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
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
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
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            int count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
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
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                if (!filterPredicate(count++, kvp.Key, kvp.Value))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate 
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AddFilteredEnumerate(value, filterPredicate, valueStyler, keyFormatString);

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2> (IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            int count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate,  CustomTypeStyler<TVBase2> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                if (!filterPredicate(count++, kvp.Key, kvp.Value))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate 
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AddFilteredEnumerate(value, filterPredicate, valueStyler, keyStyler);

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate 
          , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            int count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                if (!filterPredicate(count++, kvp.Key, kvp.Value))
                {
                    hasValue = value.MoveNext();
                    continue;
                }
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }
}
