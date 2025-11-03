using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public interface ITransferQuoteState
{
    ITransferQuoteState CopyFrom(ITransferQuoteState source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags);
}

public interface ITransferQuoteState<T> : ITransferQuoteState where T : class
{
    T CopyFrom(T source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public interface IParentQuoteElement
{
    QuoteInstantBehaviorFlags QuoteBehavior { get; }
}

public interface IChildQuoteElement
{
    IParentQuoteElement? Parent { get; }
}

public interface IMutableChildQuoteElement : IChildQuoteElement
{
    new IParentQuoteElement? Parent { get; set; }
}

public interface IReusableQuoteElement : IRecyclableObject, ITransferQuoteState, ICloneable { }

public interface IReusableQuoteElement<T> : IReusableQuoteElement, ITransferQuoteState<IReusableQuoteElement<T>>, ICloneable<T>, ITransferQuoteState<T>, IInterfacesComparable<T>
    where T : class { }

public abstract class ReusableQuoteElement<T> : RecyclableObject, IReusableQuoteElement<T> where T : class
{
    protected readonly int InstanceNum = InstanceCounter<T>.NextInstanceNum;

    ITransferQuoteState ITransferQuoteState.CopyFrom(ITransferQuoteState source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) => 
        (ITransferQuoteState)CopyFrom((T)source, behaviorFlags, copyMergeFlags);

    IReusableQuoteElement<T> ITransferQuoteState<IReusableQuoteElement<T>>.CopyFrom
        (IReusableQuoteElement<T> source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) =>
        (IReusableQuoteElement<T>)CopyFrom((T)source, behaviorFlags, copyMergeFlags);

    object ICloneable.Clone() => Clone();

    public abstract T Clone();

    public T CopyFrom(T source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => CopyFrom(source, QuoteInstantBehaviorFlags.DisableUpgradeLayer, copyMergeFlags);

    public abstract T CopyFrom(T source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    public abstract bool AreEquivalent(T? other, bool exactTypes = false);
}