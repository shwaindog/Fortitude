using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public interface INonNullFieldIncludeSelectKeys<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;
}

public class NonNullFieldIncludeSelectKeys<TExt> : RecyclableObject, INonNullFieldIncludeSelectKeys<TExt> 
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysFieldIncludeSelectKeys<TExt> aisk = null!;

    public NonNullFieldIncludeSelectKeys<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder
      , IAlwaysFieldIncludeSelectKeys<TExt> alwaysIncludeSelectKeys)
    {
        stb = styledComplexTypeBuilder;

        aisk = alwaysIncludeSelectKeys;

        return this;
    }

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
    value != null 
    ? aisk.WithName(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
    : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull where TValue : struct  =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct  =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? aisk.WithName(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public override void StateReset()
    {
        stb  = null!;
        aisk = null!;

        base.StateReset();
    }
}
