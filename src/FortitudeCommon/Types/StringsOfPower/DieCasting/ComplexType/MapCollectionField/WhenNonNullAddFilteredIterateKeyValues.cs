// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        WhenConditionMetAddFilteredIterate(value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? =>
        WhenConditionMetAddFilteredIterate(value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        WhenConditionMetAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>
            (value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        WhenConditionMetAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>
            (value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateValueRevealer
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateValueRevealer
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct =>
        WhenConditionMetAddFilteredIterateNullValueRevealer
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct =>
        WhenConditionMetAddFilteredIterateNullValueRevealer
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?
        where TValue : struct =>
        WhenConditionMetAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct =>
        WhenConditionMetAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    
    public TMold WhenNonNullAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKey : struct
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullValueRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullValueRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct =>
        WhenConditionMetAddFilteredIterateBothNullRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct =>
        WhenConditionMetAddFilteredIterateBothNullRevealers
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

}
