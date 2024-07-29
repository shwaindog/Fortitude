// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsApi.Pricing;

public interface IValidRangeBidAskPeriod : IReusableObject<IValidRangeBidAskPeriod>, IBidAskInstant, IDoublyLinkedListNode<IValidRangeBidAskPeriod>
{
    bool UsePreviousValues       { get; set; }
    bool UseNextValues           { get; set; }
    bool ExcludeValues           { get; set; }
    bool ExcludeTime             { get; set; }
    bool SweepMarkedForAggregate { get; set; }
    bool SweepMarkedForBreakdown { get; set; }
    bool SweepReplace            { get; set; }
    bool SweepRemove             { get; set; }
    bool SweepEarliestToKeep     { get; set; }
    bool SweepLatestToKeep       { get; set; }
    bool SweepStartOfMarketDay   { get; set; }
    bool SweepEndOfMarketDay     { get; set; }
    bool SweepStartOfMarketWeek  { get; set; }
    bool SweepEndOfMarketWeek    { get; set; }
    bool SweepStartOfDataGap     { get; set; }
    bool SweepEndOfDataGap       { get; set; }

    TimePeriod CoveringPeriod { get; }

    DateTime ValidFrom { get; }
    DateTime ValidTo   { get; }

    new IValidRangeBidAskPeriod? Next { get; set; }

    new IValidRangeBidAskPeriod? Previous { get; set; }

    void GroupSetBooleanValues(ValidPeriodFlags flags);

    TimeSpan ValidTime(BoundedTimeRange timeRange);

    TimeSpan InvalidTime(BoundedTimeRange timeRange);

    BoundedTimeRange? ValidTimeRange();

    BoundedTimeRange CoveringRange(DateTime maxDateTime);

    IValidRangeBidAskPeriod? ContainingPeriodEndNode(TimeSeriesPeriod forPeriod);

    IDoublyLinkedList<IValidRangeBidAskPeriod> ReplaceRange(IValidRangeBidAskPeriod endNode, IDoublyLinkedList<IValidRangeBidAskPeriod> replacements);

    IValidRangeBidAskPeriod? SwapOut(IDoublyLinkedList<IValidRangeBidAskPeriod> replacements);

    IValidRangeBidAskPeriod LastContiguousPeriodEndNode();

    new IValidRangeBidAskPeriod Clone();
}

public struct ValidRangeBidAskPeriodValue
{
    public ValidRangeBidAskPeriodValue() { }

    public ValidRangeBidAskPeriodValue(ValidRangeBidAskPeriodValue toClone)
    {
        BidPrice  = toClone.BidPrice;
        AskPrice  = toClone.AskPrice;
        AtTime    = toClone.AtTime;
        ValidTo   = toClone.ValidTo;
        ValidFrom = toClone.ValidFrom.Max(AtTime);

        CoveringPeriod = toClone.CoveringPeriod;
    }

    public ValidRangeBidAskPeriodValue(IValidRangeBidAskPeriod toClone)
    {
        BidPrice  = toClone.BidPrice;
        AskPrice  = toClone.AskPrice;
        AtTime    = toClone.AtTime;
        ValidTo   = toClone.ValidTo;
        ValidFrom = toClone.ValidFrom.Max(AtTime);

        CoveringPeriod = toClone.CoveringPeriod;
    }

    public ValidRangeBidAskPeriodValue(ILevel1Quote toCapture)
    {
        BidPrice  = toCapture.BidPriceTop;
        AskPrice  = toCapture.AskPriceTop;
        AtTime    = toCapture.SourceTime;
        ValidTo   = toCapture.ValidTo;
        ValidFrom = toCapture.ValidFrom.Max(AtTime);

        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.Tick);
    }

    public ValidRangeBidAskPeriodValue
        (decimal bidPrice, decimal askPrice, DateTime validTo, DateTime? atTime = null, DateTime? validFrom = null, TimePeriod? coveringPeriod = null)
    {
        BidPrice  = bidPrice;
        AskPrice  = askPrice;
        AtTime    = atTime ?? DateTime.UtcNow;
        ValidTo   = validTo;
        ValidFrom = (validFrom ?? AtTime).Max(AtTime);

        CoveringPeriod = coveringPeriod ?? new TimePeriod(TimeSeriesPeriod.Tick);
    }

    public decimal  BidPrice  { get; }
    public decimal  AskPrice  { get; }
    public DateTime AtTime    { get; }
    public DateTime ValidFrom { get; }
    public DateTime ValidTo   { get; }

    public TimePeriod CoveringPeriod { get; }

    public static implicit operator BidAskInstantPair(ValidRangeBidAskPeriodValue toConvert) =>
        new(toConvert.BidPrice, toConvert.AskPrice, toConvert.AtTime);
}

public static class ValidRangeBidAskPeriodValueExtensions
{
    public static ValidRangeBidAskPeriod ToValidRangeBidAskPeriod(this ValidRangeBidAskPeriodValue pair, IRecycler? recycler = null)
    {
        var instant = recycler?.Borrow<ValidRangeBidAskPeriod>() ?? new ValidRangeBidAskPeriod();
        instant.Configure(pair);
        return instant;
    }

    public static ValidRangeBidAskPeriodValue SetBidPrice
        (this ValidRangeBidAskPeriodValue pair, decimal bidPrice) =>
        new(bidPrice, pair.AskPrice, pair.ValidTo, pair.AtTime, pair.ValidFrom);

    public static ValidRangeBidAskPeriodValue SetAskPrice
        (this ValidRangeBidAskPeriodValue pair, decimal askPrice) =>
        new(pair.BidPrice, askPrice, pair.ValidTo, pair.AtTime, pair.ValidFrom);

    public static ValidRangeBidAskPeriodValue SetAtTime
        (this ValidRangeBidAskPeriodValue pair, DateTime atTime) =>
        new(pair.BidPrice, pair.AskPrice, pair.ValidTo, atTime, pair.ValidFrom);

    public static ValidRangeBidAskPeriodValue SetValidTo
        (this ValidRangeBidAskPeriodValue pair, DateTime validTo) =>
        new(pair.BidPrice, pair.AskPrice, validTo, pair.AtTime, pair.ValidFrom);

    public static ValidRangeBidAskPeriodValue SetValidFrom
        (this ValidRangeBidAskPeriodValue pair, DateTime validFrom) =>
        new(pair.BidPrice, pair.AskPrice, pair.ValidTo, pair.AtTime, validFrom);
}

[Flags]
public enum ValidPeriodFlags : ushort
{
    None                 = 0x00_00
  , ExcludeValues        = 0x00_01
  , ExcludeTime          = 0x00_02
  , UsePreviousValues    = 0x00_04
  , UseNextValues        = 0x00_08
  , MarkedForAggregate   = 0x00_10
  , MarkedForBreakdown   = 0x00_20
  , MarkedForReplace     = 0x00_40
  , MarkedForRemove      = 0x00_80
  , MarkedEarliestToKeep = 0x01_00
  , MarkedLatestToKeep   = 0x02_00
  , StartOfMarketDay     = 0x04_00
  , EndOfMarketDay       = 0x08_00
  , StartOfMarketWeek    = 0x10_00
  , EndOfMarketWeek      = 0x20_00
  , StartOfDataGap       = 0x40_00
  , EndOfDataGap         = 0x80_00
}

public class ValidRangeBidAskPeriod : BidAskInstant, IValidRangeBidAskPeriod, ICloneable<ValidRangeBidAskPeriod>
  , IDoublyLinkedListNode<ValidRangeBidAskPeriod>
{
    private ValidPeriodFlags booleanFlags;

    private DateTime validTo;
    public ValidRangeBidAskPeriod() { }

    public ValidRangeBidAskPeriod
        (BidAskInstantPair bidAskInstantPair, DateTime validToTime, DateTime? validFrom = null, TimePeriod? coveringPeriod = null)
        : base(bidAskInstantPair)
    {
        CoveringPeriod = coveringPeriod ?? new TimePeriod(TimeSeriesPeriod.Tick);

        ValidTo   = validToTime;
        ValidFrom = (validFrom ?? AtTime).Max(AtTime);
    }

    public ValidRangeBidAskPeriod
        (BidAskInstant bidAskInstant, DateTime validToTime, DateTime? validFrom = null, TimePeriod? coveringPeriod = null)
        : base(bidAskInstant)
    {
        CoveringPeriod = coveringPeriod ?? new TimePeriod(TimeSeriesPeriod.Tick);

        ValidTo   = validToTime;
        ValidFrom = (validFrom ?? AtTime).Max(AtTime);
    }

    public ValidRangeBidAskPeriod(ILevel1Quote toCapture) : base(toCapture)
    {
        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.Tick);

        ValidTo   = toCapture.FeedSyncStatus == FeedSyncStatus.Good ? toCapture.ValidTo : AtTime;
        ValidFrom = toCapture.ValidFrom.Max(AtTime);
    }

    public ValidRangeBidAskPeriod
    (decimal bidPrice, decimal askPrice, DateTime validToTime, DateTime? atTime = null
      , DateTime? validFromTime = null, TimePeriod? coveringPeriod = null)
        : base(bidPrice, askPrice, atTime)
    {
        CoveringPeriod = coveringPeriod ?? new TimePeriod(TimeSeriesPeriod.Tick);

        ValidTo   = validToTime;
        ValidFrom = (validFromTime ?? AtTime).Max(AtTime);
    }

    public ValidRangeBidAskPeriod(IValidRangeBidAskPeriod toClone) : base(toClone)
    {
        CoveringPeriod = toClone.CoveringPeriod;

        ValidTo   = toClone.ValidTo;
        ValidFrom = toClone.ValidFrom;
    }

    public ValidRangeBidAskPeriod(ValidRangeBidAskPeriodValue toClone) : base(toClone)
    {
        CoveringPeriod = toClone.CoveringPeriod;

        ValidTo   = toClone.ValidTo;
        ValidFrom = toClone.ValidFrom;
    }

    public new ValidRangeBidAskPeriod Clone() =>
        Recycler?.Borrow<ValidRangeBidAskPeriod>().CopyFrom(this) as ValidRangeBidAskPeriod
     ?? new ValidRangeBidAskPeriod((IValidRangeBidAskPeriod)this);

    public new ValidRangeBidAskPeriod? Previous
    {
        get => base.Previous as ValidRangeBidAskPeriod;
        set => base.Previous = value;
    }

    public new ValidRangeBidAskPeriod? Next
    {
        get => base.Next as ValidRangeBidAskPeriod;
        set => base.Previous = value;
    }

    public TimePeriod CoveringPeriod { get; protected set; }

    IReusableObject<IValidRangeBidAskPeriod> IStoreState<IReusableObject<IValidRangeBidAskPeriod>>.CopyFrom
        (IReusableObject<IValidRangeBidAskPeriod> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IValidRangeBidAskPeriod)source, copyMergeFlags);

    public IValidRangeBidAskPeriod CopyFrom(IValidRangeBidAskPeriod source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        ValidTo        = source.ValidTo;
        ValidFrom      = source.ValidFrom;
        CoveringPeriod = source.CoveringPeriod;
        if (source is ValidRangeBidAskPeriod validRangeBidAskPeriod)
        {
            booleanFlags = validRangeBidAskPeriod.booleanFlags;
        }
        else
        {
            UsePreviousValues       = source.UsePreviousValues;
            UseNextValues           = source.UseNextValues;
            ExcludeValues           = source.ExcludeValues;
            ExcludeTime             = source.ExcludeTime;
            SweepMarkedForAggregate = source.SweepMarkedForAggregate;
            SweepMarkedForBreakdown = source.SweepMarkedForBreakdown;
            SweepReplace            = source.SweepReplace;
            SweepRemove             = source.SweepRemove;
            SweepEarliestToKeep     = source.SweepEarliestToKeep;
            SweepLatestToKeep       = source.SweepLatestToKeep;
            SweepStartOfMarketDay   = source.SweepStartOfMarketDay;
            SweepEndOfMarketDay     = source.SweepEndOfMarketDay;
            SweepStartOfMarketWeek  = source.SweepStartOfMarketWeek;
            SweepEndOfMarketWeek    = source.SweepEndOfMarketWeek;
            SweepStartOfDataGap     = source.SweepStartOfDataGap;
            SweepEndOfDataGap       = source.SweepEndOfDataGap;
        }

        return this;
    }

    IValidRangeBidAskPeriod? IValidRangeBidAskPeriod.Previous
    {
        get => base.Previous as IValidRangeBidAskPeriod;
        set => base.Previous = value;
    }

    IValidRangeBidAskPeriod? IValidRangeBidAskPeriod.Next
    {
        get => base.Next as IValidRangeBidAskPeriod;
        set => base.Previous = value;
    }

    IValidRangeBidAskPeriod? IDoublyLinkedListNode<IValidRangeBidAskPeriod>.Previous
    {
        get => base.Previous as IValidRangeBidAskPeriod;
        set => base.Previous = value;
    }

    IValidRangeBidAskPeriod? IDoublyLinkedListNode<IValidRangeBidAskPeriod>.Next
    {
        get => base.Next as IValidRangeBidAskPeriod;
        set => base.Previous = value;
    }

    public bool UsePreviousValues
    {
        get => (booleanFlags & ValidPeriodFlags.UsePreviousValues) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.UsePreviousValues;
            else
                booleanFlags &= ~ValidPeriodFlags.UsePreviousValues;
        }
    }
    public bool UseNextValues
    {
        get => (booleanFlags & ValidPeriodFlags.UseNextValues) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.UseNextValues;
            else
                booleanFlags &= ~ValidPeriodFlags.UseNextValues;
        }
    }
    public bool ExcludeValues
    {
        get => (booleanFlags & ValidPeriodFlags.ExcludeValues) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.ExcludeValues;
            else
                booleanFlags &= ~ValidPeriodFlags.ExcludeValues;
        }
    }
    public bool ExcludeTime
    {
        get => (booleanFlags & ValidPeriodFlags.ExcludeTime) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.ExcludeTime;
            else
                booleanFlags &= ~ValidPeriodFlags.ExcludeTime;
        }
    }
    public bool SweepMarkedForAggregate
    {
        get => (booleanFlags & ValidPeriodFlags.MarkedForAggregate) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.MarkedForAggregate;
            else
                booleanFlags &= ~ValidPeriodFlags.MarkedForAggregate;
        }
    }
    public bool SweepMarkedForBreakdown
    {
        get => (booleanFlags & ValidPeriodFlags.MarkedForBreakdown) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.MarkedForBreakdown;
            else
                booleanFlags &= ~ValidPeriodFlags.MarkedForBreakdown;
        }
    }
    public bool SweepReplace
    {
        get => (booleanFlags & ValidPeriodFlags.MarkedForReplace) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.MarkedForReplace;
            else
                booleanFlags &= ~ValidPeriodFlags.MarkedForReplace;
        }
    }
    public bool SweepRemove
    {
        get => (booleanFlags & ValidPeriodFlags.MarkedForRemove) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.MarkedForRemove;
            else
                booleanFlags &= ~ValidPeriodFlags.MarkedForRemove;
        }
    }
    public bool SweepEarliestToKeep
    {
        get => (booleanFlags & ValidPeriodFlags.MarkedEarliestToKeep) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.MarkedEarliestToKeep;
            else
                booleanFlags &= ~ValidPeriodFlags.MarkedEarliestToKeep;
        }
    }
    public bool SweepLatestToKeep
    {
        get => (booleanFlags & ValidPeriodFlags.MarkedLatestToKeep) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.MarkedLatestToKeep;
            else
                booleanFlags &= ~ValidPeriodFlags.MarkedLatestToKeep;
        }
    }
    public bool SweepStartOfMarketDay
    {
        get => (booleanFlags & ValidPeriodFlags.StartOfMarketDay) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.StartOfMarketDay;
            else
                booleanFlags &= ~ValidPeriodFlags.StartOfMarketDay;
        }
    }
    public bool SweepEndOfMarketDay
    {
        get => (booleanFlags & ValidPeriodFlags.EndOfMarketDay) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.EndOfMarketDay;
            else
                booleanFlags &= ~ValidPeriodFlags.EndOfMarketDay;
        }
    }
    public bool SweepStartOfMarketWeek
    {
        get => (booleanFlags & ValidPeriodFlags.StartOfMarketWeek) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.StartOfMarketWeek;
            else
                booleanFlags &= ~ValidPeriodFlags.StartOfMarketWeek;
        }
    }
    public bool SweepEndOfMarketWeek
    {
        get => (booleanFlags & ValidPeriodFlags.EndOfMarketWeek) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.EndOfMarketWeek;
            else
                booleanFlags &= ~ValidPeriodFlags.EndOfMarketWeek;
        }
    }
    public bool SweepStartOfDataGap
    {
        get => (booleanFlags & ValidPeriodFlags.StartOfDataGap) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.StartOfDataGap;
            else
                booleanFlags &= ~ValidPeriodFlags.StartOfDataGap;
        }
    }
    public bool SweepEndOfDataGap
    {
        get => (booleanFlags & ValidPeriodFlags.EndOfDataGap) > 0;
        set
        {
            if (value)
                booleanFlags |= ValidPeriodFlags.EndOfDataGap;
            else
                booleanFlags &= ~ValidPeriodFlags.EndOfDataGap;
        }
    }

    public void GroupSetBooleanValues(ValidPeriodFlags flags)
    {
        booleanFlags = flags;
    }

    public DateTime ValidTo
    {
        get
        {
            if (Next == null || Next.AtTime > validTo) return validTo;
            return Next.AtTime;
        }
        set => validTo = value;
    }

    public DateTime ValidFrom { get; set; }

    public BoundedTimeRange? ValidTimeRange()
    {
        if (ValidTo <= ValidFrom) return null;
        return new BoundedTimeRange(ValidFrom, ValidTo);
    }

    public BoundedTimeRange CoveringRange(DateTime maxDateTime) =>
        maxDateTime < AtTime
            ? new BoundedTimeRange(maxDateTime, maxDateTime)
            : new BoundedTimeRange(AtTime, maxDateTime.Min(Next?.AtTime ?? ValidTo));

    public TimeSpan ValidTime(BoundedTimeRange timeRange)
    {
        if (timeRange.ToTime < ValidFrom || timeRange.FromTime >= ValidTo) return TimeSpan.Zero;

        var validRange = ValidTimeRange();

        var validIntersectionTimeRange = validRange?.Intersection(timeRange);
        return validIntersectionTimeRange?.TimeSpan() ?? TimeSpan.Zero;
    }

    public TimeSpan InvalidTime(BoundedTimeRange timeRange)
    {
        if (timeRange.ToTime < AtTime) return TimeSpan.Zero;
        BoundedTimeRange applicableRange;
        applicableRange = Next == null
            ? timeRange.TrimFromTime(AtTime)
            : new BoundedTimeRange(AtTime, timeRange.ToTime.Min(Next.AtTime));

        var validRange = ValidTimeRange();

        var validIntersectionTimeRange = validRange?.Intersection(applicableRange);
        if (validIntersectionTimeRange != null) return applicableRange.TimeSpan() - validIntersectionTimeRange.Value.TimeSpan();
        return applicableRange.TimeSpan();
    }

    public IValidRangeBidAskPeriod LastContiguousPeriodEndNode()
    {
        IValidRangeBidAskPeriod? currentNode = this;
        var                      nextNode    = Next;
        while (nextNode != null && Equals(nextNode.CoveringPeriod, CoveringPeriod))
        {
            currentNode = nextNode;
            nextNode    = nextNode.Next;
        }
        return currentNode;
    }

    public IValidRangeBidAskPeriod? ContainingPeriodEndNode(TimeSeriesPeriod forPeriod)
    {
        var periodStart = forPeriod.ContainingPeriodBoundaryStart(AtTime);
        var periodEnd   = forPeriod.PeriodEnd(periodStart);

        IValidRangeBidAskPeriod currentNode = this;

        var nextNode = Next;

        while (nextNode != null)
        {
            var nextPeriodStart = forPeriod.ContainingPeriodBoundaryStart(nextNode.AtTime);
            var nextPeriodEnd   = forPeriod.PeriodEnd(nextPeriodStart);
            if (nextPeriodEnd != periodEnd) break;

            currentNode = nextNode;
            nextNode    = nextNode.Next;
        }
        return nextNode != null ? currentNode : null;
    }

    public IDoublyLinkedList<IValidRangeBidAskPeriod> ReplaceRange
        (IValidRangeBidAskPeriod endNode, IDoublyLinkedList<IValidRangeBidAskPeriod> replacements)
    {
        if (replacements.IsEmpty) return new DoublyLinkedList<IValidRangeBidAskPeriod>();
        if (Previous != null)
        {
            ((IValidRangeBidAskPeriod)Previous).Next = replacements.Head;

            replacements.Head!.Previous = Previous;
        }
        if (endNode.Next != null)
        {
            endNode.Next.Previous   = replacements.Tail;
            replacements.Tail!.Next = endNode.Next;
        }
        if (Previous == null) replacements.Head!.Previous = null;
        if (Next == null) replacements.Tail!.Next         = null;
        replacements.DetachNodes();
        Previous     = null;
        endNode.Next = null;
        IValidRangeBidAskPeriod? currentNode = this;

        while (currentNode != endNode && currentNode != null)
        {
            replacements.AddLast(currentNode);
            currentNode = currentNode.Next;
        }
        if (currentNode != null) replacements.AddLast(currentNode);
        return replacements;
    }

    public IValidRangeBidAskPeriod? SwapOut(IDoublyLinkedList<IValidRangeBidAskPeriod> replacements)
    {
        if (replacements.IsEmpty) return null;
        if (Previous != null)
        {
            ((IValidRangeBidAskPeriod)Previous).Next = replacements.Head;
            replacements.Head!.Previous              = Previous;
        }
        if (Next != null)
        {
            ((IValidRangeBidAskPeriod)Next).Previous = replacements.Tail;
            replacements.Tail!.Next                  = Next;
        }
        if (Previous == null) replacements.Head!.Previous = null;

        if (Next == null) replacements.Tail!.Next = null;

        replacements.DetachNodes();
        Next     = null;
        Previous = null;
        return this;
    }

    IValidRangeBidAskPeriod IValidRangeBidAskPeriod.Clone() => Clone();

    IValidRangeBidAskPeriod ICloneable<IValidRangeBidAskPeriod>.Clone() => Clone();

    public void Configure
    (BidAskPair bidAskPair, DateTime atTime, DateTime validToTime, DateTime? validFromTime = null
      , TimePeriod? coveringPeriod = null)
    {
        BidPrice       = bidAskPair.BidPrice;
        AskPrice       = bidAskPair.AskPrice;
        AtTime         = atTime;
        CoveringPeriod = coveringPeriod ?? new TimePeriod(TimeSeriesPeriod.Tick);
        ValidTo        = validToTime;
        ValidFrom      = (validFromTime ?? AtTime).Max(AtTime);
        booleanFlags   = ValidPeriodFlags.None;
    }

    public void Configure(ValidRangeBidAskPeriodValue toCopy)
    {
        BidPrice       = toCopy.BidPrice;
        AskPrice       = toCopy.AskPrice;
        AtTime         = toCopy.AtTime;
        CoveringPeriod = toCopy.CoveringPeriod;
        ValidTo        = toCopy.ValidTo;
        ValidFrom      = toCopy.ValidFrom;
        booleanFlags   = ValidPeriodFlags.None;
    }

    public void Configure(IValidRangeBidAskPeriod toCopy)
    {
        BidPrice       = toCopy.BidPrice;
        AskPrice       = toCopy.AskPrice;
        AtTime         = toCopy.AtTime;
        CoveringPeriod = toCopy.CoveringPeriod;
        ValidTo        = toCopy.ValidTo;
        ValidFrom      = toCopy.ValidFrom;
        if (toCopy is ValidRangeBidAskPeriod validRangeBidAskPeriod) booleanFlags = validRangeBidAskPeriod.booleanFlags;
    }

    public override IValidRangeBidAskPeriod CopyFrom(ILevel1Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        ValidTo        = source.ValidTo;
        ValidFrom      = source.ValidFrom;
        CoveringPeriod = new TimePeriod(TimeSeriesPeriod.Tick);

        return this;
    }
}

public static class ValidRangeBidAskInstantExtensions
{
    public static bool IsValidAt(this IValidRangeBidAskPeriod validRBidAsk, DateTime time) =>
        time >= validRBidAsk.AtTime && time < validRBidAsk.ValidTo;

    public static bool IsValidAt(this ValidRangeBidAskPeriodValue validRBidAsk, DateTime time) =>
        time >= validRBidAsk.AtTime && time < validRBidAsk.ValidTo;

    public static bool HasValidPeriod(this IValidRangeBidAskPeriod validRBidAsk) => validRBidAsk.IsValidAt(validRBidAsk.AtTime);

    public static bool HasValidPeriod(this ValidRangeBidAskPeriodValue validRBidAsk) => validRBidAsk.IsValidAt(validRBidAsk.AtTime);

    public static bool IsMarketClosed
        (this IValidRangeBidAskPeriod checkIsClose) =>
        checkIsClose.SweepEndOfMarketDay || checkIsClose.SweepEndOfMarketWeek;

    public static int AddChain(this IDoublyLinkedList<ValidRangeBidAskPeriod> existing, ValidRangeBidAskPeriod chainToAdd)
    {
        var currentExisting = existing.Head;
        if (currentExisting == null)
        {
            var nextChainToAdd = chainToAdd.Next;
            existing.AddFirst(chainToAdd);
            return 1 + (nextChainToAdd != null ? existing.AddChain(nextChainToAdd) : 0);
        }
        while (currentExisting != null)
        {
            if (ReferenceEquals(currentExisting, chainToAdd)) break;
            if (chainToAdd.AtTime == currentExisting.AtTime) break;
            if (currentExisting.AtTime > chainToAdd.AtTime)
            {
                var insertAfter    = currentExisting;
                var nextChainToAdd = chainToAdd.Next;
                insertAfter.Next         = chainToAdd;
                chainToAdd.Previous      = insertAfter;
                currentExisting.Previous = chainToAdd;
                chainToAdd.Next          = currentExisting;
                return 1 + (nextChainToAdd != null ? existing.AddChain(nextChainToAdd) : 0);
            }
            if (currentExisting?.Next == null)
            {
                var nextChainToAdd = chainToAdd.Next;
                existing.AddLast(chainToAdd);
                return 1 + (nextChainToAdd != null ? existing.AddChain(nextChainToAdd) : 0);
            }

            currentExisting = currentExisting.Next;
        }
        return 0;
    }

    public static int AddChainIncRefCount
        (this IDoublyLinkedList<ValidRangeBidAskPeriod> existing, ValidRangeBidAskPeriod chainToAdd)
    {
        var toAdd = chainToAdd;
        while (toAdd != null)
        {
            toAdd.IncrementRefCount();
            toAdd = toAdd.Next;
        }
        return existing.AddChain(chainToAdd);
    }

    public static int ClearTimeRange(this IDoublyLinkedList<ValidRangeBidAskPeriod> existing, BoundedTimeRange timeRangeToClear)
    {
        var currentExisting = existing.Head;
        if (currentExisting == null) return 0;
        var countRemoved = 0;
        while (currentExisting != null)
        {
            if (timeRangeToClear.Contains(currentExisting.AtTime))
            {
                var removed = existing.Remove(currentExisting);
                countRemoved++;
                removed.DecrementRefCount();
            }
            if (currentExisting.AtTime > timeRangeToClear.ToTime) break;
            currentExisting = currentExisting.Next;
        }
        return countRemoved;
    }

    public static IValidRangeBidAskPeriod ToValidBidAskPeriod(this ILevel1Quote level1Quote, IRecycler? recycler = null)
    {
        var bidAskPeriod = recycler?.Borrow<ValidRangeBidAskPeriod>().CopyFrom(level1Quote) ?? new ValidRangeBidAskPeriod(level1Quote);
        return bidAskPeriod;
    }

    public static IValidRangeBidAskPeriod AverageToValidRangeBidAskPeriod(this IPricePeriodSummary pricePeriodSummary, IRecycler? recycler = null)
    {
        var bidAskPeriod = recycler?.Borrow<ValidRangeBidAskPeriod>() ?? new ValidRangeBidAskPeriod();

        var missingStartIndex   = 0;
        var missingPeriodsFlags = (uint)pricePeriodSummary.PeriodSummaryFlags >> 16;
        for (var i = 1; i <= 16; i++)
        {
            var isMissing = (missingPeriodsFlags & 1) > 0;
            missingPeriodsFlags >>= 1;
            if (isMissing)
                missingStartIndex = i;
            else
                break;
        }
        missingPeriodsFlags = (uint)pricePeriodSummary.PeriodSummaryFlags >> 16;
        var missingEndIndex = 0;
        for (var i = 1; i <= 16; i++)
        {
            var isMissing = (missingPeriodsFlags & 0x80_00) > 0;
            missingPeriodsFlags <<= 1;
            if (isMissing)
                missingEndIndex = i;
            else
                break;
        }
        DateTime validFrom;
        DateTime validTo;
        switch (missingStartIndex)
        {
            case 0 when missingEndIndex == 0:
                validFrom = pricePeriodSummary.PeriodStartTime;
                validTo   = pricePeriodSummary.PeriodEndTime;
                break;
            case 16 when missingEndIndex == 16:
                validFrom = pricePeriodSummary.PeriodStartTime;
                validTo   = pricePeriodSummary.PeriodStartTime;
                break;
            default:
            {
                var totalTimeSpan = pricePeriodSummary.PeriodEndTime - pricePeriodSummary.PeriodStartTime;
                var oneSixteenth  = totalTimeSpan / 16;

                validFrom = pricePeriodSummary.PeriodStartTime + oneSixteenth * missingStartIndex;
                validTo   = pricePeriodSummary.PeriodEndTime - oneSixteenth * missingEndIndex;
                break;
            }
        }

        bidAskPeriod.Configure(pricePeriodSummary.AverageBidAsk, pricePeriodSummary.PeriodStartTime, validTo
                             , validFrom, new TimePeriod(pricePeriodSummary.TimeSeriesPeriod));
        return bidAskPeriod;
    }
}
