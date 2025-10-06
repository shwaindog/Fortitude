﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            stb.StartDictionary(value!);
            while (hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendMatchOrNull(kvp.Value);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            stb.StartDictionary(value!);
            while (hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString, true).FieldEnd()
                    : stb.AppendMatchOrNull(kvp.Key, true).FieldEnd();
                stb.AppendOrNull(kvp.Value, valueRevealer);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
         where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var kvpType   = typeof(KeyValuePair<TKey, TValue>);
            var itemCount = 0;
            stb.StartDictionary(value!);
            while (hasValue)
            {
                var kvp = value!.Current;
                stb.AppendOrNull(kvp.Key, keyRevealer, true);
                stb.AppendOrNull(kvp.Value, valueRevealer);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(kvpType, itemCount++);
            }
            stb.EndDictionary();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
}
