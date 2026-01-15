// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        value != null 
          ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString, formatFlags) 
          : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      !stb.SkipFields && value != null 
        ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString, formatFlags) 
        : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>(
      string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>(
      string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : TVRevealBase?  
       where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : struct   =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TVRevealBase>(
      string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>(
      string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : struct  =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TVRevealBase>(
      string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>(
      string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
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
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase 
       where TValue : struct 
       where TKRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
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
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , KeyValuePair<TKey?, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase>(
      string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , KeyValuePair<TKey?, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : struct  =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
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
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : TKRevealBase? 
      where TValue : struct 
      where TKRevealBase : notnull =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : struct 
      where TValue : struct  =>
      value != null 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
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
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TVRevealBase>(
      string fieldName
    , IEnumerable<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKRevealBase>(
      string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>(
      string fieldName
    , IEnumerable<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : struct 
       where TValue : struct  =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
      string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TVRevealBase>(
      string fieldName
    , IEnumerator<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKRevealBase>(
      string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>(
      string fieldName
    , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : struct  =>
      value != null 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags) 
        : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);


}
