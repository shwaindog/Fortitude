// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

public interface IAddSelectKeysTypeIsKeyValueCollection: IRecyclableObject
{
    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue> value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue> value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue> value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue> value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue> value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> value
      , TKey[] selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;

    KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TKey : struct where TValue : struct;
}

public class AddSelectKeysTypeIsKeyValueCollection : RecyclableObject, IAddSelectKeysTypeIsKeyValueCollection
{
    private IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> stb = null!;

    public AddSelectKeysTypeIsKeyValueCollection Initialize(IStyleTypeBuilderComponentAccess<KeyValueCollectionBuilder> styledComplexTypeBuilder)
    {
        stb  = styledComplexTypeBuilder;

        return this;
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : notnull =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueFormatString, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                _ = valueFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                    : stb.AppendOrNull(keyValue);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull
    {
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(key).Append(": ");
            _ = valueFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(keyValue, valueFormatString)
                : stb.AppendOrNull(keyValue);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TKey : notnull where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
     ,  [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
         , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct   =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyFormatString);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys, StructStyler<TValue> valueStructStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                _ = keyFormatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                    : stb.AppendOrNull(key).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKey> selectKeys, StructStyler<TValue> valueStructStyler 
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) where TValue : struct 
    {
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            _ = keyFormatString.IsNotNullOrEmpty()
                ? stb.AppendFormattedOrNull(key, keyFormatString).AppendOrNull(": ")
                : stb.AppendOrNull(key).Append(": ");
            stb.AppendOrNull(keyValue, valueStructStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(ConcurrentDictionary<TKey, TValue>? value
      , TKey[] selectKeys , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (ConcurrentDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>
    (ConcurrentDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) 
        where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(Dictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  =>
        From((IReadOnlyDictionary<TKey, TValue>?)value, selectKeys, valueStructStyler, keyStructStyler);

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Count; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendOrNull(key, keyStructStyler).Append(": ");
            stb.AppendOrNull(keyValue, valueStructStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            for (var i = 0; i < selectKeys.Length; i++)
            {
                var key = selectKeys[i];
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
          , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        if (value != null)
        {
            foreach (var key in selectKeys)
            {
                if (!value.TryGetValue(key, out var keyValue)) continue;
                stb.AppendOrNull(key, keyStructStyler).Append(": ");
                stb.AppendOrNull(keyValue, valueStructStyler);
                stb.GoToNextCollectionItemStart();
            }
        }
        return stb.AddGoToNext();
    }

    public KeyValueCollectionBuilder From<TKey, TValue>
    (IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        var hasValue = selectKeys.MoveNext();
        while(hasValue && value != null) 
        {
            var key = selectKeys.Current;
            if (!value.TryGetValue(key, out var keyValue))
            {
                hasValue = selectKeys.MoveNext();
                continue;
            }
            stb.AppendOrNull(key, keyStructStyler).Append(": ");
            stb.AppendOrNull(keyValue, valueStructStyler);
            stb.GoToNextCollectionItemStart();
            hasValue = selectKeys.MoveNext();
        } 
        return stb.AddGoToNext();
    }

    public override void StateReset()
    {
        stb  = null!;

        base.StateReset();
    }
}
