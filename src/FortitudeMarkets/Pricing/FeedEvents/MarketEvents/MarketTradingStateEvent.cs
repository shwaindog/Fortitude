// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public class MarketTradingStateEvent : ReusableObject<IMarketTradingStateEvent>, IMutableMarketTradingStateEvent
{
    public MarketTradingStateEvent() { }

    public MarketTradingStateEvent
    (MarketTradingStatusType marketTradingStatus = MarketTradingStatusType.None, DateTime? startedAtTime = null
      , uint eventSequenceId = 0, int estimatedLengthSeconds = 0, DateTime? endedAt = null)
    {
        MarketTradingStatus           = marketTradingStatus;
        StartTime          = startedAtTime ?? DateTime.Now;
        EstimatedLengthMs = estimatedLengthSeconds;
        EventSequenceId        = eventSequenceId;
    }

    public MarketTradingStateEvent(IMarketTradingStateEvent toClone)
    {
        MarketTradingStatus           = toClone.MarketTradingStatus;
        StartTime          = toClone.StartTime;
        EstimatedLengthMs = toClone.EstimatedLengthMs;
        EventSequenceId        = toClone.EventSequenceId;
    }

    public MarketTradingStatusType MarketTradingStatus { get; set; }

    public uint EventSequenceId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndedAt { get; set; }

    public int EstimatedLengthMs { get; set; }

    public bool IsEmpty
    {
        get =>
            MarketTradingStatus == MarketTradingStatusType.None
         && EventSequenceId == 0
         && StartTime == DateTime.MinValue
         && EstimatedLengthMs == 0;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    public IMutableMarketTradingStateEvent ResetWithTracking()
    {
        MarketTradingStatus           = MarketTradingStatusType.None;
        EventSequenceId        = 0;
        StartTime          = DateTime.MinValue;
        EstimatedLengthMs = 0;

        return this;
    }

    IMutableMarketTradingStateEvent ICloneable<IMutableMarketTradingStateEvent>.Clone() => Clone();

    IMutableMarketTradingStateEvent IMutableMarketTradingStateEvent.Clone() => Clone();

    public override MarketTradingStateEvent Clone() =>
        Recycler?.Borrow<MarketTradingStateEvent>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new MarketTradingStateEvent(this);

    public override MarketTradingStateEvent CopyFrom(IMarketTradingStateEvent source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        MarketTradingStatus           = source.MarketTradingStatus;
        StartTime          = source.StartTime;
        EstimatedLengthMs = source.EstimatedLengthMs;
        EventSequenceId        = source.EventSequenceId;

        return this;
    }

    public bool AreEquivalent(IMarketTradingStateEvent? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var tradingEventSame = MarketTradingStatus == other.MarketTradingStatus;
        var startedAtSame    = StartTime == other.StartTime;
        var estimatedLenSame = EstimatedLengthMs == other.EstimatedLengthMs;
        var eventSeqIdSame   = EventSequenceId == other.EventSequenceId;

        var allAreSame = tradingEventSame && startedAtSame && estimatedLenSame && eventSeqIdSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMarketTradingStateEvent, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)MarketTradingStatus;
            hashCode = (hashCode * 397) ^ (int)EventSequenceId;
            hashCode = (hashCode * 397) ^ StartTime.GetHashCode();
            hashCode = (hashCode * 397) ^ EstimatedLengthMs;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(MarketTradingStateEvent)}{{{nameof(MarketTradingStatus)}: {MarketTradingStatus}, {nameof(EventSequenceId)}: {EventSequenceId}, " +
        $"{nameof(StartTime)}: {StartTime}, {nameof(EstimatedLengthMs)}: {EstimatedLengthMs}, {nameof(EndedAt)}: {EndedAt}}}";
}
