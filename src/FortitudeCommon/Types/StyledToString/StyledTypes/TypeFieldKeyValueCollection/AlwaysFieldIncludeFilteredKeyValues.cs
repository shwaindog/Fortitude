using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct=>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStructStyler, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
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
                stb.AppendOrNull(kvp.Value, valueStructStyler);
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
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
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
                stb.AppendOrNull(kvp.Value, valueStructStyler);
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
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
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
                stb.AppendOrNull(kvp.Value, valueStructStyler);
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
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct
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
            stb.AppendOrNull(kvp.Value, valueStructStyler);
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
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler);

    public TExt AlwaysAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
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
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
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
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
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
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
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
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
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
                stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
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
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
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
            stb.AppendOrNull(kvp.Key, keyStructStyler).Append(": ");
            stb.AppendOrNull(kvp.Value, valueStructStyler);
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
