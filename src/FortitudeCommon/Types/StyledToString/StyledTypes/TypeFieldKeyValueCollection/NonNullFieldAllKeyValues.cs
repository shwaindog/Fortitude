// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public interface INonNullFieldIncludeAllKeyValues<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;
}

public class NonNullFieldIncludeAllKeyValues<TExt> : RecyclableObject, INonNullFieldIncludeAllKeyValues<TExt> 
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysFieldIncludeAllKeyValues<TExt> aikv = null!;

    public NonNullFieldIncludeAllKeyValues<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder
      , IAlwaysFieldIncludeAllKeyValues<TExt> alwaysIncludeAllKeyValues)
    {
        stb  = styledComplexTypeBuilder;
        aikv = alwaysIncludeAllKeyValues;

        return this;
    }

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aikv.WithName(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aikv.WithName(fieldName, value, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;


}
