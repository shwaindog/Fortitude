// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
          ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
          : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue>(
      bool condition
    , string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue>(
      bool condition
    , string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : TVRevealBase?  
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : struct   =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue>(
      bool condition
    , string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue>(
      bool condition
    , string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase 
       where TValue : TVRevealBase? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase 
       where TValue : struct 
       where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey?, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey?, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : TKRevealBase? 
      where TValue : struct 
      where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : struct 
      where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase? 
       where TValue : TVRevealBase? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IEnumerable<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , IEnumerable<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue>(
      bool condition
    , string fieldName
    , IEnumerable<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : struct 
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IEnumerator<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , IEnumerator<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TKey, TValue>(
      bool condition
    , string fieldName
    , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);
}
