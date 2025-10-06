#region

using System.Diagnostics.CodeAnalysis;

#endregion

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys, PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKSelectDerived : TKey where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;
}
