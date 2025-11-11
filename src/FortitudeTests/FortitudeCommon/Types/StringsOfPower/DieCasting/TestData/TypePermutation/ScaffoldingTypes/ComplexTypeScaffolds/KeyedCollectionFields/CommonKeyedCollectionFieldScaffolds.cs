// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.KeyedCollectionFields;

public abstract class FormattedKeyValueFieldMoldScaffold<TKey, TValue> : FormattedMoldScaffold<Dictionary<TKey, TValue>>, ISupportsKeyFormatString
    where TKey : notnull
{
    public string? KeyFormatString { get; set; }
}

public abstract class FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealerBase> : 
    MoldScaffoldBase<Dictionary<TKey, TValue>>, ISupportsKeyFormatString, ISupportsValueRevealer<TVRevealerBase>
    where TKey : notnull
    where TValue : TVRevealerBase
{
    public Delegate ValueRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TVRevealerBase> ValueRevealer
    {
        get => (PalantírReveal<TVRevealerBase>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
    public string? KeyFormatString { get; set; }
}

public abstract class KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealerBase, TVRevealerBase> : 
    MoldScaffoldBase<Dictionary<TKey, TValue>>, ISupportsKeyRevealer<TKRevealerBase>, ISupportsValueRevealer<TVRevealerBase>
    where TKey : notnull, TKRevealerBase
    where TValue : TVRevealerBase
{
    public Delegate KeyRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TKRevealerBase> KeyRevealer
    {
        get => (PalantírReveal<TKRevealerBase>)KeyRevealerDelegate;
        set => KeyRevealerDelegate = value;
    }
    
    public Delegate ValueRevealerDelegate { get; set; } = null!;
    
    public PalantírReveal<TVRevealerBase> ValueRevealer
    {
        get => (PalantírReveal<TVRevealerBase>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
}

public abstract class FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TValueRevealBase> 
    : FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TValueRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : notnull
    where TValue : TValueRevealBase
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealerBase, TVRevealerBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealerBase, TVRevealerBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : notnull, TKRevealerBase
    where TValue : TVRevealerBase
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>, ISupportsSubsetDisplayKeys<TKSelectDerived>
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

public abstract class SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TValueRevealBase> 
    : FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TValueRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull
    where TValue : TValueRevealBase
    where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealerBase, TVRevealerBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealerBase, TVRevealerBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull, TKRevealerBase
    where TValue : TVRevealerBase
    where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }
}
