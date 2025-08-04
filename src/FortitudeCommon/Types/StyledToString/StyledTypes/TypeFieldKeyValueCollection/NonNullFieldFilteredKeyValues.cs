using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public interface INonNullFieldIncludeFilteredKeyValues<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    T WithName<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null);

    T WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;

    T WithName<TKey, TValue>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct
        where TValue : struct;
}

public class NonNullFieldIncludeFilteredKeyValues<TExt> : RecyclableObject, INonNullFieldIncludeFilteredKeyValues<TExt> 
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysFieldIncludeFilteredKeyValues<TExt> aifkv = null!;

    public NonNullFieldIncludeFilteredKeyValues<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder
      , IAlwaysFieldIncludeFilteredKeyValues<TExt> alwaysIncludeFilteredKeyValues)
    {
        stb  = styledComplexTypeBuilder;
        aifkv = alwaysIncludeFilteredKeyValues;

        return this;
    }

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ConcurrentDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, Dictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, ICollection<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public TExt WithName<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        value != null ? aifkv.WithName(fieldName, value, filterPredicate, valueStructStyler, keyStructStyler) : stb.StyleTypeBuilder;

    public override void StateReset()
    {
        stb = null!;
        base.StateReset();
    }
}