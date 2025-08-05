using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ?  AlwaysAddFiltered(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;
}
