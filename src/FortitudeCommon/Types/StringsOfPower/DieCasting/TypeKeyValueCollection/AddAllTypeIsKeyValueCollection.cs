// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

public partial class KeyValueCollectionMold
{
    public KeyValueCollectionMold AddAll<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        AddAllEnumerate(value, valueFormatString, keyFormatString);

    public KeyValueCollectionMold AddAll<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase =>
        AddAllEnumerate(value, valueStyler, keyFormatString);

    public KeyValueCollectionMold AddAll<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
          , PalantírReveal<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TVBase>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TVBase> (IEnumerable<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            keyFormatString   ??= "";
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TVBase>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : TVBase
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
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase =>
        AddAllEnumerate(value, valueStyler, keyStyler);

    public KeyValueCollectionMold AddAll<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
          , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAll<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        if (value != null)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionMold AddAllEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVBase> valueStyler, PalantírReveal<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase
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
                stb.AppendOrNull(kvp.Key, keyStyler, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.AddGoToNext();
    }

}
