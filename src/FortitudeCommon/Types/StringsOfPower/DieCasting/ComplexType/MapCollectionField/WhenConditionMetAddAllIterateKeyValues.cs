// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenConditionMetAddAllIterate<TEnumtr>(
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

    public TMold WhenConditionMetAddAllIterate<TEnumtr>(
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

    public TMold WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TValue>(
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

    public TMold WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TValue>(
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

    public TMold WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
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
    
    public TMold WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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
