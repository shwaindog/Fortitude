// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{

    public TMold WhenNonNullAddAllEnumerate<TEnumbl>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
      WhenConditionMetAddAllEnumerate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerate<TEnumbl>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
      WhenConditionMetAddAllEnumerate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerate<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>> =>
      WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerate<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>? =>
      WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable?
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
      string fieldName
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
      WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
      string fieldName
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
      WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    
    public TMold WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable
       where TKey : struct  
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable?
       where TKey : struct  
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
       where TKey : struct 
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
       where TKey : struct 
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

}
