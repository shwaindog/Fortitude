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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
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
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKey>
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKey>
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKey>
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKSelectEnumbl : IEnumerable<TKey>
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKSelectEnumbl : IEnumerable<TKey>
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                if (ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            }
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKey>?
        where TKey: notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ItemCount == 0)
            {
                BeforeFirstElement(Mws);
            }
            Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            Mws.FieldEnd();
            Mws.AppendMatchFormattedOrNull(keyValue, valueFormatString, formatFlags);
            Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKey>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            keyFormatString ??= "";
            var key = selectKeys!.Current!;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ItemCount == 0)
            {
                BeforeFirstElement(Mws);
            }
            Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            Mws.FieldEnd();
            Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKey>?
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            keyFormatString ??= "";
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ItemCount == 0)
            {
                BeforeFirstElement(Mws);
            }
            Mws.AppendMatchFormattedOrNull(key, keyFormatString, formatFlags | IsFieldName);
            Mws.FieldEnd();
            Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKey>?
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ItemCount == 0)
            {
                BeforeFirstElement(Mws);
            }
            Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
            Mws.FieldEnd();
            Mws.RevealCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddWithSelectKeysIterate<TKey, TValue, TKSelectTEnumtr, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectTEnumtr? selectKeys
      , PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectTEnumtr : IEnumerator<TKey>?
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        var hasValue = selectKeys?.MoveNext() ?? false;
        var kvpType  = typeof(KeyValuePair<TKey, TValue>);
        while (hasValue && value != null)
        {
            var key = selectKeys!.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            if (ItemCount == 0)
            {
                BeforeFirstElement(Mws);
            }
            Mws.RevealCloakedBearerOrNull(key, keyStyler, null, formatFlags | IsFieldName);
            Mws.FieldEnd();
            Mws.RevealNullableCloakedBearerOrNull(keyValue, valueStyler, valueFormatString, formatFlags);
            Mws.GoToNextCollectionItemStart(kvpType, ItemCount++);
            hasValue = selectKeys.MoveNext();
        }
        return Mws.Mold;
    }
}
