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
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) 
        where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyStyler) : stb.StyleTypeBuilder;


}
