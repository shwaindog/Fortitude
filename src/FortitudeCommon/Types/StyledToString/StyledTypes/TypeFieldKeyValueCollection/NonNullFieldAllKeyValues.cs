// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? AlwaysAddAll(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;


}
