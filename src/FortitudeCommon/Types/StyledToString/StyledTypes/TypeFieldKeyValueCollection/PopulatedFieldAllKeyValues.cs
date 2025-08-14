// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value?.Any() ?? false ?  AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Value, valueFormatString)
                    : stb.AppendOrNull(kvp.Value);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAllEnumerate(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(kvp.Key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(kvp.Key).Append(": ");
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAll(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value?.Any() ?? false ?  AlwaysAddAllEnumerate(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct 
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartDictionary();
            while(hasValue)
            {
                var kvp = value!.Current;
                stb.AppendOrNull(kvp.Key, keyStructStyler);
                stb.AppendOrNull(kvp.Value, valueStructStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndDictionary();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

}
