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
  , IMutableExpiringCachedPeriodUpdateHistory<IPQRecentlyTraded, IPQLastTrade>
{
    new IPQLastTrade this[int index] { get; set; }

    new DateTime UpdateTime { get; set; }

    new IReadOnlyList<ElementShift> ElementShifts { get; set; }

    new ushort? ClearedElementsAfterIndex { get; set; }

    new bool HasRandomAccessUpdates { get; set; }

    new int CachedMaxCount { get; set; }

    new TimeBoundaryPeriod DuringPeriod { get; set; }

    new ElementShift ClearAll();

    new ElementShift DeleteAt(int index);

    new ElementShift ShiftElements(int byElements, int pinElementsFromIndex);

    new ElementShift ApplyElementShift(ElementShift shiftToApply);

    new IPQRecentlyTraded Clone();

    bool IsDuringPeriodUpdated { get; set; }

    bool IsPeriodUpdateDateUpdated        { get; set; }
    bool IsPeriodUpdateSub2MinTimeUpdated { get; set; }

    new IPQRecentlyTraded ResetWithTracking();
}

public class PQRecentlyTraded : PQLastTradedList, IPQRecentlyTraded
{
    protected PQLastTradedListUpdatedFlags UpdatedFlags;

    private ListElementShiftRegistry<IPQLastTrade>? elementShiftRegistry;

    private DateTime updateTime = DateTime.MinValue;

    private TimeBoundaryPeriod duringPeriod;

    public PQRecentlyTraded()
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(IPQNameIdLookupGenerator nameIdLookup, LastTradedTransmissionFlags transmissionFlags
      , int maxCacheCount = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(nameIdLookup)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        CachedMaxCount       = maxCacheCount;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(LastTradedTransmissionFlags transmissionFlags, int maxCacheCount = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        CachedMaxCount       = maxCacheCount;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(ISourceTickerInfo sourceTickerInfo, LastTradedTransmissionFlags transmissionFlags
      , int maxCacheCount = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(sourceTickerInfo)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        CachedMaxCount       = maxCacheCount;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(ISourceTickerInfo sourceTickerInfo, IPQNameIdLookupGenerator nameIdLookup, LastTradedTransmissionFlags transmissionFlags
      , int maxCacheCount = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(sourceTickerInfo, nameIdLookup)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        CachedMaxCount       = maxCacheCount;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(LastTradedTransmissionFlags transmissionFlags, IEnumerable<IPQLastTrade> lastTrades
      , int maxCacheCount = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(lastTrades)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        CachedMaxCount       = maxCacheCount;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(LastTradedTransmissionFlags transmissionFlags, IList<IPQLastTrade> lastTrades
      , int maxCacheCount = IRecentlyTradedHistory.PublishedHistoryMaxTradeCount) : base(lastTrades)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        CachedMaxCount       = maxCacheCount;
        TransferFlags        = transmissionFlags;

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(IRecentlyTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null) : base(toClone, nameIdLookup)
    {
        elementShiftRegistry = new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        TransferFlags        = toClone.TransferFlags;
        CachedMaxCount       = toClone.CachedMaxCount;
        UpdateTime = toClone.UpdateTime;
        DuringPeriod = toClone.DuringPeriod;

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQRecentlyTraded)) NumUpdatesSinceEmpty = 0;
    }

    public PQRecentlyTraded(IPQRecentlyTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null) : this((IRecentlyTraded)toClone, nameIdLookup) { }

    public PQRecentlyTraded(PQRecentlyTraded toClone, IPQNameIdLookupGenerator? nameIdLookup = null) : this((IRecentlyTraded)toClone, nameIdLookup) { }

    public LastTradedTransmissionFlags TransferFlags { get; set; }

    public TimeBoundaryPeriod DuringPeriod
    {
        get => duringPeriod;
        set
        {
            IsDuringPeriodUpdated |= value != duringPeriod || NumUpdatesSinceEmpty == 0;

            duringPeriod = value;
        }
    }

    public DateTime UpdateTime
    {
        get => updateTime;
        set
        {
            IsPeriodUpdateDateUpdated |= value.Get2MinIntervalsFromUnixEpoch() != updateTime.Get2MinIntervalsFromUnixEpoch() || NumUpdatesSinceEmpty == 0;
            IsPeriodUpdateSub2MinTimeUpdated |= value.GetSub2MinComponent() != updateTime.GetSub2MinComponent() || NumUpdatesSinceEmpty == 0;

            updateTime                =  value;
            if(elementShiftRegistry != null) elementShiftRegistry.UpdateTime = value;
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

    public IReadOnlyList<ElementShift> ElementShifts
    {
        get => elementShiftRegistry?.ShiftCommands.AsReadOnly() ?? new List<ElementShift>().AsReadOnly();
        set
        {
            if(elementShiftRegistry != null) elementShiftRegistry.ShiftCommands = (List<ElementShift>)value;
        }
    }

    public ushort? ClearedElementsAfterIndex
    {
        get => elementShiftRegistry?.ClearRemainingElementsAt ?? ushort.MaxValue;
        set
        {
            if (elementShiftRegistry != null) elementShiftRegistry.ClearRemainingElementsAt = value;
        }
    }

    public bool HasRandomAccessUpdates
    {
        get => elementShiftRegistry?.HasRandomAccessUpdates ?? false;
        set
        {
            if (elementShiftRegistry != null) elementShiftRegistry.HasRandomAccessUpdates = value;
        }
    }

    public int CachedMaxCount
    {
        get => elementShiftRegistry?.CachedMaxCount ?? PQFeedFieldsExtensions.TwoByteFieldIdMaxBookDepth;
        set
        {
            if (elementShiftRegistry != null) elementShiftRegistry.CachedMaxCount = value;
        }
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

    public bool IsEmpty
    {
        get => LastTrades.All(lt => lt.IsEmpty);
        set
        {
            foreach (var lastTrade in LastTrades)
            {
                lastTrade.IsEmpty = value;
            }

            if (!value) return;
            NumUpdatesSinceEmpty = 0;
        }
    }

    // more recent trades appear at start
    // find the shift of the first entry in previous collection in the updated collection
    public void CalculateShift
        (DateTime asAtTime, IReadOnlyList<IPQLastTrade> updatedCollection) =>
        elementShiftRegistry!.CalculateShift(asAtTime, updatedCollection);

    public ElementShift ShiftElements
        (int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry!.ShiftElements(byElements, pinElementsFromIndex);

    public ElementShift ApplyElementShift(ElementShift shiftToApply) => elementShiftRegistry!.ApplyElementShift(shiftToApply);

    public ElementShift InsertAtStart(IPQLastTrade toInsertAtStart) => elementShiftRegistry!.InsertAtStart(toInsertAtStart);

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

    public ElementShift InsertAt(int index, IPQLastTrade toInsertAtStart) => elementShiftRegistry!.InsertAt(index, toInsertAtStart);

    public ElementShift DeleteAt(int index) => elementShiftRegistry!.DeleteAt(index);

    public ElementShift ClearAll() => elementShiftRegistry!.ClearAll();

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

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;

        // Potentially only send this with a snapshot TBD

        foreach (var shiftCommand in elementShiftRegistry!.ShiftCommands)
        {
            yield return new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, PQTradingSubFieldKeys.CommandElementsShift
                                         , (uint)shiftCommand);
        }

        foreach (var lastTradeUpdates in base.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
        {
            yield return lastTradeUpdates.WithFieldId(PQFeedFields.LastTradedAllRecentlyLimitedHistory);
        }

        if (!updatedOnly || IsDuringPeriodUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, PQTradingSubFieldKeys.LastTradedSummaryPeriod
                                         , (uint)DuringPeriod);

        if (!updatedOnly || IsPeriodUpdateDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.LastTradedAllRecentlyLimitedHistory, PQTradingSubFieldKeys.LastTradedPeriodUpdateDate
                                         , updateTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsPeriodUpdateSub2MinTimeUpdated)
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
                    var elementShift = (ElementShift)(pqFieldUpdate.Payload);
                    elementShiftRegistry!.ShiftCommands.Append(elementShift);
                    ApplyElementShift(elementShift);
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
        Recycler?.Borrow<PQRecentlyTraded>().CopyFrom(this) ??
        new PQRecentlyTraded((IRecentlyTraded)this);


    public override PQRecentlyTraded CopyFrom
        (ILastTradedList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        elementShiftRegistry ??= new ListElementShiftRegistry<IPQLastTrade>(this, NewElementFactory);
        base.CopyFrom(source, copyMergeFlags);
        if (source is IRecentlyTraded recentlyTraded)
        {
            TransferFlags = recentlyTraded.TransferFlags;
            UpdateTime = recentlyTraded.UpdateTime;
            DuringPeriod = recentlyTraded.DuringPeriod;

            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (isFullReplace) SetFlagsSame(recentlyTraded);
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
        $"{PQLastTradedListToStringMembers}, {nameof(TransferFlags)}: {TransferFlags}, {nameof(UpdateTime)}: {UpdateTime:O}, {nameof(DuringPeriod)}: {DuringPeriod}";


    protected string UpdateFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{nameof(PQRecentlyTraded)}{{{PQRecentlyTradedToStringMembers}, {UpdateFlagsToString}}}";
}
