// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public class TradingStateEvent : ReusableObject<ITradingStateEvent>, IMutableTradingStateEvent
{
    public TradingStateEvent() { }

    public TradingStateEvent(TradingEventType tradingEvent = TradingEventType.None, DateTime? startedAtTime = null
      , uint eventSequenceId = 0, int estimatedLengthSeconds = 0)
    {
        TradingEvent           = tradingEvent;
        StartedAtTime          = startedAtTime ?? DateTime.Now;
        EstimatedLengthSeconds = estimatedLengthSeconds;
        EventSequenceId        = eventSequenceId;
    }

    public TradingStateEvent(ITradingStateEvent toClone)
    {
        TradingEvent           = toClone.TradingEvent;
        StartedAtTime          = toClone.StartedAtTime;
        EstimatedLengthSeconds = toClone.EstimatedLengthSeconds;
        EventSequenceId        = toClone.EventSequenceId;
    }

    public TradingEventType TradingEvent { get; set; }

    public uint EventSequenceId { get; set; }

    public DateTime StartedAtTime { get; set; }

    public int EstimatedLengthSeconds { get; set; }

    IMutableTradingStateEvent ICloneable<IMutableTradingStateEvent>.Clone() => Clone();

    IMutableTradingStateEvent IMutableTradingStateEvent.Clone() => Clone();

    public override TradingStateEvent Clone() =>
        Recycler?.Borrow<TradingStateEvent>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new TradingStateEvent(this);

    public override TradingStateEvent CopyFrom(ITradingStateEvent source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        TradingEvent           = source.TradingEvent;
        StartedAtTime          = source.StartedAtTime;
        EstimatedLengthSeconds = source.EstimatedLengthSeconds;
        EventSequenceId        = source.EventSequenceId;

        return this;
    }

    public bool AreEquivalent(ITradingStateEvent? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var tradingEventSame = TradingEvent == other.TradingEvent;
        var startedAtSame    = StartedAtTime == other.StartedAtTime;
        var estimatedLenSame = EstimatedLengthSeconds == other.EstimatedLengthSeconds;
        var eventSeqIdSame   = EventSequenceId == other.EventSequenceId;

        var allAreSame = tradingEventSame && startedAtSame && estimatedLenSame && eventSeqIdSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITradingStateEvent, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)TradingEvent;
            hashCode = (hashCode * 397) ^ (int)EventSequenceId;
            hashCode = (hashCode * 397) ^ StartedAtTime.GetHashCode();
            hashCode = (hashCode * 397) ^ EstimatedLengthSeconds;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(TradingStateEvent)}{{{nameof(TradingEvent)}: {TradingEvent}, {nameof(EventSequenceId)}: {EventSequenceId}, " +
        $"{nameof(StartedAtTime)}: {StartedAtTime}, {nameof(EstimatedLengthSeconds)}: {EstimatedLengthSeconds}}}";
}
