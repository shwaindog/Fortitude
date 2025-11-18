// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

public partial class KeyValueCollectionMold
{
    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKDerived>
        (IReadOnlyDictionary<TKey, TValue?>? value, TKDerived[] selectKeys
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
    (IReadOnlyDictionary<TKey, TValue?>? value, Span<TKDerived> selectKeys
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
    (IReadOnlyDictionary<TKey, TValue?>? value, ReadOnlySpan<TKDerived> selectKeys
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
    (IReadOnlyDictionary<TKey, TValue?>? value, IReadOnlyList<TKDerived> selectKeys
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
    (IReadOnlyDictionary<TKey, TValue?>? value, IEnumerable<TKDerived> selectKeys
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
        (IReadOnlyDictionary<TKey, TValue?>? value, IEnumerator<TKDerived> selectKeys
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

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
        (IReadOnlyDictionary<TKey, TValue?>? value, Span<TKSelectDerived> selectKeys
          , PalantírReveal<TVRevealBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKSelectDerived : TKey  
        where TValue : TVRevealBase
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
        (IReadOnlyDictionary<TKey, TValue?>? value, ReadOnlySpan<TKSelectDerived> selectKeys
          , PalantírReveal<TVRevealBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKSelectDerived : TKey  
        where TValue : TVRevealBase
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)  
        where TKSelectDerived : TKey  
        where TValue : TVRevealBase
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, IEnumerable<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)   
        where TKSelectDerived : TKey  
        where TValue : TVRevealBase
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
        (IReadOnlyDictionary<TKey, TValue?>? value, IEnumerator<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags)    
        where TKSelectDerived : TKey  
        where TValue : TVRevealBase
        where TVRevealBase : notnull 
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
            stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull 
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
                stb.RevealCloakedBearerOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
        (IReadOnlyDictionary<TKey, TValue?>? value, IEnumerable<TKSelectDerived> selectKeys
          , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKRevealBase, TVRevealBase>
    (IReadOnlyDictionary<TKey, TValue?>? value, IEnumerator<TKDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKDerived : TKey
        where TKRevealBase : notnull 
        where TVRevealBase : notnull 
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
            stb.RevealCloakedBearerOrNull(key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
            stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }
}
