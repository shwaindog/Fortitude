// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField.FixtureScaffolding;


public abstract class CollectionFieldMoldScaffold<TValue, TCollection> : MoldScaffoldBase<TCollection?>
    where TCollection : IEnumerable<TValue> { }

public abstract class EnumeratorFieldMoldScaffold<TValue, TCollection> : MoldScaffoldBase<TCollection?>
    where TCollection : IEnumerator<TValue> { }

public abstract class FormattedCollectionFieldMoldScaffold<TValue, TCollection> : FormattedMoldScaffold<TCollection?>
    where TCollection : IEnumerable<TValue> { }

public abstract class FormattedEnumeratorFieldMoldScaffold<TValue, TCollection> : FormattedMoldScaffold<TCollection?> 
    where TCollection : IEnumerator<TValue>
{ }

public abstract class RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCollection> 
    : FormattedMoldScaffold<TCollection>, ISupportsValueRevealer<TRevealBase>
    where TCollection : IEnumerable<TCloaked>?
    where TRevealBase : notnull
{
    public Delegate ValueRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TRevealBase> ValueRevealer
    {
        get => (PalantírReveal<TRevealBase>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
}

public abstract class RevealerEnumeratorFieldMoldScaffold<TCloaked, TRevealBase, TCollection> : 
    MoldScaffoldBase<TCollection>
  , ISupportsValueRevealer<TRevealBase>
    where TCollection : IEnumerator<TCloaked>?
    where TRevealBase : notnull
{
    public Delegate ValueRevealerDelegate { get; set; } = null!;

    public PalantírReveal<TRevealBase> ValueRevealer
    {
        get => (PalantírReveal<TRevealBase>)ValueRevealerDelegate;
        set => ValueRevealerDelegate = value;
    }
}

public abstract class FilteredCollectionFieldMoldScaffold<TValue, TCollection> 
    : CollectionFieldMoldScaffold<TValue, TCollection>, ISupportsOrderedCollectionPredicate<TValue> where TCollection : IEnumerable<TValue>
{
    public OrderedCollectionPredicate<TValue> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TValue>.GetNoFilterPredicate;
}

public abstract class FilteredCollectionFieldMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    CollectionFieldMoldScaffold<TValue, TCollection>, ISupportsOrderedCollectionPredicate<TValueFilterBase>
    where TValue : TValueFilterBase
    where TCollection : IEnumerable<TValue>
{
    public OrderedCollectionPredicate<TValueFilterBase> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TValueFilterBase>.GetNoFilterPredicate;
}


public abstract class FormattedFilteredCollectionFieldMoldScaffold<TValue, TCollection> 
    : FormattedCollectionFieldMoldScaffold<TValue, TCollection>
      , ISupportsOrderedCollectionPredicate<TValue> where TCollection : IEnumerable<TValue>
{
    
    public OrderedCollectionPredicate<TValue> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TValue>.GetNoFilterPredicate;
}

public abstract class FormattedFilteredCollectionFieldMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    FormattedCollectionFieldMoldScaffold<TValue, TCollection>, ISupportsOrderedCollectionPredicate<TValueFilterBase>
    where TValue : TValueFilterBase?
    where TCollection : IEnumerable<TValue>
{
    
    public OrderedCollectionPredicate<TValueFilterBase> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TValueFilterBase>.GetNoFilterPredicate;
}

public abstract class RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCollection> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCollection>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedFilterBase?
    where TCollection : IEnumerable<TCloaked>?
    where TRevealBase : notnull
{
    
    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;
}

public abstract class RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCollection> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TCloaked, TCollection>, ISupportsOrderedCollectionPredicate<TCloaked>
    where TCollection : IEnumerable<TCloaked>
    where TCloaked : notnull
{
    
    public OrderedCollectionPredicate<TCloaked> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TCloaked>.GetNoFilterPredicate;
}

public abstract class FilteredEnumeratorFieldMoldScaffold<TValue, TCollection> 
    : EnumeratorFieldMoldScaffold<TValue, TCollection>
      , ISupportsOrderedCollectionPredicate<TValue> where TCollection : IEnumerator<TValue>
{
    public OrderedCollectionPredicate<TValue> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TValue>.GetNoFilterPredicate;
}

public abstract class FilteredEnumeratorFieldMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    EnumeratorFieldMoldScaffold<TValue, TCollection>, ISupportsOrderedCollectionPredicate<TValueFilterBase>
    where TValue : TValueFilterBase
    where TCollection : IEnumerator<TValue>
{
    public OrderedCollectionPredicate<TValueFilterBase> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TValueFilterBase>.GetNoFilterPredicate;
}

public abstract class FormattedFilteredEnumeratorFieldMoldScaffold<TValue, TCollection> 
    : FormattedEnumeratorFieldMoldScaffold<TValue, TCollection>
      , ISupportsOrderedCollectionPredicate<TValue> where TCollection : IEnumerator<TValue>
{
    public OrderedCollectionPredicate<TValue> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TValue>.GetNoFilterPredicate;
}

public abstract class FormattedFilteredEnumeratorFieldMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    FormattedEnumeratorFieldMoldScaffold<TValue, TCollection>, ISupportsOrderedCollectionPredicate<TValueFilterBase>
    where TValue : TValueFilterBase
    where TCollection : IEnumerator<TValue>
{
    public OrderedCollectionPredicate<TValueFilterBase> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TValueFilterBase>.GetNoFilterPredicate;
}

public abstract class RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TCollection> : 
    RevealerEnumeratorFieldMoldScaffold<TCloaked, TCloaked, TCollection>, ISupportsOrderedCollectionPredicate<TCloaked>
    where TCollection : IEnumerator<TCloaked>?
    where TCloaked : notnull
{
    public OrderedCollectionPredicate<TCloaked> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TCloaked>.GetNoFilterPredicate;
}

public abstract class RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCollection> : 
    RevealerEnumeratorFieldMoldScaffold<TCloaked, TRevealBase, TCollection>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedFilterBase
    where TCollection : IEnumerator<TCloaked>?
    where TRevealBase : notnull
{
    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; } = 
        ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;
}
