// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeKeyValueCollection;

public partial class KeyedCollectionMold
{
    public KeyedCollectionMold AddAll<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags) =>
        AddAllEnumerate(value, valueFormatString, keyFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        AddAllEnumerate(value, valueStyler, keyFormatString, formatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue>(IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : struct =>
        AddAllEnumerate(value, valueStyler, keyFormatString, formatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>
    (IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TVRevealBase>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue>
    (IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AddAllEnumerate(value, valueStyler, keyStyler, formatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey?, TValue>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        AddAllEnumerate(value, valueStyler, keyStyler, formatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull =>
        AddAllEnumerate(value, valueStyler, keyStyler, formatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue>(IReadOnlyDictionary<TKey?, TValue?>? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        AddAllEnumerate(value, valueStyler, keyStyler, formatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.SkipField<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.SkipField<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TVRevealBase>(IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey?, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey?, TValue>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TKRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        if (stb.SkipField<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            ItemCount = 0;
            foreach (var kvp in value)
            {
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TVRevealBase>
        (IEnumerator<KeyValuePair<TKey?, TValue>>? value, PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKey> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey?, TValue>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey?, TValue>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue>);
            ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue, TKRevealBase>
    (IEnumerator<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TValue> valueStyler, PalantírReveal<TKRevealBase> keyStyler
      , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.RevealCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddAllEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey?, TValue?>>? value, PalantírReveal<TValue> valueStyler, PalantírReveal<TKey> keyStyler
          , string? formatString = null, FieldContentHandling formatFlags = DefaultCallerTypeFlags)
            where TKey : struct
            where TValue : struct
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), "", formatFlags);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType = typeof(KeyValuePair<TKey?, TValue?>);
            ItemCount = 0;
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueStyler, formatString, formatFlags);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }
}
