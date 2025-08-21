using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
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

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
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

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
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

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
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

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKDerived>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(key).Append(": ");
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                : stb.AppendOrNull(keyValue);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
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

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TValue> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct where TKDerived : TKey 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
          , CustomTypeStyler<TValue> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct where TKDerived : TKey  
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct where TKDerived : TKey  
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct where TKDerived : TKey  
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue, TKDerived>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, CustomTypeStyler<TValue> valueStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct where TKDerived : TKey  
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(key).Append(": ");
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
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

    public TExt AlwaysWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeys<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt AlwaysWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct 
    {
        var foundValues = false;
        stb.FieldNameJoin(fieldName);
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (!foundValues)
            {
                stb.StartDictionary();
                foundValues = true;
            }
            stb.AppendOrNull(key, keyStyler).Append(": ");
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
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