// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[Flags]
public enum PQLastTradedListUpdatedFlags : byte
{
    None                        = 0x00
  , DuringPeriodFlag            = 0x01
  , PeriodUpdateDateFlag        = 0x02
  , PeriodUpdateSub2MinTimeFlag = 0x04
}

public interface IPQRecentlyTraded : IPQLastTradedList, IMutableRecentlyTraded, ITrackableReset<IPQRecentlyTraded>
  , IMutableExpiringCachedPeriodUpdateHistory<IPQLastTrade, ILastTrade>
{
    new IPQLastTrade this[int index] { get; set; }

    new DateTime UpdateTime { get; set; }

    new IReadOnlyList<ListShiftCommand> ShiftCommands { get; set; }

    new int? ClearRemainingElementsFromIndex { get; set; }

    new bool HasUnreliableListTracking { get; set; }

    new ushort MaxAllowedSize { get; set; }

    new TimeBoundaryPeriod DuringPeriod { get; set; }

    new ListShiftCommand ClearAll();

    new ListShiftCommand DeleteAt(int index);

    new ListShiftCommand ShiftElements(int byElements);

    new ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex);

    new ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex);

    new ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply);

    new ListShiftCommand MoveSingleElementBy(IPQLastTrade existingItem, int shift);

    new ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift);

    new ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd);

    new void ClearShiftCommands();

    new ListShiftCommand MoveToEnd(int indexToMoveToEnd);

    new ListShiftCommand MoveToStart(int indexToMoveToStart);

    new bool CalculateShift(DateTime asAtTime, IReadOnlyList<ILastTrade> updatedCollection);

    new IPQRecentlyTraded Clone();

    bool IsDuringPeriodUpdated { get; set; }

    bool IsPeriodUpdateDateUpdated { get; set; }

    bool IsPeriodUpdateSub2MinTimeUpdated { get; set; }

    new IPQRecentlyTraded ResetWithTracking();
}

public class PQRecentlyTraded : PQLastTradedList, IPQRecentlyTraded
{
    protected PQLastTradedListUpdatedFlags UpdatedFlags;

    private TracksListReorderingRegistry<IPQLastTrade, ILastTrade>? elementShiftRegistry;

    private DateTime updateTime = DateTime.MinValue;

    private TimeBoundaryPeriod duringPeriod;

    public PQRecentlyTraded()
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);


        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(IPQNameIdLookupGenerator nameIdLookup, ListTransmissionFlags transmissionFlags
      , ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(nameIdLookup)
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(ListTransmissionFlags transmissionFlags, ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount)
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(ISourceTickerInfo sourceTickerInfo, ListTransmissionFlags transmissionFlags
      , ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(sourceTickerInfo)
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(ISourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookup, ListTransmissionFlags transmissionFlags
      , ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(sourceTickerInfo, nameIdLookup)
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(ListTransmissionFlags transmissionFlags, IEnumerable<IPQLastTrade> lastTrades
      , ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(lastTrades)
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(ListTransmissionFlags transmissionFlags, IList<IPQLastTrade> lastTrades
      , ushort maxAllowedSize = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(lastTrades)
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        MaxAllowedSize       = maxAllowedSize;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(IRecentlyTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null) : base(toClone, nameIdLookup)
    {
        elementShiftRegistry = new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);
        elementShiftRegistry.CopyFrom(toClone.ShiftCommands);

        TransferFlags  = toClone.TransferFlags;
        MaxAllowedSize = toClone.MaxAllowedSize;
        UpdateTime     = toClone.UpdateTime;
        DuringPeriod   = toClone.DuringPeriod;

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQRecentlyTraded)) SequenceId = 0;
    }

    public PQRecentlyTraded(IPQRecentlyTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null)
        : this((IRecentlyTraded)toClone, nameIdLookup) { }

    public PQRecentlyTraded(PQRecentlyTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null)
        : this((IRecentlyTraded)toClone, nameIdLookup) { }


    protected static Func<ILastTrade, ILastTrade, bool> SameTradeId = (lhs, rhs) => lhs.TradeId == rhs.TradeId;

    public ListTransmissionFlags TransferFlags { get; set; }

    public TimeBoundaryPeriod DuringPeriod
    {
        get => duringPeriod;
        set
        {
            IsDuringPeriodUpdated |= value != duringPeriod || SequenceId == 0;

            duringPeriod = value;
        }
    }

    public DateTime UpdateTime
    {
        get => updateTime;
        set
        {
            IsPeriodUpdateDateUpdated
                |= value.Get2MinIntervalsFromUnixEpoch() != updateTime.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsPeriodUpdateSub2MinTimeUpdated |= value.GetSub2MinComponent() != updateTime.GetSub2MinComponent() || SequenceId == 0;

            updateTime = value;
            if (elementShiftRegistry != null) elementShiftRegistry.UpdateTime = value;
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

    public bool IsPeriodUpdateDateUpdated
    {
        get => (UpdatedFlags & PQLastTradedListUpdatedFlags.PeriodUpdateDateFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQLastTradedListUpdatedFlags.PeriodUpdateDateFlag;

            else if (IsPeriodUpdateDateUpdated) UpdatedFlags ^= PQLastTradedListUpdatedFlags.PeriodUpdateDateFlag;
        }
    }

    public bool IsPeriodUpdateSub2MinTimeUpdated
    {
        get => (UpdatedFlags & PQLastTradedListUpdatedFlags.PeriodUpdateSub2MinTimeFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PQLastTradedListUpdatedFlags.PeriodUpdateSub2MinTimeFlag;

            else if (IsPeriodUpdateSub2MinTimeUpdated) UpdatedFlags ^= PQLastTradedListUpdatedFlags.PeriodUpdateSub2MinTimeFlag;
        }
    }

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementShiftRegistry!.ShiftCommands;
        set => elementShiftRegistry!.ShiftCommands = (List<ListShiftCommand>)value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementShiftRegistry!.ClearRemainingElementsFromIndex;
        set => elementShiftRegistry!.ClearRemainingElementsFromIndex = (ushort?)value;
    }

    public bool HasUnreliableListTracking
    {
        get => elementShiftRegistry!.HasUnreliableListTracking;
        set => elementShiftRegistry!.HasUnreliableListTracking = value;
    }

    IMutableLastTrade IMutableTracksShiftsList<IMutableLastTrade, ILastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    IMutableLastTrade IMutableCachedRecentCountHistory<IMutableLastTrade, ILastTrade>.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

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

    ILastTrade IReadOnlyList<ILastTrade>.this[int index] => this[index];

    ILastTrade IRecentlyTraded.this[int index] => this[index];

    IMutableLastTrade IMutableRecentlyTraded.this[int index]
    {
        get => this[index];
        set => this[index] = (IPQLastTrade)value;
    }

    // more recent trades appear at start
    // find the shift of the first entry in previous collection in the updated collection

    public void ReceiveEventLifeCycleChange(IMarketTradingStateEvent updatedItem, EventStateLifecycle eventType)
    {
        if (TransferFlags.HasResetOnNewTradingDayFlag() && eventType.IsActive() && updatedItem.MarketTradingStatus.HasMarketOpenFlag())
        {
            ClearAll();
        }
    }

    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<ILastTrade> updatedCollection)
    {
        UpdateTime = asAtTime;
        return elementShiftRegistry!.CalculateShift(asAtTime, updatedCollection);
    }

    ListShiftCommand IMutableTracksShiftsList<IMutableLastTrade, ILastTrade>.AppendShiftCommand(ListShiftCommand toAppendAtEnd) =>
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IMutableTracksShiftsList<IPQLastTrade, ILastTrade>.AppendShiftCommand(ListShiftCommand toAppendAtEnd) =>
        elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    ListShiftCommand IPQRecentlyTraded.AppendShiftCommand(ListShiftCommand toAppendAtEnd) => elementShiftRegistry!.AppendShiftCommand(toAppendAtEnd);

    public void ClearShiftCommands()
    {
        elementShiftRegistry!.ClearShiftCommands();
    }

    public ListShiftCommand ShiftElements(int byElements) => elementShiftRegistry!.ShiftElements(byElements);

    public ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex)
    {
        return elementShiftRegistry!.ShiftElementsFrom(byElements, pinElementsFromIndex);
    }

    public ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex)
    {
        return elementShiftRegistry!.ShiftElementsUntil(byElements, pinElementsFromIndex);
    }

    public ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply)
    {
        if (!IsEmpty)
        {
            return elementShiftRegistry!.ApplyListShiftCommand(shiftCommandToApply);
        }
        return new ListShiftCommand(0);
    }

    public ListShiftCommand InsertAtStart(IPQLastTrade toInsertAtStart) => elementShiftRegistry!.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IMutableLastTrade toAppendAtEnd) => AppendAtEnd((IPQLastTrade)toAppendAtEnd);

    public bool AppendAtEnd(IPQLastTrade toAppendAtEnd) => elementShiftRegistry!.AppendAtEnd(toAppendAtEnd);

    ListShiftCommand IMutableTracksShiftsList<IMutableLastTrade, ILastTrade>.InsertAtStart(IMutableLastTrade toInsertAtStart) =>
        InsertAtStart((IPQLastTrade)toInsertAtStart);

    ListShiftCommand IMutableTracksShiftsList<IMutableLastTrade, ILastTrade>.InsertAt
        (int index, IMutableLastTrade toInsertAtStart) =>
        InsertAt(index, (IPQLastTrade)toInsertAtStart);

    public ListShiftCommand InsertAt(int index, IPQLastTrade toInsertAtStart) => elementShiftRegistry!.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementShiftRegistry!.DeleteAt(index);

    ListShiftCommand IMutableTracksShiftsList<IMutableLastTrade, ILastTrade>.Delete
        (IMutableLastTrade toDelete) => Delete((IPQLastTrade)toDelete);

    public ListShiftCommand Delete(IPQLastTrade toDelete) => elementShiftRegistry!.Delete(toDelete);

    public ListShiftCommand ClearAll() => elementShiftRegistry!.ClearAll();

    ListShiftCommand IMutableTracksReorderingList<IMutableLastTrade, ILastTrade>.MoveToStart
        (IMutableLastTrade existingItem) =>
        elementShiftRegistry!.MoveToStart((IPQLastTrade)existingItem);

    ListShiftCommand IMutableTracksReorderingList<IMutableLastTrade, ILastTrade>.MoveSingleElementBy
        (int indexToMoveToEnd, int shift) =>
        elementShiftRegistry!.MoveSingleElementBy(indexToMoveToEnd, shift);

    ListShiftCommand IMutableTracksReorderingList<IMutableLastTrade, ILastTrade>.MoveSingleElementBy
        (IMutableLastTrade existingItem, int shift) =>
        elementShiftRegistry!.MoveSingleElementBy((IPQLastTrade)existingItem, shift);

    ListShiftCommand IMutableTracksReorderingList<IMutableLastTrade, ILastTrade>.MoveToEnd(IMutableLastTrade existingItem) =>
        elementShiftRegistry!.MoveToEnd((IPQLastTrade)existingItem);

    public ListShiftCommand MoveToStart(IPQLastTrade existingItem) => elementShiftRegistry!.MoveToStart(existingItem);

    public ListShiftCommand MoveToStart(int indexToMoveToStart) => elementShiftRegistry!.MoveToStart(indexToMoveToStart);

    public ListShiftCommand MoveToEnd(int indexToMoveToEnd) => elementShiftRegistry!.MoveToEnd(indexToMoveToEnd);

    public ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift) =>
        elementShiftRegistry!.MoveSingleElementBy(indexToMoveToEnd, shift);

    public ListShiftCommand MoveSingleElementBy(IPQLastTrade existingItem, int shift) =>
        elementShiftRegistry!.MoveSingleElementBy(existingItem, shift);

    public ListShiftCommand MoveToEnd(IPQLastTrade existingItem) => elementShiftRegistry!.MoveToEnd(existingItem);

    IMutableRecentlyTraded ITrackableReset<IMutableRecentlyTraded>.ResetWithTracking() => ResetWithTracking();

    IMutableRecentlyTraded IMutableRecentlyTraded.ResetWithTracking() => ResetWithTracking();

    IPQRecentlyTraded ITrackableReset<IPQRecentlyTraded>.ResetWithTracking() => ResetWithTracking();

    IPQRecentlyTraded IPQRecentlyTraded.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IMutableLastTrade> ITrackableReset<ITracksResetCappedCapacityList<IMutableLastTrade>>.ResetWithTracking() =>
        ResetWithTracking();

    IMutableLastTradedList IMutableLastTradedList.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IPQLastTrade> ITrackableReset<ITracksResetCappedCapacityList<IPQLastTrade>>.ResetWithTracking() =>
        ResetWithTracking();

    public override PQRecentlyTraded ResetWithTracking()
    {
        elementShiftRegistry!.ClearShiftCommands();
        base.ResetWithTracking();
        return this;
    }

    bool IInterfacesComparable<IRecentlyTraded>.AreEquivalent(IRecentlyTraded? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, PQMessageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var fullPicture = (messageFlags & PQMessageFlags.Complete) > 0;

        // Potentially only send this with a snapshot TBD

        foreach (var shiftCommand in elementShiftRegistry!.ShiftCommands)
        {
            yield return new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, PQTradingSubFieldKeys.CommandElementsShift
                                         , (uint)shiftCommand, (PQFieldFlags)shiftCommand);
        }

        foreach (var lastTradeUpdates in base.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
        {
            yield return lastTradeUpdates.WithFieldId(PQFeedFields.LastTradedAllRecentlyLimitedHistory);
        }

        if (fullPicture || IsDuringPeriodUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, PQTradingSubFieldKeys.LastTradedSummaryPeriod
                                         , (uint)DuringPeriod);

        if (fullPicture || IsPeriodUpdateDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, PQTradingSubFieldKeys.LastTradedPeriodUpdateDate
                                         , updateTime.Get2MinIntervalsFromUnixEpoch());
        if (fullPicture || IsPeriodUpdateSub2MinTimeUpdated)
        {
            var extended = updateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, PQTradingSubFieldKeys.LastTradedPeriodUpdateSub2MinTime
                                         , value, extended);
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate is { Id: PQFeedFields.LastTradedAllRecentlyLimitedHistory })
        {
            switch (pqFieldUpdate.TradingSubId)
            {
                case PQTradingSubFieldKeys.CommandElementsShift:
                    var elementShift = (ListShiftCommand)(pqFieldUpdate);
                    elementShiftRegistry!.AppendShiftCommand(elementShift);
                    ApplyListShiftCommand(elementShift);
                    break;
                case PQTradingSubFieldKeys.LastTradedSummaryPeriod:
                    IsDuringPeriodUpdated = true;
                    DuringPeriod          = (TimeBoundaryPeriod)pqFieldUpdate.Payload;
                    break;
                case PQTradingSubFieldKeys.LastTradedPeriodUpdateDate:
                    PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref updateTime, pqFieldUpdate.Payload);
                    IsPeriodUpdateDateUpdated = true;
                    if (updateTime == DateTime.UnixEpoch) updateTime = default;
                    return 0;
                case PQTradingSubFieldKeys.LastTradedPeriodUpdateSub2MinTime:
                    PQFieldConverters.UpdateSub2MinComponent(ref updateTime,
                                                             pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                    IsPeriodUpdateSub2MinTimeUpdated = true;
                    if (updateTime == DateTime.UnixEpoch) updateTime = default;
                    return 0;
            }
        }

        return base.UpdateField(pqFieldUpdate);
    }

    IRecentlyTraded IRecentlyTraded.Clone() => Clone();

    IMutableRecentlyTraded IMutableRecentlyTraded.Clone() => Clone();

    IPQRecentlyTraded IPQRecentlyTraded.Clone() => Clone();

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    public override PQRecentlyTraded Clone() =>
        Recycler?.Borrow<PQRecentlyTraded>().CopyFrom(this) ?? new PQRecentlyTraded((IRecentlyTraded)this);


    public override PQRecentlyTraded CopyFrom
        (ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var isFullReplace = copyMergeFlags.HasFullReplace();
        elementShiftRegistry ??= new TracksListReorderingRegistry<IPQLastTrade, ILastTrade>(this, NewElementFactory, SameTradeId);

        if (TransferFlags.HasOnlyWhenCopyMergeFlagsKeepCacheItemsFlag() && !copyMergeFlags.HasKeepCachedItems()) return this;
        elementShiftRegistry.CopyFrom(source, copyMergeFlags);
        base.CopyFrom(source, copyMergeFlags);
        if (source is IRecentlyTraded recentTraded)
        {
            TransferFlags = recentTraded.TransferFlags;
            UpdateTime    = recentTraded.UpdateTime;
            DuringPeriod  = recentTraded.DuringPeriod;

            if (isFullReplace) SetFlagsSame(recentTraded);
        }
        return this;
    }

    protected void SetFlagsSame(IRecentlyTraded toCopyFlags)
    {
        if (toCopyFlags is PQRecentlyTraded pqToClone)
        {
            UpdatedFlags = pqToClone.UpdatedFlags;
        }
    }

    protected string PQRecentlyTradedToStringMembers =>
        $"{PQNonLastTradedListToStringMembers}, {nameof(TransferFlags)}: {TransferFlags}, {nameof(UpdateTime)}: {UpdateTime:O}" +
        $", {nameof(DuringPeriod)}: {DuringPeriod}";


    protected string UpdateFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{nameof(PQRecentlyTraded)}{{{PQRecentlyTradedToStringMembers}, {UpdateFlagsToString}, {PQLastTradedListToString}}}";
}
