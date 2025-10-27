// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

public partial class KeyValueCollectionMold
{
    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived>
        (IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived>
    (IReadOnlyDictionary<TKey, TValue>? value, Span<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived>
    (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)  
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags)   
        where TKDerived : TKey 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue  = selectKeys.MoveNext();
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while(hasValue && value != null) 
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
            stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, valueFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , PalantírReveal<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKDerived : TKey where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
        (IReadOnlyDictionary<TKey, TValue>? value, Span<TKDerived> selectKeys
          , PalantírReveal<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
        (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
          , PalantírReveal<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , PalantírReveal<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)  
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, PalantírReveal<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)   
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, PalantírReveal<TVBase> valueStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)    
        where TKDerived : TKey  where TValue : TVBase 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue  = selectKeys.MoveNext();
        var kvpType   = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while(hasValue && value != null) 
        {
            keyFormatString   ??= "";
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendMatchFormattedOrNull(key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
            stb.AppendOrNull(keyValue, valueStyler, valueFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
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
                stb.AppendOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, Span<TKDerived> selectKeys
      , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
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
                stb.AppendOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
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
                stb.AppendOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
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
                stb.AppendOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
          , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
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
                stb.AppendOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
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
            stb.AppendOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
            stb.AppendOrNull(keyValue, valueStyler, valueFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }
}
