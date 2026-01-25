#region

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#endregion

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeys(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeysEnumerate(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        WhenConditionMetAddWithSelectKeysEnumerate(value != null, fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
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

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
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

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        WhenConditionMetAddWithSelectKeysEnumerate
            (value != null, fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString, formatFlags);
}
