// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.Utils;
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
  , IMutableExpiringCachedPeriodUpdateHistory<IPQRecentlyTraded, IPQLastTrade>
{
    new IPQLastTrade this[int index] { get; set; }

    new DateTime UpdateTime { get; set; }

    new IReadOnlyList<ElementShift> ElementShifts { get; set; }

    new ushort? ClearedElementsAfterIndex { get; set; }

    new bool    HasRandomAccessUpdates    { get; set; }

    new int CachedMaxCount { get; set; }

    new TimeBoundaryPeriod DuringPeriod { get; set; }

    new ElementShift ClearAll();

    new ElementShift DeleteAt(int index);

    new ElementShift ShiftElements(int byElements, int pinElementsFromIndex);

    new ElementShift ApplyElementShift(ElementShift shiftToApply);

    new IPQRecentlyTraded Clone();

    bool IsDuringPeriodUpdated { get; set; }

    new IPQRecentlyTraded ResetWithTracking();
}

public class PQRecentlyTraded : PQLastTradedList, IPQRecentlyTraded
{
    protected PQLastTradedListUpdatedFlags UpdatedFlags;

    private readonly ListElementShiftRegistry<IPQLastTrade> elementShiftRegistry;

    private TimeBoundaryPeriod duringPeriod;

    public PQRecentlyTraded() => elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);

    public PQRecentlyTraded(ISourceTickerInfo sourceTickerInfo) : base(sourceTickerInfo) =>
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);

    public PQRecentlyTraded(IEnumerable<IPQLastTrade> lastTrades) : base(lastTrades) =>
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);

    public PQRecentlyTraded(IList<IPQLastTrade> lastTrades) : base(lastTrades) =>
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);

    public PQRecentlyTraded(IRecentlyTraded toClone) : base(toClone) =>
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);

    public PQRecentlyTraded(IPQRecentlyTraded toClone) : this((IRecentlyTraded)toClone) =>
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);

    public PQRecentlyTraded(PQRecentlyTraded toClone) : this((IRecentlyTraded)toClone) =>
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);

    public IReadOnlyList<ElementShift> ElementShifts
    {
        get => elementShiftRegistry.ShiftCommands.AsReadOnly();
        set => elementShiftRegistry.ShiftCommands = (List<ElementShift>)value;
    }

    public ushort? ClearedElementsAfterIndex
    {
        get => elementShiftRegistry.ClearRemainingElementsAt;
        set => elementShiftRegistry.ClearRemainingElementsAt = value;
    }

    public bool HasRandomAccessUpdates
    {
        get => elementShiftRegistry.HasRandomAccessUpdates;
        set => elementShiftRegistry.HasRandomAccessUpdates = value;
    }

    public DateTime UpdateTime
    {
        get => elementShiftRegistry.UpdateTime;
        set => elementShiftRegistry.UpdateTime = value;
    }

    public int CachedMaxCount
    {
        get => elementShiftRegistry.CachedMaxCount;
        set => elementShiftRegistry.CachedMaxCount = value;
    }

    IMutableLastTrade IMutableSupportsElementsShift<IMutableRecentlyTraded, IMutableLastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    IMutableLastTrade IMutableCachedRecentCountHistory<IMutableRecentlyTraded, IMutableLastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    IMutableLastTrade ISupportsElementsShift<IMutableRecentlyTraded, IMutableLastTrade>.this[int index] => this[index];

    IMutableLastTrade IList<IMutableLastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    IMutableLastTrade IMutableLastTradedList.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    ILastTrade ISupportsElementsShift<IRecentlyTraded, ILastTrade>.this[int index] => this[index];

    ILastTrade IReadOnlyList<ILastTrade>.this[int index] => this[index];

    ILastTrade IRecentlyTraded.this[int index] => this[index];

    IMutableLastTrade IMutableRecentlyTraded.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    // more recent trades appear at start
    // find the shift of the first entry in previous collection in the updated collection
    public void CalculateShift
        (DateTime asAtTime, IReadOnlyList<IPQLastTrade> updatedCollection) =>
        elementShiftRegistry.CalculateShift(asAtTime, updatedCollection);

    public ElementShift ShiftElements
        (int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry.ShiftElements(byElements, pinElementsFromIndex);

    public ElementShift ApplyElementShift(ElementShift shiftToApply) => elementShiftRegistry.ApplyElementShift(shiftToApply);

    public ElementShift InsertAtStart(IPQLastTrade toInsertAtStart) => elementShiftRegistry.InsertAtStart(toInsertAtStart);

    ElementShift IMutableSupportsElementsShift<IMutableRecentlyTraded, IMutableLastTrade>.InsertAtStart(IMutableLastTrade toInsertAtStart) =>
        InsertAtStart((IPQLastTrade)toInsertAtStart);

    ElementShift IMutableSupportsElementsShift<IMutableRecentlyTraded, IMutableLastTrade>.InsertAt(int index, IMutableLastTrade toInsertAtStart) =>
        InsertAt(index, (IPQLastTrade)toInsertAtStart);

    void IMutableSupportsElementsShift<IMutableRecentlyTraded, IMutableLastTrade>.CalculateShift
        (DateTime asAtTime, IReadOnlyList<IMutableLastTrade> updatedCollection) =>
        CalculateShift(asAtTime, (IReadOnlyList<IPQLastTrade>)updatedCollection);

    ElementShift IMutableCachedRecentCountHistory<IMutableRecentlyTraded, IMutableLastTrade>.InsertAtStart(IMutableLastTrade toInsertAtStart) =>
        InsertAtStart((IPQLastTrade)toInsertAtStart);

    ElementShift IMutableCachedRecentCountHistory<IMutableRecentlyTraded, IMutableLastTrade>.InsertAt(int index, IMutableLastTrade toInsertAtStart) =>
        InsertAt(index, (IPQLastTrade)toInsertAtStart);

    public ElementShift InsertAt(int index, IPQLastTrade toInsertAtStart) => elementShiftRegistry.InsertAt(index, toInsertAtStart);

    public ElementShift DeleteAt(int index) => elementShiftRegistry.DeleteAt(index);

    public ElementShift ClearAll() => elementShiftRegistry.ClearAll();

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

    public TimeBoundaryPeriod DuringPeriod
    {
        get => duringPeriod;
        set
        {
            IsDuringPeriodUpdated |= value != duringPeriod || NumUpdates == 0;

            duringPeriod = value;
        }
    }

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
        foreach (var shiftCommand in elementShiftRegistry.ShiftCommands)
        {
            yield return new PQFieldUpdate(PQFeedFields.LastTradedRecentlyByPeriod, PQTradingSubFieldKeys.CommandElementsShift, (uint)shiftCommand);
        }

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
        if (pqFieldUpdate is { Id: PQFeedFields.LastTradedRecentlyByPeriod})
        {
            switch (pqFieldUpdate.TradingSubId)
            {
                case PQTradingSubFieldKeys.CommandElementsShift :
                    var elementShift          = (ElementShift)(pqFieldUpdate.Payload);
                    elementShiftRegistry.ShiftCommands.Append(elementShift);
                    ApplyElementShift(elementShift);
                break;
                    case PQTradingSubFieldKeys.LastTradedSummaryPeriod :
                    IsDuringPeriodUpdated = true;
                    DuringPeriod          = (TimeBoundaryPeriod)pqFieldUpdate.Payload;
                break;
            }
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
