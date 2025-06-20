﻿using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQOnTickLastTraded : IPQLastTradedList, IMutableOnTickLastTraded, ITrackableReset<IPQOnTickLastTraded>
{
    new IPQLastTrade this[int index] { get; set; }
    new IPQOnTickLastTraded       Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();

    new IPQOnTickLastTraded ResetWithTracking();
}

public class PQOnTickLastTraded : PQLastTradedList, IPQOnTickLastTraded
{
    public PQOnTickLastTraded() { }
    public PQOnTickLastTraded(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) { }

    public PQOnTickLastTraded(ISourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookup) 
        : base(sourceTickerInfo, nameIdLookup) { }

    public PQOnTickLastTraded(IPQNameIdLookupGenerator nameIdLookup) : base(nameIdLookup) { }
    public PQOnTickLastTraded(IEnumerable<IPQLastTrade> lastTrades) : base(lastTrades) { }
    public PQOnTickLastTraded(IList<IPQLastTrade> lastTrades) : base(lastTrades) { }

    public PQOnTickLastTraded(IOnTickLastTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null) 
        : base(toClone, nameIdLookup) { }

    public PQOnTickLastTraded(IPQOnTickLastTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null) 
        : this((IOnTickLastTraded)toClone, nameIdLookup) { }

    public PQOnTickLastTraded(PQOnTickLastTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null) 
        : this((IOnTickLastTraded)toClone, nameIdLookup) { }

    IMutableOnTickLastTraded ITrackableReset<IMutableOnTickLastTraded>.ResetWithTracking() => ResetWithTracking();

    IMutableOnTickLastTraded IMutableOnTickLastTraded.ResetWithTracking() => ResetWithTracking();

    IPQOnTickLastTraded ITrackableReset<IPQOnTickLastTraded>.ResetWithTracking() => ResetWithTracking();

    IPQOnTickLastTraded IPQOnTickLastTraded.ResetWithTracking() => ResetWithTracking();

    public override PQOnTickLastTraded ResetWithTracking()
    {
        base.ResetWithTracking();
        return this;
    }

    IOnTickLastTraded IOnTickLastTraded.Clone() => Clone();

    IMutableOnTickLastTraded IMutableOnTickLastTraded.Clone() => Clone();

    IPQOnTickLastTraded IPQOnTickLastTraded.Clone() => Clone();

    IOnTickLastTraded ICloneable<IOnTickLastTraded>.Clone() => Clone();

    IMutableOnTickLastTraded ICloneable<IMutableOnTickLastTraded>.Clone() => Clone();

    public override PQOnTickLastTraded Clone() =>
        Recycler?.Borrow<PQOnTickLastTraded>().CopyFrom(this) ??
        new PQOnTickLastTraded((IOnTickLastTraded)this);


    public override PQOnTickLastTraded CopyFrom
        (ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        return this;
    }

    bool IInterfacesComparable<IOnTickLastTraded>.AreEquivalent(IOnTickLastTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public override string ToString() => $"{nameof(PQOnTickLastTraded)}{{{PQNonLastTradedListToStringMembers}, {PQLastTradedListToString}}}";
}
