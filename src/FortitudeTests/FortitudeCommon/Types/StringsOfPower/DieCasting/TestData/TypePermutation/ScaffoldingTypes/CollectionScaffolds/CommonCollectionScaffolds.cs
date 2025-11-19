// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.CollectionScaffolds;



public abstract class CollectionMoldScaffold<TValue, TCollection> : CollectionFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue>
    where TCollection : IEnumerable<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class EnumeratorMoldScaffold<TValue, TCollection> : EnumeratorFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue> where TCollection : IEnumerator<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FormattedCollectionMoldScaffold<TValue, TCollection> : FormattedCollectionFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue>
    where TCollection : IEnumerable<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FormattedEnumeratorMoldScaffold<TValue, TCollection> : FormattedEnumeratorFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue> where TCollection : IEnumerator<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class RevealerCollectionMoldScaffold<TCloaked, TRevealBase ,TCollection> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase , TCollection>
  , IEnumerable<TCloaked>
    where TCollection : IEnumerable<TCloaked>
    where TRevealBase : notnull
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TCloaked>().GetEnumerator();
}

public abstract class RevealerEnumeratorMoldScaffold<TCloaked, TRevealBase, TCollection> : 
    RevealerEnumeratorFieldMoldScaffold<TCloaked, TRevealBase, TCollection>, IEnumerable<TCloaked>
    where TCollection : IEnumerator<TCloaked>
    where TRevealBase : notnull
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => Value ?? Enumerable.Empty<TCloaked>().GetEnumerator();
}

public abstract class FilteredCollectionMoldScaffold<TValue, TCollection> : FilteredCollectionFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue> where TCollection : IEnumerable<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FilteredCollectionMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    FilteredCollectionFieldMoldScaffold<TValue, TValueFilterBase, TCollection>, IEnumerable<TValue>
    where TValue : TValueFilterBase
    where TCollection : IEnumerable<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FilteredEnumeratorMoldScaffold<TValue, TCollection> : FilteredEnumeratorFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue>
    where TCollection : IEnumerator<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FilteredEnumeratorMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    FilteredEnumeratorFieldMoldScaffold<TValue, TCollection>, IEnumerable<TValue>
    where TValue : TValueFilterBase
    where TCollection : IEnumerator<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FormattedFilteredCollectionMoldScaffold<TValue, TCollection> : FormattedFilteredCollectionFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue> where TCollection : IEnumerable<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FormattedFilteredCollectionMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    FormattedFilteredCollectionFieldMoldScaffold<TValue, TValueFilterBase, TCollection>, IEnumerable<TValue>
    where TValue : TValueFilterBase
    where TCollection : IEnumerable<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FormattedFilteredEnumeratorMoldScaffold<TValue, TCollection> : FormattedFilteredEnumeratorFieldMoldScaffold<TValue, TCollection>
  , IEnumerable<TValue>
    where TCollection : IEnumerator<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class FormattedFilteredEnumeratorMoldScaffold<TValue, TValueFilterBase, TCollection> : 
    FormattedFilteredEnumeratorFieldMoldScaffold<TValue, TCollection>, IEnumerable<TValue>
    where TValue : TValueFilterBase
    where TCollection : IEnumerator<TValue>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => Value ?? Enumerable.Empty<TValue>().GetEnumerator();
}

public abstract class RevealerFilteredCollectionMoldScaffold<TCloaked, TCollection> 
    : FormattedFilteredCollectionFieldMoldScaffold<TCloaked, TCloaked, TCollection>
      , IEnumerable<TCloaked> where TCollection : IEnumerable<TCloaked>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TCloaked>().GetEnumerator();
}

public abstract class RevealerFilteredCollectionMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCollection> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCollection>, IEnumerable<TCloaked>
    where TCloaked : TCloakedFilterBase?
    where TCollection : IEnumerable<TCloaked>
    where TRevealBase : notnull
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => Value?.GetEnumerator() ?? Enumerable.Empty<TCloaked>().GetEnumerator();
}

public abstract class RevealerFilteredEnumeratorMoldScaffold<TCloaked, TCollection> : 
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TCollection>, IEnumerable<TCloaked>
    where TCollection : IEnumerator<TCloaked>
    where TCloaked : notnull
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => Value ?? Enumerable.Empty<TCloaked>().GetEnumerator();
}

public abstract class RevealerFilteredEnumeratorMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCollection> : 
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCollection>, IEnumerable<TCloaked>
    where TCloaked : TCloakedFilterBase
    where TCollection : IEnumerator<TCloaked>
    where TRevealBase : notnull
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<TCloaked> GetEnumerator() => Value ?? Enumerable.Empty<TCloaked>().GetEnumerator();
}