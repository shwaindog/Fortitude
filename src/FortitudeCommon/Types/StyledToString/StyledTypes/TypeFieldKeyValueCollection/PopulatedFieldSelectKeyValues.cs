using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
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

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
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

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
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

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
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

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        var hasValue    = selectKeys.MoveNext();
        var kvpType     = typeof(KeyValuePair<TKey, TValue>);
        var itemCount   = 0;
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
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : stb.AppendMatchOrNull(key, true).FieldEnd();
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                : stb.AppendMatchOrNull(keyValue);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TVBase>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
          , CustomTypeStyler<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)  
        where TKDerived : TKey where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)   
        where TKDerived : TKey where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, itemCount);
            }
        }
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)    
        where TKDerived : TKey where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        var hasValue    = selectKeys.MoveNext();
        var kvpType     = typeof(KeyValuePair<TKey, TValue>);
        var itemCount   = 0;
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
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : stb.AppendMatchOrNull(key, true).FieldEnd();
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, itemCount);
            hasValue = selectKeys.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
        (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
        (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
          , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (!foundValues)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartDictionary();
                    foundValues = true;
                }
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
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

    public TExt WhenPopulatedWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKDerived : TKey where TKey : TKBase where TValue : TVBase 
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        var foundValues = false;
        var hasValue    = selectKeys.MoveNext();
        var kvpType     = typeof(KeyValuePair<TKey, TValue>);
        var itemCount   = 0;
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
                stb.FieldNameJoin(fieldName);
                stb.StartDictionary();
                foundValues = true;
            }
            stb.AppendOrNull(key, keyStyler, true).FieldEnd();
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        if (foundValues)
        {
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
}
