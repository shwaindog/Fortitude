// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;


public interface IPQRecentlyTraded : IPQLastTradedList, IMutableRecentlyTraded
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQRecentlyTraded         Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();
}

public class PQRecentlyTraded : PQLastTradedList, IPQRecentlyTraded
{
    public PQRecentlyTraded() { }
    public PQRecentlyTraded(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) { }
    public PQRecentlyTraded(IEnumerable<IPQLastTrade?> lastTrades) : base(lastTrades) { }
    public PQRecentlyTraded(IList<IPQLastTrade?> lastTrades) : base(lastTrades) { }
    public PQRecentlyTraded(IRecentlyTraded toClone) : base(toClone) { }

    bool IInterfacesComparable<IRecentlyTraded>.AreEquivalent(IRecentlyTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public TimeBoundaryPeriod DuringPeriod                                                   { get; set; }

    IRecentlyTraded IRecentlyTraded.Clone() => Clone();

    IMutableRecentlyTraded IMutableRecentlyTraded.Clone() => Clone();

    IPQRecentlyTraded IPQRecentlyTraded.Clone() => Clone();

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    public override PQRecentlyTraded Clone() => 
        Recycler?.Borrow<PQRecentlyTraded>().CopyFrom(this) ??
        new PQRecentlyTraded((IRecentlyTraded)this);


    public override PQRecentlyTraded CopyFrom
        (ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IRecentlyTraded recentlyTraded)
        {
            DuringPeriod = recentlyTraded.DuringPeriod;
        }
        return this;
    }
    public override string ToString() => $"{nameof(PQRecentlyTraded)}{{{PQLastTradedListToStringMembers}, {nameof(DuringPeriod)}: {DuringPeriod}}}";
}
