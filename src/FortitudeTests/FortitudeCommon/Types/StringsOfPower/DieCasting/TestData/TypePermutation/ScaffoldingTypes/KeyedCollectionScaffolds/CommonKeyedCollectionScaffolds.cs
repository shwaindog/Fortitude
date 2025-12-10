// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.KeyedCollectionFields;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.KeyedCollectionScaffolds;

public abstract class FormattedKeyValueMoldScaffold<TKey, TValue> : FormattedKeyValueFieldMoldScaffold<TKey, TValue>
, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{

    IEnumerator IEnumerable.                        GetEnumerator() => GetEnumerator();
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.ContainsKey(key) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = default;
        return Value?.TryGetValue(key, out value) ?? false;
    }

    public TValue this[TKey key] => TryGetValue(key, out var result) ? result : default!;
    
    public IEnumerable<TKey> Keys => Value?.Keys ?? Enumerable.Empty<TKey>();
    
    public IEnumerable<TValue> Values => Value?.Values ?? Enumerable.Empty<TValue>();
}

public abstract class FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, IReadOnlyDictionary<TKey, TValue?>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue?>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.ContainsKey(key) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = default;
        return Value?.TryGetValue(key, out value) ?? false;
    }

    public TValue this[TKey key] => TryGetValue(key, out var result) ? result : default!;
    
    public IEnumerable<TKey> Keys => Value?.Keys ?? Enumerable.Empty<TKey>();
    
    public IEnumerable<TValue?> Values => Value?.Values ?? Enumerable.Empty<TValue?>();
}

public abstract class KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, IReadOnlyDictionary<TKey, TValue>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Value!.GetEnumerator();

    public int Count => Value!.Count;

    public bool ContainsKey(TKey key) => Value?.ContainsKey(key) ?? false;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = default;
        return Value?.TryGetValue(key, out value) ?? false;
    }

    public TValue this[TKey key] => TryGetValue(key, out var result) ? result : default!;
    
    public IEnumerable<TKey> Keys => Value?.Keys ?? Enumerable.Empty<TKey>();
    
    public IEnumerable<TValue> Values => Value?.Values ?? Enumerable.Empty<TValue>();
}

public abstract class FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> 
    : FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    : FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }
}
