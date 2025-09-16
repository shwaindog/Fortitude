// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public partial class KeyValueCollectionBuilder
{
    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived>
        (IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived>
    (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)  
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendMatchOrNull(keyValue);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)   
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue  = selectKeys.MoveNext();
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : stb.AppendMatchOrNull(key, true).FieldEnd();
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(keyValue, valueFormatString)
                : stb.AppendMatchOrNull(keyValue);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
        (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
          , CustomTypeStyler<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)  
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)   
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(key, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)    
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue  = selectKeys.MoveNext();
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendMatchFormattedOrNull(key, keyFormatString, true).FieldEnd()
                : stb.AppendMatchOrNull(key, true).FieldEnd();
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue  = selectKeys.MoveNext();
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendOrNull(key, keyStyler, true).FieldEnd();
            stb.AppendOrNull(keyValue, valueStyler);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }
}
