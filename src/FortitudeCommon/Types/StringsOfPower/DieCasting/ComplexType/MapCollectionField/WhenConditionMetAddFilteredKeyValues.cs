using System.Collections;
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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable? =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerate<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        condition
            ? AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator =>
        condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? =>
        condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterate<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase? =>
        condition
            ? AlwaysAddFilteredIterate(fieldName, value, filterPredicate, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateValueRevealer<TEnumbl, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateValueRevealer<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredEnumerateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullValueRevealer<TEnumbl, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredEnumerateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredEnumerateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateNullValueRevealer<TEnumbl, TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredEnumerateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateValueRevealer<TEnumtr, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateValueRevealer<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredIterateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullValueRevealer<TEnumtr, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredIterateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredIterateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateNullValueRevealer<TEnumtr, TKey, TValue, TKFilterBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredIterateNullValueRevealer(fieldName, value, filterPredicate, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

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
        condition
            ? AlwaysAddFiltered(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothRevealers<TEnumbl, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothRevealers<TEnumbl, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TKey : struct
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TKey : struct
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredEnumerateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredEnumerateBothNullRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
        bool condition
      , string fieldName
      , TEnumbl? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredEnumerateBothNullRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumbl), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothRevealers<TEnumtr, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothRevealers<TEnumtr, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TKey : struct
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TKey : struct
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVFilterBase, TVRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullKeyRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKFilterBase, TKRevealBase>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKFilterBase, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKFilterBase?, TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        condition
            ? AlwaysAddFilteredIterateBothWithNullValueRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredIterateBothNullRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);

    public TExt WhenConditionMetAddFilteredIterateBothNullRevealers<TEnumtr, TKey, TValue>(
        bool condition
      , string fieldName
      , TEnumtr? value
      , KeyValuePredicate<TKey?, TValue?> filterPredicate
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct =>
        condition
            ? AlwaysAddFilteredIterateBothNullRevealers(fieldName, value, filterPredicate, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(TEnumtr), fieldName, formatFlags);
}
