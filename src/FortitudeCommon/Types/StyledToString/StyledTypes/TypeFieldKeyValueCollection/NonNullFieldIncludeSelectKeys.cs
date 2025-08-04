using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString)
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
    value != null 
    ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
    : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull where TValue : struct  =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct  =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, (IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;

    public TExt AddWhenNonNullWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AddAlwaysWithSelectKeys(fieldName, value, selectKeys, valueStructStyler, keyStructStyler) 
            : stb.StyleTypeBuilder;
}
