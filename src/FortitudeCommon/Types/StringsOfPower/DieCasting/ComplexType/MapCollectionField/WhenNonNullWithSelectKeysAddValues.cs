#region

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#endregion

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.KeyedCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IEnumerable<TKSelectDerived> selectKeys, PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys, PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
    , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TKSelectDerived : TKey 
      where TKey : TKRevealBase 
      where TValue : TVRevealBase? 
      where TKRevealBase : notnull
      where TVRevealBase : notnull =>
      !stb.SkipFields && value != null
        ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
        : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value, IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);
}
