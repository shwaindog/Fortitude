// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.PQ.Summaries;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public interface IPQLevel1Quote : IPQTickInstant, IMutableLevel1Quote, IDoublyLinkedListNode<IPQLevel1Quote>
{
    bool IsSourceAskTimeDateUpdated          { get; set; }
    bool IsSourceAskTimeSubHourUpdated       { get; set; }
    bool IsSourceBidTimeDateUpdated          { get; set; }
    bool IsSourceBidTimeSubHourUpdated       { get; set; }
    bool IsAdapterSentTimeDateUpdated        { get; set; }
    bool IsAdapterSentTimeSubHourUpdated     { get; set; }
    bool IsAdapterReceivedTimeDateUpdated    { get; set; }
    bool IsAdapterReceivedTimeSubHourUpdated { get; set; }
    bool IsValidFromTimeDateUpdated          { get; set; }
    bool IsValidFromTimeSubHourUpdated       { get; set; }
    bool IsValidToTimeDateUpdated            { get; set; }
    bool IsValidToTimeSubHourUpdated         { get; set; }

    bool IsBidPriceTopUpdated { get; set; }
    bool IsAskPriceTopUpdated { get; set; }
    bool IsBidPriceTopChangedUpdated { get; set; }
    bool IsAskPriceTopChangedUpdated { get; set; }
    bool IsExecutableUpdated  { get; set; }


    new IPQLevel1Quote? Next     { get; set; }
    new IPQLevel1Quote? Previous { get; set; }

    new IPQPricePeriodSummary? SummaryPeriod { get; set; }

    new IPQLevel1Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);
    new IPQLevel1Quote Clone();
}

public class PQLevel1Quote : PQTickInstant, IPQLevel1Quote, ICloneable<PQLevel1Quote>
  , IDoublyLinkedListNode<PQLevel1Quote>
{
    private DateTime adapterReceivedTime;
    private DateTime adapterSentTime;
    private decimal  askPriceTop;
    private decimal  bidPriceTop;

    protected DateTime sourceAskTime;
    protected DateTime sourceBidTime;

    protected DateTime validFromTime;
    protected DateTime validToTime;

    public PQLevel1Quote()
    {
        BooleanFields |= PQBooleanValues.IsExecutableSetFlag;

        if (GetType() == typeof(PQLevel1Quote)) NumOfUpdates = 0;
    }

    public PQLevel1Quote(ISourceTickerInfo sourceTickerInfo)
        : base(sourceTickerInfo)
    {
        Executable = true;

        if (GetType() == typeof(PQLevel1Quote)) NumOfUpdates = 0;
    }

    public PQLevel1Quote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel1Quote l1QToClone)
        {
            adapterSentTime     = l1QToClone.AdapterSentTime;
            adapterReceivedTime = l1QToClone.AdapterReceivedTime;
            sourceAskTime       = l1QToClone.SourceAskTime;
            sourceBidTime       = l1QToClone.SourceBidTime;
            validFromTime       = l1QToClone.ValidFrom;
            validToTime         = l1QToClone.ValidTo;
            Executable          = l1QToClone.Executable;
            if (this is not ILevel2Quote)
            {
                bidPriceTop          = l1QToClone.BidPriceTop;
                askPriceTop          = l1QToClone.AskPriceTop;
                IsBidPriceTopChanged = l1QToClone.IsBidPriceTopChanged;
                IsAskPriceTopChanged = l1QToClone.IsAskPriceTopChanged;
            }
            if (l1QToClone.SummaryPeriod is { IsEmpty: false }) SummaryPeriod = new PQPricePeriodSummary(l1QToClone.SummaryPeriod);
            if (toClone is IPQLevel1Quote ipqL1)
            {
                IsExecutableUpdated  = ipqL1.IsExecutableUpdated;
                IsBidPriceTopUpdated = ipqL1.IsBidPriceTopUpdated;
                IsAskPriceTopUpdated = ipqL1.IsAskPriceTopUpdated;
                IsBidPriceTopChanged = ipqL1.IsBidPriceTopChanged;
                IsAskPriceTopChanged = ipqL1.IsAskPriceTopChanged;

                IsSourceAskTimeDateUpdated          = ipqL1.IsSourceAskTimeDateUpdated;
                IsSourceAskTimeSubHourUpdated       = ipqL1.IsSourceAskTimeSubHourUpdated;
                IsSourceBidTimeDateUpdated          = ipqL1.IsSourceBidTimeDateUpdated;
                IsSourceBidTimeSubHourUpdated       = ipqL1.IsSourceBidTimeSubHourUpdated;
                IsAdapterSentTimeDateUpdated        = ipqL1.IsAdapterSentTimeDateUpdated;
                IsAdapterSentTimeSubHourUpdated     = ipqL1.IsAdapterSentTimeSubHourUpdated;
                IsAdapterReceivedTimeDateUpdated    = ipqL1.IsAdapterReceivedTimeDateUpdated;
                IsAdapterReceivedTimeSubHourUpdated = ipqL1.IsAdapterReceivedTimeSubHourUpdated;
                IsValidFromTimeDateUpdated          = ipqL1.IsValidFromTimeDateUpdated;
                IsValidFromTimeSubHourUpdated       = ipqL1.IsValidFromTimeSubHourUpdated;
                IsValidToTimeDateUpdated            = ipqL1.IsValidToTimeDateUpdated;
                IsValidToTimeSubHourUpdated         = ipqL1.IsValidToTimeSubHourUpdated;
            }
        }
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel1Quote)) NumOfUpdates = 0;
    }

    protected string Level1ToStringMembers =>
        $"{TickInstantToStringMembers}, {nameof(IsSourceAskTimeDateUpdated)}: {IsSourceAskTimeDateUpdated}, " +
        $"{nameof(IsSourceAskTimeSubHourUpdated)}: {IsSourceAskTimeSubHourUpdated}, " +
        $"{nameof(IsSourceBidTimeDateUpdated)}: {IsSourceBidTimeDateUpdated}, " +
        $"{nameof(IsSourceBidTimeSubHourUpdated)}: {IsSourceBidTimeSubHourUpdated}, " +
        $"{nameof(IsAdapterSentTimeDateUpdated)}: {IsAdapterSentTimeDateUpdated}, " +
        $"{nameof(IsAdapterSentTimeSubHourUpdated)}: {IsAdapterSentTimeSubHourUpdated}, " +
        $"{nameof(IsAdapterReceivedTimeDateUpdated)}: {IsAdapterReceivedTimeDateUpdated}, " +
        $"{nameof(IsAdapterReceivedTimeSubHourUpdated)}: {IsAdapterReceivedTimeSubHourUpdated}, " +
        $"{nameof(IsValidFromTimeDateUpdated)}: {IsValidFromTimeDateUpdated}, " +
        $"{nameof(IsValidFromTimeSubHourUpdated)}: {IsValidFromTimeSubHourUpdated}, " +
        $"{nameof(IsValidToTimeDateUpdated)}: {IsValidToTimeDateUpdated}, " +
        $"{nameof(IsValidToTimeSubHourUpdated)}: {IsValidToTimeSubHourUpdated}, " +
        $"{nameof(IsExecutableUpdated)}: {IsExecutableUpdated}, {nameof(SourceTime)}: {SourceTime}, " +
        $"{nameof(SourceAskTime)}: {SourceAskTime}, {nameof(SourceBidTime)}: {SourceBidTime}, " +
        $"{nameof(AdapterSentTime)}: {AdapterSentTime}, {nameof(AdapterReceivedTime)}: {AdapterReceivedTime}, " +
        $"{nameof(BidPriceTop)}: {BidPriceTop}, {nameof(IsBidPriceTopChanged)}: {IsBidPriceTopChanged}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop}, {nameof(IsAskPriceTopChanged)}: {IsAskPriceTopChanged}, " +
        $"{nameof(Executable)}: {Executable}, {nameof(SummaryPeriod)}: {SummaryPeriod}";


    public override PQLevel1Quote Clone() =>
        Recycler?.Borrow<PQLevel1Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) as PQLevel1Quote ?? new PQLevel1Quote(this);


    [JsonIgnore]
    public new PQLevel1Quote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PQLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new PQLevel1Quote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PQLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQLevel1Quote? IPQLevel1Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQLevel1Quote? IPQLevel1Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel1Quote? ILevel1Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel1Quote? ILevel1Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel1Quote? IDoublyLinkedListNode<ILevel1Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel1Quote? IDoublyLinkedListNode<ILevel1Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQLevel1Quote? IDoublyLinkedListNode<IPQLevel1Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQLevel1Quote? IDoublyLinkedListNode<IPQLevel1Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Previous { get; set; }
    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Next     { get; set; }

    [JsonIgnore] decimal IBidAskPair.BidPrice => BidPriceTop;

    decimal IBidAskPair.AskPrice => BidPriceTop;

    [JsonIgnore] DateTime IBidAskInstant.AtTime => SourceTime;

    [JsonIgnore]
    public bool IsSourceAskTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag;

            else if (IsSourceAskTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceAskTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceAskTimeSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceAskTimeSubHourUpdatedFlag;

            else if (IsSourceAskTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceAskTimeSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceBidTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag;

            else if (IsSourceBidTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceBidTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceBidTimeSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceBidTimeSubHourUpdatedFlag;

            else if (IsSourceBidTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceBidTimeSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterSentTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag;

            else if (IsAdapterSentTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterSentTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterSentTimeSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterSentTimeSubHourUpdatedFlag;

            else if (IsAdapterSentTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterSentTimeSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;

            else if (IsAdapterReceivedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterReceivedTimeSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterReceivedTimeSubHourUpdatedFlag;

            else if (IsAdapterReceivedTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterReceivedTimeSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsExecutableUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ExecutableUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ExecutableUpdatedFlag;

            else if (IsExecutableUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ExecutableUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsBidPriceTopChanged
    {
        get => BooleanFields.HasBidTopPriceChangedSet();
        set
        {
            IsBidPriceTopChangedUpdated |= value != IsBidPriceTopChanged || NumOfUpdates == 0;
            if (value)
                BooleanFields |= PQBooleanValues.IsBidPriceTopChangedSetFlag;

            else if (IsBidPriceTopChanged) BooleanFields ^= PQBooleanValues.IsBidPriceTopChangedSetFlag;
        }
    }

    [JsonIgnore]
    public bool IsAskPriceTopChanged
    {
        get => BooleanFields.HasAskTopPriceChangedSet();
        set
        {
            IsAskPriceTopChangedUpdated |= value != IsAskPriceTopChanged  || NumOfUpdates == 0;
            if (value)
                BooleanFields |= PQBooleanValues.IsAskPriceTopChangedSetFlag;

            else if (IsAskPriceTopChanged) BooleanFields ^= PQBooleanValues.IsAskPriceTopChangedSetFlag;
        }
    }
    [JsonIgnore]
    public bool IsBidPriceTopChangedUpdated
    {
        get => BooleanFields.HasBidTopPriceChangedUpdated();
        set
        {
            if (value)
                BooleanFields |= PQBooleanValues.IsBidPriceTopChangedUpdatedFlag;

            else if (IsBidPriceTopChangedUpdated) BooleanFields ^= PQBooleanValues.IsBidPriceTopChangedUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAskPriceTopChangedUpdated
    {
        get => BooleanFields.HasAskTopPriceChangedUpdated();
        set
        {
            if (value)
                BooleanFields |= PQBooleanValues.IsAskPriceTopChangedUpdatedFlag;

            else if (IsAskPriceTopChangedUpdated) BooleanFields ^= PQBooleanValues.IsAskPriceTopChangedUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidFromTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ValidFromDateUpdatedFlag) > 0 && ValidFrom != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidFromDateUpdatedFlag;

            else if (IsValidFromTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidFromDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidFromTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ValidFromTimeSubHourUpdatedFlag) > 0 && ValidFrom != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidFromTimeSubHourUpdatedFlag;

            else if (IsValidFromTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidFromTimeSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidToTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ValidToDateUpdatedFlag) > 0 && ValidTo != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidToDateUpdatedFlag;

            else if (IsValidToTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidToDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidToTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ValidToSubHourUpdatedFlag) > 0 && ValidTo != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidToSubHourUpdatedFlag;

            else if (IsValidToTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidToSubHourUpdatedFlag;
        }
    }

    [JsonIgnore] public override TickerDetailLevel TickerDetailLevel => TickerDetailLevel.Level1Quote;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override DateTime SourceTime
    {
        get =>
            new(Math.Max(base.SourceTime.Ticks,
                         Math.Max(sourceBidTime.Ticks, sourceAskTime.Ticks)));
        set => base.SourceTime = value;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceAskTime
    {
        get => sourceAskTime;
        set
        {
            IsSourceAskTimeDateUpdated    |= sourceAskTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch() || NumOfUpdates == 0;
            IsSourceAskTimeSubHourUpdated |= sourceAskTime.GetSubHourComponent() != value.GetSubHourComponent() || NumOfUpdates == 0;
            sourceAskTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceBidTime
    {
        get => sourceBidTime;
        set
        {
            IsSourceBidTimeDateUpdated    |= sourceBidTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch() || NumOfUpdates == 0;
            IsSourceBidTimeSubHourUpdated |= sourceBidTime.GetSubHourComponent() != value.GetSubHourComponent() || NumOfUpdates == 0;
            sourceBidTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime AdapterSentTime
    {
        get => adapterSentTime;
        set
        {
            IsAdapterSentTimeDateUpdated |= adapterSentTime.GetHoursFromUnixEpoch()
             != value.GetHoursFromUnixEpoch() || NumOfUpdates == 0;
            IsAdapterSentTimeSubHourUpdated |= adapterSentTime.GetSubHourComponent()
             != value.GetSubHourComponent() || NumOfUpdates == 0;
            adapterSentTime = value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime AdapterReceivedTime
    {
        get => adapterReceivedTime;
        set
        {
            IsAdapterReceivedTimeDateUpdated |= adapterReceivedTime.GetHoursFromUnixEpoch()
             != value.GetHoursFromUnixEpoch() || NumOfUpdates == 0;
            IsAdapterReceivedTimeSubHourUpdated |= adapterReceivedTime.GetSubHourComponent()
             != value.GetSubHourComponent() || NumOfUpdates == 0;
            adapterReceivedTime = value == DateTime.UnixEpoch ? default : value;
        }
    }

    public BidAskPair BidAskTop => new(BidPriceTop, AskPriceTop);

    [JsonIgnore]
    public virtual decimal BidPriceTop
    {
        get => bidPriceTop;
        set
        {
            IsBidPriceTopUpdated |= bidPriceTop != value || NumOfUpdates == 0;
            IsBidPriceTopChanged = bidPriceTop != value;
            bidPriceTop          =  value;
        }
    }

    [JsonIgnore]
    public bool IsBidPriceTopUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.BidTopPriceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.BidTopPriceUpdatedFlag;

            else if (IsBidPriceTopUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.BidTopPriceUpdatedFlag;
        }
    }

    [JsonIgnore]
    public virtual decimal AskPriceTop
    {
        get => askPriceTop;
        set
        {
            IsAskPriceTopUpdated |= askPriceTop != value || NumOfUpdates == 0;
            IsAskPriceTopChanged = askPriceTop != value;
            askPriceTop          =  value;
        }
    }

    [JsonIgnore]
    public bool IsAskPriceTopUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AskTopPriceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AskTopPriceUpdatedFlag;

            else if (IsAskPriceTopUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.AskTopPriceUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable
    {
        get => (BooleanFields & PQBooleanValues.IsExecutableSetFlag) > 0;
        set
        {
            IsExecutableUpdated |= Executable != value || NumOfUpdates == 0;
            if (value)
                BooleanFields |= PQBooleanValues.IsExecutableSetFlag;

            else if (Executable) BooleanFields ^= PQBooleanValues.IsExecutableSetFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidFrom
    {
        get => validFromTime;
        set
        {
            IsValidFromTimeDateUpdated    |= validFromTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch() || NumOfUpdates == 0;
            IsValidFromTimeSubHourUpdated |= validFromTime.GetSubHourComponent() != value.GetSubHourComponent() || NumOfUpdates == 0;
            validFromTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidTo
    {
        get => validToTime;
        set
        {
            IsValidToTimeDateUpdated    |= validToTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch() || NumOfUpdates == 0;
            IsValidToTimeSubHourUpdated |= validToTime.GetSubHourComponent() != value.GetSubHourComponent() || NumOfUpdates == 0;
            validToTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore] IPricePeriodSummary? ILevel1Quote.SummaryPeriod => SummaryPeriod;

    [JsonIgnore]
    IMutablePricePeriodSummary? IMutableLevel1Quote.SummaryPeriod
    {
        get => SummaryPeriod;
        set => SummaryPeriod = value as IPQPricePeriodSummary;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IPQPricePeriodSummary? SummaryPeriod { get; set; }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || (SummaryPeriod?.HasUpdates ?? false);
        set
        {
            if (SummaryPeriod != null) SummaryPeriod.HasUpdates = value;
            base.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        SummaryPeriod?.UpdateComplete();
        base.UpdateComplete();
    }

    public override void IncrementTimeBy(TimeSpan toChangeBy)
    {
        base.IncrementTimeBy(toChangeBy);
        AdapterReceivedTime += toChangeBy;
        AdapterSentTime     += toChangeBy;
        SourceBidTime       += toChangeBy;
        SourceAskTime       += toChangeBy;
        ValidFrom           += toChangeBy;
        ValidTo             += toChangeBy;
    }

    public override void ResetFields()
    {
        BooleanFields = PQBooleanValues.IsExecutableSetFlag;

        adapterSentTime     = default;
        adapterReceivedTime = default;
        sourceAskTime       = default;
        sourceBidTime       = default;
        validFromTime       = default;
        validToTime         = default;

        IsBidPriceTopUpdated = IsAskPriceTopUpdated = false;

        bidPriceTop = askPriceTop = 0m;

        if (SummaryPeriod != null) SummaryPeriod.IsEmpty = true;
        base.ResetFields();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var precisionSettings = quotePublicationPrecisionSettings ?? PQSourceTickerInfo;
        var updatedOnly       = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, messageFlags, precisionSettings)) yield return updatedField;

        if (!IsReplay) AdapterSentTime = snapShotTime;
        if (!updatedOnly || IsAdapterSentTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.AdapterSentDateTime, adapterSentTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsAdapterSentTimeSubHourUpdated)
        {
            var extended = adapterSentTime.GetSubHourComponent().BreakLongToUShortAndUint(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.AdapterSentSubHourTime, value, extended);
        }
        if (!updatedOnly || IsAdapterReceivedTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.AdapterReceivedDateTime, adapterReceivedTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsAdapterReceivedTimeSubHourUpdated)
        {
            var extended = adapterReceivedTime.GetSubHourComponent().BreakLongToUShortAndUint(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.AdapterReceivedSubHourTime, value, extended);
        }
        if (!updatedOnly || IsSourceBidTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.SourceBidDateTime, sourceBidTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsSourceBidTimeSubHourUpdated)
        {
            var extended = sourceBidTime.GetSubHourComponent().BreakLongToUShortAndUint(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.SourceBidSubHourTime, value, extended);
        }
        if (!updatedOnly || IsSourceAskTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.SourceAskDateTime, sourceAskTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsSourceAskTimeSubHourUpdated)
        {
            var extended = sourceAskTime.GetSubHourComponent().BreakLongToUShortAndUint(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.SourceAskSubHourTime, value, extended);
        }
        if (!updatedOnly || IsValidFromTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.QuoteValidFromDate, validFromTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsValidFromTimeSubHourUpdated)
        {
            var extended = validFromTime.GetSubHourComponent().BreakLongToUShortAndUint(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.QuoteValidFromSubHourTime, value, extended);
        }
        if (!updatedOnly || IsValidToTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.QuoteValidToDate, validToTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsValidToTimeSubHourUpdated)
        {
            var extended = validToTime.GetSubHourComponent().BreakLongToUShortAndUint(out var value);
            yield return new PQFieldUpdate(PQQuoteFields.QuoteValidToSubHourTime, value, extended);
        }

        foreach (var updatedField in GetDeltaUpdateTopBookPriceFields(snapShotTime, updatedOnly, precisionSettings)) yield return updatedField;
        if (SummaryPeriod != null)
            foreach (var periodSummaryUpdates in SummaryPeriod.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                                    precisionSettings))
                yield return periodSummaryUpdates;
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id >= PQQuoteFields.SummaryPeriod &&
            pqFieldUpdate.Id <= PQQuoteFields.PeriodAveragePrice)
        {
            SummaryPeriod ??= new PQPricePeriodSummary();
            return SummaryPeriod.UpdateField(pqFieldUpdate);
        }
        switch (pqFieldUpdate.Id)
        {
            case PQQuoteFields.AdapterSentDateTime:
                IsAdapterSentTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterSentTime, pqFieldUpdate.Payload);
                if (adapterSentTime == DateTime.UnixEpoch) adapterSentTime = default;
                return 0;
            case PQQuoteFields.AdapterSentSubHourTime:
                IsAdapterSentTimeSubHourUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSubHourComponent
                    (ref adapterSentTime, pqFieldUpdate.ExtendedPayload.AppendUintToMakeLong(pqFieldUpdate.Payload));
                if (adapterSentTime == DateTime.UnixEpoch) adapterSentTime = default;
                return 0;
            case PQQuoteFields.AdapterReceivedDateTime:
                IsAdapterReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterReceivedTime, pqFieldUpdate.Payload);
                if (adapterReceivedTime == DateTime.UnixEpoch) adapterReceivedTime = default;
                return 0;
            case PQQuoteFields.AdapterReceivedSubHourTime:
                IsAdapterReceivedTimeSubHourUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSubHourComponent
                    (ref adapterReceivedTime, pqFieldUpdate.ExtendedPayload.AppendUintToMakeLong(pqFieldUpdate.Payload));
                if (adapterReceivedTime == DateTime.UnixEpoch) adapterReceivedTime = default;
                return 0;
            case PQQuoteFields.SourceBidDateTime:
                IsSourceBidTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceBidTime, pqFieldUpdate.Payload);
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
                return 0;
            case PQQuoteFields.SourceBidSubHourTime:
                IsSourceBidTimeSubHourUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSubHourComponent
                    (ref sourceBidTime, pqFieldUpdate.ExtendedPayload.AppendUintToMakeLong(pqFieldUpdate.Payload));
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
                return 0;
            case PQQuoteFields.SourceAskDateTime:
                IsSourceAskTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceAskTime, pqFieldUpdate.Payload);
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
                return 0;
            case PQQuoteFields.SourceAskSubHourTime:
                IsSourceAskTimeSubHourUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSubHourComponent
                    (ref sourceAskTime, pqFieldUpdate.ExtendedPayload.AppendUintToMakeLong(pqFieldUpdate.Payload));
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
                return 0;
            case PQQuoteFields.QuoteValidFromDate:
                IsValidFromTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref validFromTime, pqFieldUpdate.Payload);
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
                return 0;
            case PQQuoteFields.QuoteValidFromSubHourTime:
                IsValidFromTimeSubHourUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSubHourComponent
                    (ref validFromTime, pqFieldUpdate.ExtendedPayload.AppendUintToMakeLong(pqFieldUpdate.Payload));
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
                return 0;
            case PQQuoteFields.QuoteValidToDate:
                IsValidToTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref validToTime, pqFieldUpdate.Payload);
                if (validToTime == DateTime.UnixEpoch) validToTime = default;
                return 0;
            case PQQuoteFields.QuoteValidToSubHourTime:
                IsValidToTimeSubHourUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSubHourComponent
                    (ref validToTime, pqFieldUpdate.ExtendedPayload.AppendUintToMakeLong(pqFieldUpdate.Payload));
                if (validToTime == DateTime.UnixEpoch) validToTime = default;
                return 0;
            case PQQuoteFields.Price:
                if (pqFieldUpdate.IsBid() && pqFieldUpdate.DepthId.KeyToDepth() == 0)
                {
                    // var previousBidPriceTopUpdatedChanged = IsBidPriceTopChangedUpdated;
                    // var previousBidPriceTopChanged        = IsBidPriceTopChanged;
                    bidPriceTop                 = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    // IsBidPriceTopChanged        = ((BooleanFields & PQBooleanValues.IsBidPriceTopChangedSetFlag) > 0);
                    // IsBidPriceTopChangedUpdated = true;
                    IsBidPriceTopUpdated        = true;
                }
                else if(pqFieldUpdate.IsAsk() && pqFieldUpdate.DepthId.KeyToDepth() == 0)
                {
                    // var previousAskPriceTopChangedUpdated = IsAskPriceTopChangedUpdated;
                    // var previousAskPriceTopChanged        = IsAskPriceTopChanged;
                    askPriceTop                 = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    // IsAskPriceTopChanged        = ((BooleanFields & PQBooleanValues.IsAskPriceTopChangedSetFlag) > 0);
                    // IsAskPriceTopChangedUpdated = true;
                    IsAskPriceTopUpdated        = true;
                }
                return 0;
            default: return base.UpdateField(pqFieldUpdate);
        }
    }

    IReusableObject<IBidAskInstant> ITransferState<IReusableObject<IBidAskInstant>>.CopyFrom
        (IReusableObject<IBidAskInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ILevel1Quote)source, copyMergeFlags);

    IBidAskInstant ITransferState<IBidAskInstant>.CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ILevel1Quote)source, copyMergeFlags);

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => Clone();

    ILevel1Quote ILevel1Quote.Clone() => Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => Clone();

    IPQLevel1Quote IPQLevel1Quote.Clone() => Clone();

    IBidAskInstant ICloneable<IBidAskInstant>.Clone() => Clone();

    bool IInterfacesComparable<IBidAskInstant>.AreEquivalent(IBidAskInstant? other, bool exactTypes)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var startTimeSame       = SourceTime.Equals(other.AtTime);
        var averageBidPriceSame = BidPriceTop == other.BidPrice;
        var averageAskPriceSame = AskPriceTop == other.AskPrice;

        var allAreSame = startTimeSame && averageBidPriceSame && averageAskPriceSame;
        return allAreSame;
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel1Quote otherL1)) return false;
        var baseSame            = base.AreEquivalent(otherL1, exactTypes);
        var adapterSentSame     = adapterSentTime.Equals(otherL1.AdapterSentTime);
        var adapterReceivedSame = adapterReceivedTime.Equals(otherL1.AdapterReceivedTime);
        var sourceAskTimeSame   = sourceAskTime.Equals(otherL1.SourceAskTime);
        var sourceBidTimeSame   = sourceBidTime.Equals(otherL1.SourceBidTime);
        var validFromTimeSame   = ValidFrom.Equals(otherL1.ValidFrom);
        var validToTimeSame     = ValidTo.Equals(otherL1.ValidTo);
        var executableSame      = Executable == otherL1.Executable;
        var bidPriceTopSame     = BidPriceTop == otherL1.BidPriceTop;
        var askPriceTopSame     = AskPriceTop == otherL1.AskPriceTop;
        var bidPriceTopChange   = IsBidPriceTopChanged == otherL1.IsBidPriceTopChanged;
        var askPriceTopChange   = IsAskPriceTopChanged == otherL1.IsAskPriceTopChanged;
        var periodSummarySame
            = ((SummaryPeriod == null || SummaryPeriod.IsEmpty) && (otherL1.SummaryPeriod == null || otherL1.SummaryPeriod.IsEmpty)) ||
              (SummaryPeriod?.AreEquivalent(otherL1.SummaryPeriod, exactTypes) ?? otherL1.SummaryPeriod == null);

        var isExecutableUpdatedSame = true;

        if (exactTypes && otherL1 is IPQLevel1Quote pqL1) isExecutableUpdatedSame = IsExecutableUpdated == pqL1.IsExecutableUpdated;

        var allAreSame = baseSame && adapterSentSame && adapterReceivedSame && sourceAskTimeSame && validFromTimeSame && validToTimeSame
                      && sourceBidTimeSame && executableSame && bidPriceTopSame && askPriceTopSame && bidPriceTopChange && askPriceTopChange
                      && periodSummarySame && isExecutableUpdatedSame;
        return allAreSame;
    }

    IPQLevel1Quote IPQLevel1Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQLevel1Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IPQLevel1Quote pq1)
        {
            // between types only copy the changed parts not everything.
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pq1.IsAdapterSentTimeDateUpdated || isFullReplace)
            {
                var originalAdapterSentTime = adapterSentTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterSentTime,
                                                           pq1.AdapterSentTime.GetHoursFromUnixEpoch());
                IsAdapterSentTimeDateUpdated = originalAdapterSentTime != adapterSentTime;
                if (adapterSentTime == DateTime.UnixEpoch) adapterSentTime = default;
            }
            if (pq1.IsAdapterSentTimeSubHourUpdated || isFullReplace)
            {
                var originalAdapterSentTime = adapterSentTime;
                PQFieldConverters.UpdateSubHourComponent(ref adapterSentTime,
                                                         pq1.AdapterSentTime.GetSubHourComponent());
                IsAdapterSentTimeSubHourUpdated = originalAdapterSentTime != adapterSentTime;
                if (adapterSentTime == DateTime.UnixEpoch) adapterSentTime = default;
            }
            if (pq1.IsAdapterReceivedTimeDateUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = adapterReceivedTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterReceivedTime,
                                                           pq1.AdapterReceivedTime.GetHoursFromUnixEpoch());
                IsAdapterReceivedTimeDateUpdated = originalAdapterReceivedTime != adapterReceivedTime;
                if (adapterReceivedTime == DateTime.UnixEpoch) adapterReceivedTime = default;
            }
            if (pq1.IsAdapterReceivedTimeSubHourUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = adapterReceivedTime;
                PQFieldConverters.UpdateSubHourComponent(ref adapterReceivedTime,
                                                         pq1.AdapterReceivedTime.GetSubHourComponent());
                IsAdapterReceivedTimeSubHourUpdated = originalAdapterReceivedTime != adapterReceivedTime;
                if (adapterReceivedTime == DateTime.UnixEpoch) adapterReceivedTime = default;
            }
            if (pq1.IsSourceBidTimeDateUpdated || isFullReplace)
            {
                var originalSourceBidTime = sourceBidTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceBidTime,
                                                           pq1.SourceBidTime.GetHoursFromUnixEpoch());
                IsSourceBidTimeDateUpdated = originalSourceBidTime != sourceBidTime;
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
            }
            if (pq1.IsSourceBidTimeSubHourUpdated || isFullReplace)
            {
                var originalSourceBidTime = sourceBidTime;
                PQFieldConverters.UpdateSubHourComponent(ref sourceBidTime,
                                                         pq1.SourceBidTime.GetSubHourComponent());
                IsSourceBidTimeSubHourUpdated = originalSourceBidTime != sourceBidTime;
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
            }
            if (pq1.IsSourceAskTimeDateUpdated || isFullReplace)
            {
                var originalSourceAskTime = sourceAskTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceAskTime,
                                                           pq1.SourceAskTime.GetHoursFromUnixEpoch());
                IsSourceAskTimeDateUpdated = originalSourceAskTime != sourceAskTime;
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
            }
            if (pq1.IsSourceAskTimeSubHourUpdated || isFullReplace)
            {
                var originalSourceAskTime = sourceAskTime;
                PQFieldConverters.UpdateSubHourComponent(ref sourceAskTime,
                                                         pq1.SourceAskTime.GetSubHourComponent());
                IsSourceAskTimeSubHourUpdated = originalSourceAskTime != sourceAskTime;
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
            }
            if (pq1.IsValidFromTimeDateUpdated || isFullReplace)
            {
                var originalValidFrom = validFromTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref validFromTime,
                                                           pq1.ValidFrom.GetHoursFromUnixEpoch());
                IsValidFromTimeDateUpdated = originalValidFrom != validFromTime;
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
            }
            if (pq1.IsValidFromTimeSubHourUpdated || isFullReplace)
            {
                var originalValidFrom = validFromTime;
                PQFieldConverters.UpdateSubHourComponent(ref validFromTime,
                                                         pq1.ValidFrom.GetSubHourComponent());
                IsValidFromTimeSubHourUpdated = originalValidFrom != validFromTime;
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
            }
            if (pq1.IsValidToTimeDateUpdated || isFullReplace)
            {
                var originalValidTo = validToTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref validToTime,
                                                           pq1.ValidTo.GetHoursFromUnixEpoch());
                IsValidToTimeDateUpdated = originalValidTo != validToTime;
                if (validToTime == DateTime.UnixEpoch) validToTime = default;
            }
            if (pq1.IsValidToTimeSubHourUpdated || isFullReplace)
            {
                var originalValidTo = validToTime;
                PQFieldConverters.UpdateSubHourComponent(ref validToTime,
                                                         pq1.ValidTo.GetSubHourComponent());
                IsValidToTimeSubHourUpdated = originalValidTo != validToTime;
                if (validToTime == DateTime.UnixEpoch) validToTime = default;
            }
            if (pq1 is not IPQLevel2Quote)
            {
                if (pq1.IsBidPriceTopUpdated || isFullReplace)
                {
                    bidPriceTop          = pq1.BidPriceTop;
                    IsBidPriceTopUpdated = true;
                }
                if (pq1.IsAskPriceTopUpdated || isFullReplace)
                {
                    askPriceTop          = pq1.AskPriceTop;
                    IsAskPriceTopUpdated = true;
                }
                if (pq1.IsBidPriceTopChangedUpdated || isFullReplace)
                {
                    IsBidPriceTopChanged        = pq1.IsBidPriceTopChanged;
                    IsBidPriceTopChangedUpdated = true;
                }
                if (pq1.IsAskPriceTopChangedUpdated || isFullReplace)
                {
                    IsAskPriceTopChanged        = pq1.IsAskPriceTopChanged;
                    IsAskPriceTopChangedUpdated = true;
                }
            }

            if (pq1.IsExecutableUpdated || isFullReplace)
            {
                Executable = pq1.Executable;
                IsExecutableUpdated = true;
            }

            if (pq1.SummaryPeriod is { IsEmpty: false, HasUpdates: true })
            {
                SummaryPeriod ??= new PQPricePeriodSummary();
                SummaryPeriod.CopyFrom(pq1.SummaryPeriod);
            }
            else if (SummaryPeriod is { IsEmpty: false } && pq1.SummaryPeriod is { HasUpdates: true })
            {
                SummaryPeriod.IsEmpty = true;
            }
            // ensure flags still match source
            if (isFullReplace && pq1 is PQLevel1Quote pqLevel1Quote)
            {
                UpdatedFlags                = pqLevel1Quote.UpdatedFlags;
                IsBidPriceTopChangedUpdated = pq1.IsBidPriceTopChangedUpdated;
                IsAskPriceTopChangedUpdated = pq1.IsAskPriceTopChangedUpdated;
                IsExecutableUpdated = pq1.IsExecutableUpdated;
                IsReplayUpdated = pq1.IsReplayUpdated;
            }
        }
        else if (source is ILevel1Quote l1Q) // normal copy
        {
            IsExecutableUpdated         = Executable != l1Q.Executable;
            IsBidPriceTopChangedUpdated = IsBidPriceTopChanged != l1Q.IsBidPriceTopChanged;
            IsAskPriceTopChangedUpdated = IsAskPriceTopChanged != l1Q.IsAskPriceTopChanged;
            AdapterSentTime             = l1Q.AdapterSentTime;
            AdapterReceivedTime         = l1Q.AdapterReceivedTime;
            SourceBidTime               = l1Q.SourceBidTime;
            SourceAskTime               = l1Q.SourceAskTime;
            ValidFrom                   = l1Q.ValidFrom;
            ValidTo                     = l1Q.ValidTo;
            if (this is not ILevel2Quote)
            {
                BidPriceTop          = l1Q.BidPriceTop;
                AskPriceTop          = l1Q.AskPriceTop;
            }
            IsAskPriceTopChanged        = l1Q.IsAskPriceTopChanged;
            IsBidPriceTopChanged        = l1Q.IsBidPriceTopChanged;

            Executable = l1Q.Executable;

            if (l1Q.SummaryPeriod is { IsEmpty: false })
            {
                SummaryPeriod ??= new PQPricePeriodSummary();
                SummaryPeriod.CopyFrom(l1Q.SummaryPeriod);
            }
            else if (SummaryPeriod is { IsEmpty: false })
            {
                SummaryPeriod.IsEmpty = true;
            }
        }

        return this;
    }

    protected override bool IsBooleanFlagsChanged() =>
        base.IsBooleanFlagsChanged() || IsExecutableUpdated || IsBidPriceTopChangedUpdated || IsAskPriceTopChangedUpdated;

    protected override PQBooleanValues GenerateBooleanFlags(bool fullUpdate)
    {
        var resultSoFar = base.GenerateBooleanFlags(fullUpdate);
        return resultSoFar | (IsExecutableUpdated || fullUpdate ? PQBooleanValues.IsExecutableUpdatedFlag : PQBooleanValues.None)
                           | (Executable ? PQBooleanValues.IsExecutableSetFlag : PQBooleanValues.None)
                           | (IsBidPriceTopChangedUpdated || fullUpdate ? PQBooleanValues.IsBidPriceTopChangedUpdatedFlag : PQBooleanValues.None)
                           | (IsBidPriceTopChanged ? PQBooleanValues.IsBidPriceTopChangedSetFlag : PQBooleanValues.None)
                           | (IsAskPriceTopChangedUpdated || fullUpdate ? PQBooleanValues.IsAskPriceTopChangedUpdatedFlag : PQBooleanValues.None)
                           | (IsAskPriceTopChanged ? PQBooleanValues.IsAskPriceTopChangedSetFlag : PQBooleanValues.None);
    }

    protected override void SetBooleanFields(PQBooleanValues booleanFlags)
    {
        base.SetBooleanFields(booleanFlags);
        IsExecutableUpdated = (booleanFlags & PQBooleanValues.IsExecutableUpdatedFlag) > 0;
        Executable = (booleanFlags & PQBooleanValues.IsExecutableSetFlag) > 0;
        IsBidPriceTopChangedUpdated = booleanFlags.HasBidTopPriceChangedUpdated()
                                   || IsBidPriceTopChanged != booleanFlags.HasBidTopPriceChangedSet();
        IsBidPriceTopChanged = booleanFlags.HasBidTopPriceChangedSet();
        IsAskPriceTopChangedUpdated = booleanFlags.HasAskTopPriceChangedUpdated() 
                                   || IsAskPriceTopChanged != booleanFlags.HasAskTopPriceChangedSet();
        IsAskPriceTopChanged = booleanFlags.HasAskTopPriceChangedSet();
    }

    protected virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateTopBookPriceFields
    (DateTime snapShotTime,
        bool updatedOnly, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        if (!updatedOnly || IsBidPriceTopUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.Price, PQDepthKey.None, BidPriceTop
                                         , PQSourceTickerInfo!.PriceScalingPrecision);
        if (!updatedOnly || IsAskPriceTopUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.Price, PQDepthKey.AskSide, AskPriceTop
                                         , PQSourceTickerInfo!.PriceScalingPrecision);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ITickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ adapterSentTime.GetHashCode();
            hashCode = (hashCode * 397) ^ adapterReceivedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ sourceAskTime.GetHashCode();
            hashCode = (hashCode * 397) ^ sourceBidTime.GetHashCode();
            hashCode = (hashCode * 397) ^ validFromTime.GetHashCode();
            hashCode = (hashCode * 397) ^ validToTime.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            hashCode = (hashCode * 397) ^ bidPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ askPriceTop.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level1ToStringMembers}, {UpdatedFlagsToString})";
}
