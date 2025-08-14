using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
    value != null 
    ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
    : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;
}
