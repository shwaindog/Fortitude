// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.KeyedCollectionFields;

public abstract class FormattedKeyValueFieldMoldScaffold<TKey, TValue> : FormattedMoldScaffold<Dictionary<TKey, TValue?>>, ISupportsKeyFormatString
    where TKey : notnull
{
    public string? KeyFormatString { get; set; }
}

public abstract class FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase> : 
    MoldScaffoldBase<Dictionary<TKey, TValue?>>, ISupportsKeyFormatString, ISupportsValueRevealer<TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public Delegate ValueRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer
    {
        get => (PalantírReveal<TVRevealBase>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
    public string? KeyFormatString { get; set; }
}

public abstract class KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase> : 
    MoldScaffoldBase<Dictionary<TKey, TValue?>>, ISupportsKeyRevealer<TKRevealBase>, ISupportsValueRevealer<TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public Delegate KeyRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TKRevealBase> KeyRevealer
    {
        get => (PalantírReveal<TKRevealBase>)KeyRevealerDelegate;
        set => KeyRevealerDelegate = value;
    }
    
    public Delegate ValueRevealerDelegate { get; set; } = null!;
    
    public PalantírReveal<TVRevealBase> ValueRevealer
    {
        get => (PalantírReveal<TVRevealBase>)ValueRevealerDelegate;
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

public abstract class FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> 
    : FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
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

public abstract class SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    : FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull
    where TValue : TVRevealBase
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }
}

public abstract class SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKSelectDerived : TKey
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
