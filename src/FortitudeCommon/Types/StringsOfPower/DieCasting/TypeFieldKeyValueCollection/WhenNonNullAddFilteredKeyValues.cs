using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKey : TKFilterBase where TValue : TVFilterBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer)
         where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer)
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase =>
        !stb.SkipFields && value != null ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;
}
