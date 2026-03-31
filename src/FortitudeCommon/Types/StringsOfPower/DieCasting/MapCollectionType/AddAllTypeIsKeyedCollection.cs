// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;

public partial class KeyedCollectionMold
{
    public KeyedCollectionMold AddAll<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        this.AddAllEnumerate<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, valueFormatString, keyFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        this.AddAllEnumerateValueRevealer<IReadOnlyDictionary<TKey, TValue>, TKey, TValue, TVRevealBase>
            (value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct =>
        this.AddAllEnumerateNullValueRevealer<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>
            (value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        this.AddAllEnumerateBothRevealers<IReadOnlyDictionary<TKey, TValue>, TKey, TValue, TKRevealBase, TVRevealBase>
            (value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(
        IReadOnlyDictionary<TKey?, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        this.AddAllEnumerateBothWithNullKeyRevealers<IReadOnlyDictionary<TKey?, TValue>, TKey, TValue, TVRevealBase>
            (value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase>(
        IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull =>
        this.AddAllEnumerateBothWithNullValueRevealers<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue, TKRevealBase>
            (value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue>(
        IReadOnlyDictionary<TKey?, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        this.AddAllEnumerateBothNullRevealers(value, valueRevealer, keyStyler, valueFormatString, formatFlags);

    public KeyedCollectionMold AddAll<TKey, TValue>(
        KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount += value.Length;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(
        KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount += value.Length;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(
        KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase>(
        KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(
        KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Length;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(
        IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            valueFormatString ??= "";
            keyFormatString   ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.AppendMatchFormattedOrNull(kvp.Value, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>
    (IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , string? valueFormatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue?>);
            for (var i = 0; i < value.Count; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue> (
        IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            keyFormatString ??= "";
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TVRevealBase>(
        IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue, TKRevealBase>(
        IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return Mws.Mold;
    }

    public KeyedCollectionMold AddAll<TKey, TValue>(
        IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipBody(actualType, "", formatFlags))
            return Mws.WasSkipped(actualType, "", formatFlags);
        if (value != null)
        {
            var kvpType = typeof(KeyValuePair<TKey, TValue>);
            for (var i = 0; i < value.Count; i++)
            {
                if (i == 0 && ItemCount == 0)
                {
                    BeforeFirstElement(Mws);
                }
                var kvp = value[i];
                Mws.RevealNullableCloakedBearerOrNull(kvp.Key, keyStyler, null, formatFlags | IsFieldName);
                Mws.FieldEnd();
                Mws.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer, valueFormatString, formatFlags);
                Mws.GoToNextCollectionItemStart(kvpType, i);
            }
            ItemCount = value.Count;
        }
        return Mws.Mold;
    }
}
