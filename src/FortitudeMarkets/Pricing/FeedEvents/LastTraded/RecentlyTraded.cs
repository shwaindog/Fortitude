// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class RecentlyTraded : LastTradedList, IMutableRecentlyTraded
{
    private TracksReorderingListRegistry<IMutableLastTrade, ILastTrade>? elementShiftRegistry;

    public RecentlyTraded() => elementShiftRegistry = new TracksReorderingListRegistry<IMutableLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);

    public RecentlyTraded(LastTradedTransmissionFlags transmissionFlags, ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount)
    {
        elementShiftRegistry = new TracksReorderingListRegistry<IMutableLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;
    }

    public RecentlyTraded
    (LastTradedTransmissionFlags transmissionFlags, IEnumerable<ILastTrade> lastTrades
      , ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(lastTrades)
    {
        elementShiftRegistry = new TracksReorderingListRegistry<IMutableLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;
    }

    public RecentlyTraded(IRecentlyTraded toClone) : base(toClone)
    {
        elementShiftRegistry = new TracksReorderingListRegistry<IMutableLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        TransferFlags        = toClone.TransferFlags;
        MaxAllowedSize       = toClone.MaxAllowedSize;
    }

    public RecentlyTraded
    (ISourceTickerInfo sourceTickerInfo, LastTradedTransmissionFlags transmissionFlags
      , ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(sourceTickerInfo)
    {
        elementShiftRegistry = new TracksReorderingListRegistry<IMutableLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;
    }

    protected static Func<ILastTrade, ILastTrade, bool> SameTradeId = (lhs, rhs) => lhs.TradeId == rhs.TradeId; 

    public TimeBoundaryPeriod DuringPeriod { get; set; }

    IMutableRecentlyTraded ITrackableReset<IMutableRecentlyTraded>.ResetWithTracking() => ResetWithTracking();

    IMutableRecentlyTraded IMutableRecentlyTraded.ResetWithTracking() => ResetWithTracking();

    public override RecentlyTraded ResetWithTracking()
    {
        DuringPeriod = TimeBoundaryPeriod.Tick;
        elementShiftRegistry!.ResetWithTracking();
        base.ResetWithTracking();
        return this;
    }

    public LastTradedTransmissionFlags TransferFlags { get; set; } = IRecentlyTradedHistory.DefaultAlertTradesTransmissionFlags;

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementShiftRegistry!.ShiftCommands;
        set => elementShiftRegistry!.ShiftCommands = value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementShiftRegistry!.ClearRemainingElementsFromIndex;
        set => elementShiftRegistry!.ClearRemainingElementsFromIndex = (ushort?)value;
    }

    public bool HasRandomAccessUpdates
    {
        get => elementShiftRegistry!.HasRandomAccessUpdates;
        set => elementShiftRegistry!.HasRandomAccessUpdates = value;
    }

    public DateTime UpdateTime
    {
        get => elementShiftRegistry!.UpdateTime;
        set => elementShiftRegistry!.UpdateTime = value;
    }

    public ushort MaxAllowedSize
    {
        get => elementShiftRegistry!.MaxAllowedSize;
        set => elementShiftRegistry!.MaxAllowedSize = value;
    }

    IMutableLastTrade IMutableTracksShiftsList<IMutableLastTrade, ILastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = value;
    }

    IMutableLastTrade IMutableCachedRecentCountHistory<IMutableLastTrade, ILastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = value;
    }

    IMutableLastTrade IList<IMutableLastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = value;
    }

    IMutableLastTrade IMutableLastTradedList.this[int index]
    {
        get => this[index];
        set => this[index] = value;
    }

    ILastTrade IReadOnlyList<ILastTrade>.this[int index] => this[index];

    ILastTrade IRecentlyTraded.this[int index] => this[index];

    IMutableLastTrade IMutableRecentlyTraded.this[int index]
    {
        get => this[index];
        set => this[index] = value;
    }

    // more recent trades appear at start
    // find the shift of the first entry in previous collection in the updated collection
    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<ILastTrade> updatedCollection) =>
        elementShiftRegistry!.CalculateShift(asAtTime, updatedCollection);


    public ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd) => 
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IMutableTracksShiftsList<IMutableLastTrade, ILastTrade>.ShiftElements(int byElements) => 
        elementShiftRegistry!.ShiftElements(byElements);

    public void ClearShiftCommands() => elementShiftRegistry!.ClearShiftCommands();

    public ListShiftCommand ShiftElementsFrom
        (int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry!.ShiftElementsFrom(byElements, pinElementsFromIndex);

    public ListShiftCommand ShiftElementsUntil
        (int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry!.ShiftElementsUntil(byElements, pinElementsFromIndex);

    public ListShiftCommand ApplyElementShift(ListShiftCommand shiftCommandToApply) => elementShiftRegistry!.ApplyElementShift(shiftCommandToApply);

    public ListShiftCommand InsertAtStart(IMutableLastTrade toInsertAtStart) => elementShiftRegistry!.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IMutableLastTrade toAppendAtEnd) => elementShiftRegistry!.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IMutableLastTrade toInsertAtStart) => elementShiftRegistry!.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementShiftRegistry!.DeleteAt(index);

    public ListShiftCommand Delete(IMutableLastTrade toDelete) => elementShiftRegistry!.Delete(toDelete);

    public ListShiftCommand ClearAll() => elementShiftRegistry!.ClearAll();

    public ListShiftCommand MoveToStart(IMutableLastTrade existingItem) => elementShiftRegistry!.MoveToStart((IPQLastTrade)existingItem);

    public ListShiftCommand MoveToStart(int indexToMoveToStart) =>
        elementShiftRegistry!.MoveToStart(indexToMoveToStart);

    public ListShiftCommand MoveToEnd (int indexToMoveToEnd) =>
        elementShiftRegistry!.MoveToEnd(indexToMoveToEnd);

    public ListShiftCommand MoveSingleElementBy
        (int indexToMoveToEnd, int shift) =>
        elementShiftRegistry!.MoveSingleElementBy(indexToMoveToEnd, shift);

    public ListShiftCommand MoveSingleElementBy(IMutableLastTrade existingItem, int shift) => 
        elementShiftRegistry!.MoveSingleElementBy((IPQLastTrade)existingItem, shift);

    public ListShiftCommand MoveToEnd(IMutableLastTrade existingItem) =>
        elementShiftRegistry!.MoveToEnd((IPQLastTrade)existingItem);

    public bool IsEmpty
    {
        get => LastTrades.All(lt => lt.IsEmpty);
        set
        {
            foreach (var lastTrade in LastTrades)
            {
                lastTrade.IsEmpty = value;
            }
        }
    }

    IRecentlyTraded IRecentlyTraded.Clone() => Clone();

    IMutableRecentlyTraded IMutableRecentlyTraded.Clone() => Clone();

    public override RecentlyTraded CopyFrom
        (ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        elementShiftRegistry ??= new TracksReorderingListRegistry<IMutableLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        base.CopyFrom(source, copyMergeFlags);
        if (source is IRecentlyTraded recentlyTraded)
        {
            DuringPeriod = recentlyTraded.DuringPeriod;
        }
        return this;
    }

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    public override RecentlyTraded Clone() => Recycler?.Borrow<RecentlyTraded>().CopyFrom(this) ?? new RecentlyTraded(this);

    bool IInterfacesComparable<IRecentlyTraded>.AreEquivalent(IRecentlyTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public override bool AreEquivalent(ILastTradedList? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);

        return baseSame;
    }

    public override string ToString() => $"{nameof(RecentlyTraded)}{{{LastTradedListToStringMembers}, {nameof(DuringPeriod)}: {DuringPeriod}}}";
}
