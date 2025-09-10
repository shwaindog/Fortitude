using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                    : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendNullOrValue(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        bool foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                    : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendNullOrValue(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        bool foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            int count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                    : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendNullOrValue(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType,  itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var count     = 0;
        var hasValue  = value?.MoveNext() ?? false;
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        var itemCount = 0;
        while(hasValue) 
        {
            var kvp = value!.Current;
            if (!filterPredicate(count++, kvp.Key, kvp.Value))
            {
                hasValue = value.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                : stb.AppendNullOrValue(kvp.Value);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2  =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                    : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                    : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                    : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var count     = 0;
        var hasValue  = value?.MoveNext() ?? false;
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        var itemCount = 0;
        while(hasValue) 
        {
            var kvp = value!.Current;
            if (!filterPredicate(count++, kvp.Key, kvp.Value))
            {
                hasValue = value.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).FieldEnd(stb)
                : stb.AppendNullOrValue(kvp.Key).FieldEnd(stb);
            stb.AppendOrNull(kvp.Value, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler).FieldEnd(stb);
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler).FieldEnd(stb);
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var count     = 0;
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler).FieldEnd(stb);
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var count     = 0;
        var hasValue  = value?.MoveNext() ?? false;
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        var itemCount = 0;
        while(hasValue) 
        {
            var kvp = value!.Current;
            if (!filterPredicate(count++, kvp.Key, kvp.Value))
            {
                hasValue = value.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.StartDictionary();
                foundValues = true;
            }
            stb.AppendOrNull(kvp.Key, keyStyler).FieldEnd(stb);
            stb.AppendOrNull(kvp.Value, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }
}
