using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags)
            : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString, formatFlags)
            : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyDictionary<TKey, TValue>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<KeyValuePair<TKey, TValue>[]>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IReadOnlyList<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IEnumerable<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , FieldContentHandling formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, formatFlags)
            : stb.WasSkipped<IEnumerator<KeyValuePair<TKey, TValue>>>(value?.GetType(), fieldName, formatFlags);
}
