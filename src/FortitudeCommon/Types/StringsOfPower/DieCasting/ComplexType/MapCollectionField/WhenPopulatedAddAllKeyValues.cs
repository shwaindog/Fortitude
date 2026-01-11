// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
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
                var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString, formatFlags);
                    hasValue = value.MoveNext();
                }
                ekcb.AppendCollectionComplete();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull  =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct   =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TValue : TVRevealBase? 
         where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
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
                var ekcb      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                    hasValue = value.MoveNext();
                }
                ekcb.AppendCollectionComplete();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
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
                var ekcb      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
                    hasValue = value.MoveNext();
                }
                ekcb.AppendCollectionComplete();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue?>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<KeyValuePair<TKey?, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<KeyValuePair<TKey, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<KeyValuePair<TKey?, TValue?>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct 
        where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false) 
            ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey?, TValue?>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : TKRevealBase? 
         where TValue : TVRevealBase? 
         where TKRevealBase : notnull 
         where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : struct 
         where TValue : TVRevealBase? 
         where TVRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : TKRevealBase? 
         where TValue : struct 
         where TKRevealBase : notnull =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
         where TKey : struct 
         where TValue : struct  =>
        !stb.SkipFields && (value?.Any() ?? false)
            ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
                var ekcb      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                    hasValue = value.MoveNext();
                }
                ekcb.AppendCollectionComplete();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
                var ekcb      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                    hasValue = value.MoveNext();
                }
                ekcb.AppendCollectionComplete();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue, TKRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
                var ekcb      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                    hasValue = value.MoveNext();
                }
                ekcb.AppendCollectionComplete();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
                var ekcb      = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value, formatFlags);
                while (hasValue)
                {
                    var kvp = value.Current;
                    ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
                    hasValue = value.MoveNext();
                }
                ekcb.AppendCollectionComplete();
                return stb.AddGoToNext();
            }
        }
        return stb.StyleTypeBuilder;
    }
}
