using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt>  where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            var count     = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase>(
        string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
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
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase>(
        string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        var count       = 0;
        var hasValue    = value?.MoveNext() ?? false;
        var kvpType     = typeof(KeyValuePair<TKey, TValue>);
        var itemCount   = 0;
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
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                : stb.AppendMatchOrNull(kvp.Value);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
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
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType,  itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase1, TVBase2>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
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
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        var count       = 0;
        var hasValue    = value?.MoveNext() ?? false;
        var kvpType     = typeof(KeyValuePair<TKey, TValue>);
        var itemCount   = 0;
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
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
            stb.AppendOrNull(kvp.Value, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TVBase1, TVBase2>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
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
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2> (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
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
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        var count       = 0;
        var hasValue    = value?.MoveNext() ?? false;
        var kvpType     = typeof(KeyValuePair<TKey, TValue>);
        var itemCount   = 0;
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
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
            stb.AppendOrNull(kvp.Value, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

}