// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull =>
        WhenConditionMetAddWithSelectKeysIterate(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull =>
        WhenConditionMetAddWithSelectKeysIterate(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysIterateNullValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysIterateNullValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
}
