using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
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
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
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
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue>(
        bool condition
      , string fieldName
      , KeyValuePair<TKey?, TValue?>[]? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
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
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFiltered<TKey, TValue>(
        bool condition
      , string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
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
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue>(
        bool condition
      , string fieldName
      , IEnumerable<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
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
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue>>? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TKey, TValue>(
        bool condition
      , string fieldName
      , IEnumerator<KeyValuePair<TKey?, TValue?>>? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct =>
        !stb.SkipFields && value != null
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);
}
