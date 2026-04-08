// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenConditionMetAddAllEnumerate<TEnumbl>(
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

    public TMold WhenConditionMetAddAllEnumerate<TEnumbl>(
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

    public TMold WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>(
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
    
    public TMold WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
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

    public TMold WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
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

    public TMold WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
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

    public TMold WhenConditionMetAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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

    public TMold WhenConditionMetAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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

}
