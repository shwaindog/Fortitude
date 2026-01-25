using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        WhenConditionMetAddFilteredEnumerate(value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        WhenConditionMetAddFilteredEnumerate(value != null, fieldName, value, filterPredicate, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : struct =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        WhenConditionMetAddFiltered(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate(value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFiltered
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFiltered
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey?, TValue?>[]? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        WhenConditionMetAddFiltered
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFiltered
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFiltered
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFiltered
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFiltered<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        WhenConditionMetAddFiltered
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue>(
        string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        WhenConditionMetAddFilteredEnumerate
            (value != null, fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString, formatFlags);
}
