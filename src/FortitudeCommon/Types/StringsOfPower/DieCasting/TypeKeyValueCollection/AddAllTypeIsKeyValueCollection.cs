// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

public partial class KeyValueCollectionMold
{
    public KeyValueCollectionMold AddAll<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) =>
        AddAllEnumerate(value, valueFormatString, keyFormatString, valueFlags);

    public KeyValueCollectionMold AddAll<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, valueFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AddAllEnumerate(value, valueStyler, keyFormatString, valueFlags);

    public KeyValueCollectionMold AddAll<TKey, TValue, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
          , PalantírReveal<TVRevealBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TVRevealBase>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TVRevealBase> (IEnumerable<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TVRevealBase>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
          , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase  
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull  =>
        AddAllEnumerate(value, valueStyler, keyStyler, valueFlags);

    public KeyValueCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
          , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase?  
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase?  
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase?  
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull  
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , FieldContentHandling valueFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase?  
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, valueFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

}
