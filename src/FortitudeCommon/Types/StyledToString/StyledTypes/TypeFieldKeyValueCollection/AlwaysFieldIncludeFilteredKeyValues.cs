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
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
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
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
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
        bool foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
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
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        bool foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            int count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
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
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var  count    = 0;
        var hasValue = value?.MoveNext() ?? false;
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
                ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(kvp.Key).Append(": ");
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                : stb.AppendOrNull(kvp.Value);
            stb.GoToNextCollectionItemStart();
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct where TKey : TKBase =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct where TKey : TKBase 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
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
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct where TKey : TKBase 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
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
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct where TKey : TKBase 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct where TKey : TKBase 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var count    = 0;
        var hasValue = value?.MoveNext() ?? false;
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
                ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(kvp.Key).Append(": ");
            stb.AppendOrNull(kvp.Value, valueStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler);

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
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
                stb.AppendOrNull(kvp.Key, keyStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(kvp.Key, keyStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKey : struct where TValue : struct
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var count    = 0;
        var hasValue = value?.MoveNext() ?? false;
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
            stb.AppendOrNull(kvp.Key, keyStyler).Append(": ");
            stb.AppendOrNull(kvp.Value, valueStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = value.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
        }
        else
        {
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        }
        return stb.Sb.AddGoToNext(stb);
    }
}
