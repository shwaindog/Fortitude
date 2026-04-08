// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{

  public TMold WhenNonNullAddAllIterate<TEnumtr>(
    string fieldName
  , TEnumtr? value
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
  , FormatFlags formatFlags = DefaultCallerTypeFlags)
    where TEnumtr : struct, IEnumerator =>
    WhenConditionMetAddAllIterate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

  public TMold WhenNonNullAddAllIterate<TEnumtr>(
    string fieldName
  , TEnumtr? value
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
  , FormatFlags formatFlags = DefaultCallerTypeFlags)
    where TEnumtr : IEnumerator? =>
    WhenConditionMetAddAllIterate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

  public TMold WhenNonNullAddAllIterate<TEnumtr, TKey, TValue>(
    string fieldName
  , TEnumtr? value
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
  , FormatFlags formatFlags = DefaultCallerTypeFlags)
    where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>> =>
    WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>
      (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

  public TMold WhenNonNullAddAllIterate<TEnumtr, TKey, TValue>(
    string fieldName
  , TEnumtr? value
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
  , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
  , FormatFlags formatFlags = DefaultCallerTypeFlags)
    where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>? =>
    WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>
      (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    
    public TMold WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
      string fieldName
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
      WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
      string fieldName
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
      WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator
        where TKey : struct  
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?
        where TKey : struct  
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct 
        where TValue : struct  =>
      WhenConditionMetAddAllIterateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct 
        where TValue : struct  =>
      WhenConditionMetAddAllIterateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

}
