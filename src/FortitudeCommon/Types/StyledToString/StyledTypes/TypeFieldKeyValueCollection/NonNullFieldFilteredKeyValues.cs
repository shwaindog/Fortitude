using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
        value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
        value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : struct =>
        value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : struct =>
        value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler
      , CustomTypeStyler<TKey> keyStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;
}
