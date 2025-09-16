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
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKey : TKBase where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
      !stb.SkipFields && value != null ?  AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler) : stb.StyleTypeBuilder;
}
