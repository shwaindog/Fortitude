// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

public abstract class FormattedKeyValueFieldMoldScaffold<TKey, TValue> : FormattedMoldScaffold<List<KeyValuePair<TKey, TValue>>?>, ISupportsKeyFormatString
    where TKey : notnull
{
    public string? KeyFormatString { get; set; }
}

public abstract class FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase> : 
    MoldScaffoldBase<List<KeyValuePair<TKey, TValue>>?>, ISupportsKeyFormatString, ISupportsValueRevealer<TVRevealBase>
    where TValue : TVRevealBase?
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

public abstract class FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue> : 
    MoldScaffoldBase<List<KeyValuePair<TKey, TValue?>>?>, ISupportsKeyFormatString, ISupportsValueRevealer<TValue>
    where TValue : struct
{
    public Delegate ValueRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TValue> ValueRevealer
    {
        get => (PalantírReveal<TValue>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
    public string? KeyFormatString { get; set; }
}

public abstract class KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase> : 
    MoldScaffoldBase<List<KeyValuePair<TKey, TValue>>?>, ISupportsKeyRevealer<TKRevealBase>, ISupportsValueRevealer<TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
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

public abstract class KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase> : 
    MoldScaffoldBase<List<KeyValuePair<TKey, TValue?>>?>, ISupportsKeyRevealer<TKRevealBase>, ISupportsValueRevealer<TValue>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public Delegate KeyRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TKRevealBase> KeyRevealer
    {
        get => (PalantírReveal<TKRevealBase>)KeyRevealerDelegate;
        set => KeyRevealerDelegate = value;
    }
    
    public Delegate ValueRevealerDelegate { get; set; } = null!;
    
    public PalantírReveal<TValue> ValueRevealer
    {
        get => (PalantírReveal<TValue>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
}

public abstract class StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue> : 
    MoldScaffoldBase<List<KeyValuePair<TKey?, TValue?>>?>, ISupportsKeyRevealer<TKey>, ISupportsValueRevealer<TValue>
    where TKey : struct
    where TValue : struct
{
    public Delegate KeyRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TKey> KeyRevealer
    {
        get => (PalantírReveal<TKey>)KeyRevealerDelegate;
        set => KeyRevealerDelegate = value;
    }
    
    public Delegate ValueRevealerDelegate { get; set; } = null!;
    
    public PalantírReveal<TValue> ValueRevealer
    {
        get => (PalantírReveal<TValue>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
}

public abstract class StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase> : 
    MoldScaffoldBase<List<KeyValuePair<TKey?, TValue>>?>, ISupportsKeyRevealer<TKey>, ISupportsValueRevealer<TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public Delegate KeyRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TKey> KeyRevealer
    {
        get => (PalantírReveal<TKey>)KeyRevealerDelegate;
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
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase> 
    : FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>, ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>
    where TValue : struct
{
    public KeyValuePredicate<TKFilterBase, TValue?> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>.GetNoFilterPredicate;
}


public abstract class FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;
}

public abstract class FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase> 
    : StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsKeyedCollectionPredicate<TKey?, TVFilterBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePredicate<TKey?, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKey?, TVFilterBase>.GetNoFilterPredicate;
}


public abstract class FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>, ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePredicate<TKFilterBase, TValue?> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TValue?>.GetNoFilterPredicate;
}

public abstract class FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue> 
    : StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>, ISupportsKeyedCollectionPredicate<TKey?, TValue?>
    where TKey : struct
    where TValue : struct
{
    public KeyValuePredicate<TKey?, TValue?> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKey?, TValue?>.GetNoFilterPredicate;
}

public abstract class SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList();
        set => displayKeys = value;
    }
}

public abstract class SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    : FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList();
        set => displayKeys = value;
    }
}

public abstract class SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    : FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList();
        set => displayKeys = value;
    }
}

public abstract class SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList();
        set => displayKeys = value;
    }
}

public abstract class SelectStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>, ISupportsSubsetDisplayKeys<TKey>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    private IReadOnlyList<TKey>? displayKeys;

    public IReadOnlyList<TKey>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKey)(object?)kvp.Key!).ToList();
        set => displayKeys = value;
    }
}

public abstract class SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>, ISupportsSubsetDisplayKeys<TKSelectDerived>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyList<TKSelectDerived>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKSelectDerived)(object?)kvp.Key!).ToList();
        set => displayKeys = value;
    }
}

public abstract class SelectStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>, ISupportsSubsetDisplayKeys<TKey>
    where TKey : struct
    where TValue : struct
{
    private IReadOnlyList<TKey>? displayKeys;

    public IReadOnlyList<TKey>? DisplayKeys
    {
        get => displayKeys ??= Value?.Select(kvp => (TKey)(object?)kvp.Key!).ToList();
        set => displayKeys = value;
    }
}
