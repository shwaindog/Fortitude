// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeysEnumerateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

}            