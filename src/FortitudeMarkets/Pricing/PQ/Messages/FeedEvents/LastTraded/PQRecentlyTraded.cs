// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[Flags]
public enum PQLastTradedListUpdatedFlags : byte
{
    None             = 0x00
  , DuringPeriodFlag = 0x01
}

public interface IPQRecentlyTraded : IPQLastTradedList, IMutableRecentlyTraded, ITrackableReset<IPQRecentlyTraded>
{
    new IPQLastTrade? this[int index] { get; set; }
    new IPQRecentlyTraded Clone();



    bool IsDuringPeriodUpdated { get; set; }

    new IPQRecentlyTraded ResetWithTracking();
}

public class PQRecentlyTraded : PQLastTradedList, IPQRecentlyTraded
{
    protected PQLastTradedListUpdatedFlags UpdatedFlags;

    protected int ShiftPreviousLastTrades;

    public PQRecentlyTraded() { }
    public PQRecentlyTraded(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) { }
    public PQRecentlyTraded(IEnumerable<IPQLastTrade?> lastTrades) : base(lastTrades) { }
    public PQRecentlyTraded(IList<IPQLastTrade?> lastTrades) : base(lastTrades) { }
    public PQRecentlyTraded(IRecentlyTraded toClone) : base(toClone) { }
    public PQRecentlyTraded(IPQRecentlyTraded toClone) : this((IRecentlyTraded)toClone) { }
    public PQRecentlyTraded(PQRecentlyTraded toClone) : this((IRecentlyTraded)toClone) { }

    IMutableRecentlyTraded ITrackableReset<IMutableRecentlyTraded>.ResetWithTracking() => ResetWithTracking();

    IMutableRecentlyTraded IMutableRecentlyTraded.ResetWithTracking() => ResetWithTracking();

    IPQRecentlyTraded ITrackableReset<IPQRecentlyTraded>.ResetWithTracking() => ResetWithTracking();

    IPQRecentlyTraded IPQRecentlyTraded.ResetWithTracking() => ResetWithTracking();

    public override PQRecentlyTraded ResetWithTracking()
    {
        base.ResetWithTracking();
        return this;
    }

    bool IInterfacesComparable<IRecentlyTraded>.AreEquivalent(IRecentlyTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public TimeBoundaryPeriod DuringPeriod { get; set; }

    public bool IsDuringPeriodUpdated
    {
        get => (UpdatedFlags & PQLastTradedListUpdatedFlags.DuringPeriodFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQLastTradedListUpdatedFlags.DuringPeriodFlag;

            else if (IsDuringPeriodUpdated) UpdatedFlags ^= PQLastTradedListUpdatedFlags.DuringPeriodFlag;
        }
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        foreach (var lastTradeUpdates in base.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
        {
            yield return lastTradeUpdates;
        }

        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;

        if (!updatedOnly || IsDuringPeriodUpdated)
        {
            yield return new PQFieldUpdate(PQFeedFields.LastTradedRecentlyByPeriod, PQTradingSubFieldKeys.LastTradedSummaryPeriod, (uint)DuringPeriod);
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate is { Id: PQFeedFields.LastTradedRecentlyByPeriod, TradingSubId: PQTradingSubFieldKeys.LastTradedSummaryPeriod })
        {
            IsDuringPeriodUpdated = true;
            DuringPeriod          = (TimeBoundaryPeriod)pqFieldUpdate.Payload;
        }

        return base.UpdateField(pqFieldUpdate);
    }

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
