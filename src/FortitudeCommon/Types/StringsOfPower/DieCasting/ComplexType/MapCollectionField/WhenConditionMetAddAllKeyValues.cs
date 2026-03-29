// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
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
          : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
      condition 
        ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>> =>
      condition 
        ? AlwaysAddAllEnumerate<TEnumbl, TKey, TValue>(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>? =>
      condition 
        ? AlwaysAddAllEnumerate<TEnumbl, TKey, TValue>(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterate<TEnumtr>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator
      =>
        condition 
          ? AlwaysAddAllIterate(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
          : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterate<TEnumtr>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?
      =>
        condition 
          ? AlwaysAddAllIterate(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
          : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
      =>
      condition 
        ? AlwaysAddAllIterate<TEnumtr, TKey, TValue>(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
      =>
      condition 
        ? AlwaysAddAllIterate<TEnumtr, TKey, TValue>(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumbl : struct, IEnumerable 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumbl : IEnumerable? 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerateNullValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable?
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerateNullValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllIterateNullValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllIterateNullValueRevealer(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
          (fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
       where TKey : TKRevealBase? 
       where TValue : TVRevealBase? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
       where TKey : TKRevealBase? 
       where TValue : TVRevealBase? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable
       where TKey : struct  
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullKeyRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable?
       where TKey : struct  
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullKeyRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable
       where TValue : struct 
       where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullValueRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable?
       where TValue : struct 
       where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullValueRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
       where TKey : struct 
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerateBothNullRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
       where TKey : struct 
       where TValue : struct  =>
      condition 
        ? AlwaysAddAllEnumerateBothNullRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator
        where TKey : struct  
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullKeyRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?
        where TKey : struct  
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullKeyRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TValue : struct 
        where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullValueRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullValueRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
          (fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllIterateBothNullRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
      bool condition
    , string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAllIterateBothNullRevealers(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);
}
