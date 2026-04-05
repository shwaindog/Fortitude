// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.FixtureScaffolding;

public abstract class FormattedKeyValueCollectionMoldScaffold<TKey, TValue> : FormattedKeyValueFieldMoldScaffold<TKey, TValue>
, IList<KeyValuePair<TKey, TValue>>
{
    IEnumerator IEnumerable.                        GetEnumerator() => GetEnumerator();
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Value?.GetEnumerator()!;

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = ( Value != null ? Value.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value : default);
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }


    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Value?.Add(item);
    }
    
    public void Clear()
    {
        Value?.Clear();
    }
    
    // ReSharper disable once UsageOfDefaultStructEquality
    public bool Contains(KeyValuePair<TKey, TValue> item) => Value?.Contains(item) ?? false;

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        Value?.CopyTo(array, arrayIndex);
    }
    
    // ReSharper disable once UsageOfDefaultStructEquality
    public bool Remove(KeyValuePair<TKey, TValue> item) => Value?.Remove(item) ?? false;

    public bool IsReadOnly => false;

    // ReSharper disable once UsageOfDefaultStructEquality
    public int  IndexOf(KeyValuePair<TKey, TValue> item) => Value?.IndexOf(item) ?? -1;

    public void Insert(int index, KeyValuePair<TKey, TValue> item)
    {
        Value?.Insert(index, item);
    }
    
    public void RemoveAt(int index)
    {
        Value?.RemoveAt(index);
    }

    public KeyValuePair<TKey, TValue> this[int index]
    {
        get => Value![index];
        set => Value![index] = value;
    }

    public IEnumerable<TKey> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class FormattedKeyValueMoldScaffold<TKey, TValue> : FormattedKeyValueFieldMoldScaffold<TKey, TValue>
, IReadOnlyDictionary<TKey, TValue>
{

    IEnumerator IEnumerable.                        GetEnumerator() => GetEnumerator();
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Value?.GetEnumerator()!;

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = ( Value != null ? Value.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value : default);
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }

    public TValue this[TKey key] => TryGetValue(key, out var result) ? result : default!;
    
    public IEnumerable<TKey> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, IReadOnlyDictionary<TKey, TValue>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = ( Value != null ? Value.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value : default);
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }

    public TValue this[TKey key] => TryGetValue(key, out var result) ? result : default!;
    
    public IEnumerable<TKey> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>, IReadOnlyDictionary<TKey, TValue?>
    where TValue : struct
{

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue?>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, out TValue? value)
    {
        value = Value?.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value;
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }

    public TValue? this[TKey key] => TryGetValue(key, out var result) ? result : null;
    
    public IEnumerable<TKey> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue?> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, IReadOnlyDictionary<TKey, TValue>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = ( Value != null ? Value.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value : default);
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }

    public TValue this[TKey key] => TryGetValue(key, out var result) ? result : default!;
    
    public IEnumerable<TKey> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, IEnumerable<KeyValuePair<TKey?, TValue>>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey?, TValue>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = ( Value != null ? Value.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value : default);
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }

    public TValue this[TKey key] => TryGetValue(key, out var result) ? result : default!;
    
    public IEnumerable<TKey?> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>, IReadOnlyDictionary<TKey, TValue?>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue?>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, out TValue? value)
    {
        value = Value?.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value;
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }

    public TValue? this[TKey key] => TryGetValue(key, out var result) ? result : null;
    
    public IEnumerable<TKey> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue?> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>, IEnumerable<KeyValuePair<TKey?, TValue?>>
    where TKey : struct
    where TValue : struct
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey?, TValue?>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;

    public bool TryGetValue(TKey key, out TValue? value)
    {
        value = Value?.FirstOrDefault(kvp => Equals(kvp.Key, key)).Value;
        return Value?.Any(kvp => Equals(kvp.Key, key)) ?? false;
    }

    public TValue? this[TKey key] => TryGetValue(key, out var result) ? result : null;
    
    public IEnumerable<TKey?> Keys => Value?.Select(kvp => kvp.Key) ?? [];
    
    public IEnumerable<TValue?> Values => Value?.Select(kvp => kvp.Value) ?? [];
}

public abstract class FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredFormattedKeyStructValueMoldScaffold<TKey, TValue, TKFilterBase> : 
    FormattedKeyValueMoldScaffold<TKey, TValue?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>
    where TKey : TKFilterBase?
    where TValue : struct
{
    public KeyValuePredicate<TKFilterBase, TValue?> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>.GetNoFilterPredicate;
}

public abstract class FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> 
    : FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase> 
    : FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>, ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>
    where TKey : TKFilterBase?
    where TValue : struct
{
    public KeyValuePredicate<TKFilterBase, TValue?> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>.GetNoFilterPredicate;
}

public abstract class FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKey?, TVFilterBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKey?, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKey?, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TValue?> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>.GetNoFilterPredicate;
}

public abstract class FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>, ISupportsKeyedCollectionPredicate<TKey?, TValue?>
    where TKey : struct
    where TValue : struct
{
    public KeyValuePredicate<TKey?, TValue?> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKey?, TValue?>.GetNoFilterPredicate;
}

public abstract class SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    : FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived> 
    : FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TValue : struct
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList() ?? [];
        set => displayKeys = value;
    }
}
