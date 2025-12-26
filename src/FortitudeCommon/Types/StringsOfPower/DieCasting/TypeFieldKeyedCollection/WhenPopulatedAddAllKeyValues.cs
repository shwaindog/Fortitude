// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyedCollection;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags, keyFormatString ?? "");
        if (value != null)
        {
            var hasValue  = value.MoveNext();
            if (hasValue)
            {
                stb.FieldNameJoin(fieldName);
                var kvpType   = typeof(KeyValuePair<TKey, TValue>);
                var itemCount = 0;
                stb.StartDictionary(value.GetType(), typeof(TKey), typeof(TValue), formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
                    stb.AppendMatchFormattedOrNull(kvp.Value, valueFormatString ?? "");
                    hasValue = value.MoveNext();
                    stb.GoToNextCollectionItemStart(kvpType, itemCount++);
                }
                stb.EndDictionary();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull  =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct   =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TValue : TVRevealBase? 
         where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags, keyFormatString ?? "");
        if (value != null)
        {
            var hasValue = value.MoveNext();
            if (hasValue)
            {
                stb.FieldNameJoin(fieldName);
                var kvpType   = typeof(KeyValuePair<TKey, TValue>);
                var itemCount = 0;
                stb.StartDictionary(value.GetType(), typeof(TKey), typeof(TValue), formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
                    stb.RevealCloakedBearerOrNull(kvp.Value, valueRevealer);
                    hasValue = value.MoveNext();
                    stb.GoToNextCollectionItemStart(kvpType, itemCount++);
                }
                stb.EndDictionary();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct 
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags, keyFormatString ?? "");
        if (value != null)
        {
            var hasValue = value.MoveNext();
            if (hasValue)
            {
                stb.FieldNameJoin(fieldName);
                var kvpType   = typeof(KeyValuePair<TKey, TValue?>);
                var itemCount = 0;
                stb.StartDictionary(value.GetType(), typeof(TKey), typeof(TValue?), formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    stb.AppendMatchFormattedOrNull(kvp.Key, keyFormatString ?? "", DefaultCallerTypeFlags, true).FieldEnd();
                    stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer);
                    hasValue = value.MoveNext();
                    stb.GoToNextCollectionItemStart(kvpType, itemCount++);
                }
                stb.EndDictionary();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<KeyValuePair<TKey?, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<KeyValuePair<TKey?, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : TKRevealBase? 
         where TValue : TVRevealBase? 
         where TKRevealBase : notnull 
         where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : struct 
         where TValue : TVRevealBase? 
         where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : TKRevealBase? 
         where TValue : struct 
         where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : struct 
         where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull 
        where TVRevealBase : notnull 
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            var hasValue = value.MoveNext();
            if (hasValue)
            {
                stb.FieldNameJoin(fieldName);
                var kvpType   = typeof(KeyValuePair<TKey, TValue>);
                var itemCount = 0;
                stb.StartDictionary(value.GetType(), typeof(TKey), typeof(TValue), formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    stb.RevealCloakedBearerOrNull(kvp.Key, keyRevealer, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                    stb.RevealCloakedBearerOrNull(kvp.Value, valueRevealer);
                    hasValue = value.MoveNext();
                    stb.GoToNextCollectionItemStart(kvpType, itemCount++);
                }
                stb.EndDictionary();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKey> keyRevealer
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            var hasValue = value.MoveNext();
            if (hasValue)
            {
                stb.FieldNameJoin(fieldName);
                var kvpType   = typeof(KeyValuePair<TKey, TValue>);
                var itemCount = 0;
                stb.StartDictionary(value.GetType(), typeof(TKey?), typeof(TValue), formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyRevealer, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                    stb.RevealCloakedBearerOrNull(kvp.Value, valueRevealer);
                    hasValue = value.MoveNext();
                    stb.GoToNextCollectionItemStart(kvpType, itemCount++);
                }
                stb.EndDictionary();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : struct
        where TKRevealBase : notnull 
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            var hasValue = value.MoveNext();
            if (hasValue)
            {
                stb.FieldNameJoin(fieldName);
                var kvpType   = typeof(KeyValuePair<TKey, TValue>);
                var itemCount = 0;
                stb.StartDictionary(value.GetType(), typeof(TKey), typeof(TValue?), formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    stb.RevealCloakedBearerOrNull(kvp.Key, keyRevealer, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                    stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer);
                    hasValue = value.MoveNext();
                    stb.GoToNextCollectionItemStart(kvpType, itemCount++);
                }
                stb.EndDictionary();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct 
    {
        if (stb.SkipField<IEnumerator<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags)) 
            return stb.WasSkipped<IEnumerator<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);
        formatFlags = stb.StyleFormatter.ResolveContentFormattingFlags(stb.Sb, value, formatFlags);
        if (value != null)
        {
            var hasValue = value.MoveNext();
            if (hasValue)
            {
                stb.FieldNameJoin(fieldName);
                var kvpType   = typeof(KeyValuePair<TKey?, TValue?>);
                var itemCount = 0;
                stb.StartDictionary(value.GetType(), typeof(TKey?), typeof(TValue?) , formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    stb.RevealNullableCloakedBearerOrNull(kvp.Key, keyRevealer, formatString, DefaultCallerTypeFlags, true).FieldEnd();
                    stb.RevealNullableCloakedBearerOrNull(kvp.Value, valueRevealer);
                    hasValue = value.MoveNext();
                    stb.GoToNextCollectionItemStart(kvpType, itemCount++);
                }
                stb.EndDictionary();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }
}
