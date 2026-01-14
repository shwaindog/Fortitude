// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public partial class KeyedCollectionMold
{
    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        var hasValue = selectKeys.MoveNext();
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while (hasValue && value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
            stb.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        var hasValue = selectKeys.MoveNext();
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while (hasValue && value != null)
        {
            keyFormatString ??= "";
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
            stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
        where TValue : struct
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        var hasValue = selectKeys.MoveNext();
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while (hasValue && value != null)
        {
            keyFormatString ??= "";
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName, true).FieldEnd();
            stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            ItemCount = 0;
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
                stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), "", formatFlags);
        var hasValue = selectKeys.MoveNext();
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        ItemCount = 0;
        while (hasValue && value != null)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
            stb.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return stb.StyleTypeBuilder;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectDerived : TKey
        where TKRevealBase : notnull
    {
        if (stb.HasSkipBody<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags))
            return stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), "", formatFlags);
        var hasValue = selectKeys.MoveNext();
        var kvpType  = typeof(KeyValuePair<TKey, TValue?>);
        ItemCount = 0;
        while (hasValue && value != null)
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName).FieldEnd();
            stb.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            stb.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return stb.StyleTypeBuilder;
    }
}
