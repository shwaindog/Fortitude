using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;


public interface IPQOnTickLastTraded : IPQLastTradedList, IMutableOnTickLastTraded
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQOnTickLastTraded       Clone();
    new IEnumerator<IPQLastTrade> GetEnumerator();
}

public class PQOnTickLastTraded: PQLastTradedList, IPQOnTickLastTraded
{
    public PQOnTickLastTraded() { }
    public PQOnTickLastTraded(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) { }
    public PQOnTickLastTraded(IEnumerable<IPQLastTrade?> lastTrades) : base(lastTrades) { }
    public PQOnTickLastTraded(IList<IPQLastTrade?> lastTrades) : base(lastTrades) { }
    public PQOnTickLastTraded(IOnTickLastTraded toClone) : base(toClone) { }
    public PQOnTickLastTraded(IPQOnTickLastTraded toClone) : this((IOnTickLastTraded)toClone) { }
    public PQOnTickLastTraded(PQOnTickLastTraded toClone) : this((IOnTickLastTraded)toClone) { }

    bool IInterfacesComparable<IOnTickLastTraded>.AreEquivalent(IOnTickLastTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);


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

    public override string ToString() => $"{nameof(PQOnTickLastTraded)}{{{PQLastTradedListToStringMembers}}}";
}