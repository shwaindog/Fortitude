#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#endregion

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

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
        WhenConditionMetAddWithSelectKeysIterate(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

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
        WhenConditionMetAddWithSelectKeysIterate(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

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
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateValueRevealer
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
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

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
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateValueRevealer
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
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysIterateNullValueRevealer
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TKSelectDerived : TKey 
      where TKey : TKRevealBase 
      where TValue : TVRevealBase? 
      where TKRevealBase : notnull
      where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TMold WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

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
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothRevealers
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
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

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
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothRevealers
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
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
}
