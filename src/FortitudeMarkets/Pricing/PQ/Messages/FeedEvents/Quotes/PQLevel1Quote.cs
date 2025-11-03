// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

public interface IPQLevel1Quote : IPQTickInstant, IMutableLevel1Quote, ITrackableReset<IPQLevel1Quote>
{

    bool IsSourceAskTimeDateUpdated    { get; set; }
    bool IsSourceAskTimeSub2MinUpdated { get; set; }
    bool IsSourceBidTimeDateUpdated    { get; set; }
    bool IsSourceBidTimeSub2MinUpdated { get; set; }
    bool IsValidFromTimeDateUpdated    { get; set; }
    bool IsValidFromTimeSub2MinUpdated { get; set; }
    bool IsValidToTimeDateUpdated      { get; set; }
    bool IsValidToTimeSub2MinUpdated   { get; set; }

    bool IsBidPriceTopUpdated        { get; set; }
    bool IsAskPriceTopUpdated        { get; set; }
    bool IsBidPriceTopChangedUpdated { get; set; }
    bool IsAskPriceTopChangedUpdated { get; set; }
    bool IsExecutableUpdated         { get; set; }

    new bool AreEquivalent(ITickInstant? other, bool exactTypes = false);

    new IPQLevel1Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);
    new IPQLevel1Quote Clone();
    new IPQLevel1Quote ResetWithTracking();
}

public interface IPQPublishableLevel1Quote : IPQPublishableTickInstant, IMutablePublishableLevel1Quote, IPQLevel1Quote
  , IDoublyLinkedListNode<IPQPublishableLevel1Quote>, ITrackableReset<IPQPublishableLevel1Quote>
{
    new IPQPublishableLevel1Quote? Next     { get; set; }
    new IPQPublishableLevel1Quote? Previous { get; set; }

    new IPQCandle? ConflatedTicksCandle { get; set; }

    new IPQLevel1Quote AsNonPublishable { get; }

    new bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false);

    new IPQPublishableLevel1Quote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags);
    new IPQPublishableLevel1Quote Clone();
    new IPQPublishableLevel1Quote ResetWithTracking();
}

public class PQLevel1Quote : PQTickInstant, IPQLevel1Quote, ICloneable<PQLevel1Quote>
{
    private decimal askPriceTop;
    private decimal bidPriceTop;

    private DateTime sourceAskTime;
    private DateTime sourceBidTime;

    private DateTime validFromTime;
    private DateTime validToTime;

    public PQLevel1Quote()
    {
        QuoteBooleanFields |= PQQuoteBooleanValues.IsExecutableSetFlag;

        if (GetType() == typeof(PQLevel1Quote)) SequenceId = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQLevel1Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    public PQLevel1Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, decimal bidPriceTop = 0m, decimal askPriceTop = 0m
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null
      , DateTime? validFrom = null, DateTime? validTo = null, bool executable = true, decimal singlePrice = 0m)
        : base(sourceTickerInfo, singlePrice, sourceTime)
    {
        if (this is not ILevel2Quote)
        {
            BidPriceTop = bidPriceTop;
            AskPriceTop = askPriceTop;
        }
        SourceBidTime        = sourceBidTime ?? DateTime.MinValue;
        IsBidPriceTopChanged = isAskPriceTopChanged;
        SourceAskTime        = sourceAskTime ?? DateTime.MinValue;
        IsAskPriceTopChanged = isBidPriceTopChanged;
        ValidFrom            = validFrom ?? DateTime.MinValue;
        ValidTo              = validTo ?? DateTime.MinValue;
        Executable           = executable;
        IsExecutableUpdated  = !executable;

        if (GetType() == typeof(PQLevel1Quote)) SequenceId = 0;
    }

    public PQLevel1Quote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel1Quote l1QToClone)
        {
            SourceAskTime = l1QToClone.SourceAskTime;
            SourceBidTime = l1QToClone.SourceBidTime;
            validFromTime = l1QToClone.ValidFrom;
            validToTime   = l1QToClone.ValidTo;
            Executable    = l1QToClone.Executable;
            if (this is not ILevel2Quote)
            {
                bidPriceTop          = l1QToClone.BidPriceTop;
                askPriceTop          = l1QToClone.AskPriceTop;
                IsBidPriceTopChanged = l1QToClone.IsBidPriceTopChanged;
                IsAskPriceTopChanged = l1QToClone.IsAskPriceTopChanged;
            }
            if (toClone is IPQLevel1Quote ipqL1)
            {
                IsExecutableUpdated  = ipqL1.IsExecutableUpdated;
                IsBidPriceTopUpdated = ipqL1.IsBidPriceTopUpdated;
                IsAskPriceTopUpdated = ipqL1.IsAskPriceTopUpdated;
                IsBidPriceTopChanged = ipqL1.IsBidPriceTopChanged;
                IsAskPriceTopChanged = ipqL1.IsAskPriceTopChanged;

                IsSourceAskTimeDateUpdated    = ipqL1.IsSourceAskTimeDateUpdated;
                IsSourceAskTimeSub2MinUpdated = ipqL1.IsSourceAskTimeSub2MinUpdated;
                IsSourceBidTimeDateUpdated    = ipqL1.IsSourceBidTimeDateUpdated;
                IsSourceBidTimeSub2MinUpdated = ipqL1.IsSourceBidTimeSub2MinUpdated;
                IsValidFromTimeDateUpdated    = ipqL1.IsValidFromTimeDateUpdated;
                IsValidFromTimeSub2MinUpdated = ipqL1.IsValidFromTimeSub2MinUpdated;
                IsValidToTimeDateUpdated      = ipqL1.IsValidToTimeDateUpdated;
                IsValidToTimeSub2MinUpdated   = ipqL1.IsValidToTimeSub2MinUpdated;
            }
        }
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel1Quote)) SequenceId = 0;
    }

    public QuoteInstantBehaviorFlags QuoteBehavior { get; set; }

    [JsonIgnore] decimal IBidAskPair.BidPrice => BidPriceTop;

    decimal IBidAskPair.AskPrice => BidPriceTop;

    [JsonIgnore] DateTime IBidAskInstant.AtTime => SourceTime;


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
            IsSourceAskTimeDateUpdated |= sourceAskTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsSourceAskTimeSub2MinUpdated |= sourceAskTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            sourceAskTime = value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceBidTime
    {
        get => sourceBidTime;
        set
        {
            IsSourceBidTimeDateUpdated |= sourceBidTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsSourceBidTimeSub2MinUpdated |= sourceBidTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            sourceBidTime = value == DateTime.UnixEpoch ? default : value;
        }
    }

    public BidAskPair BidAskTop => new(BidPriceTop, AskPriceTop);

    [JsonIgnore]
    public virtual decimal BidPriceTop
    {
        get => bidPriceTop;
        set
        {
            IsBidPriceTopUpdated |= bidPriceTop != value || SequenceId == 0;
            IsBidPriceTopChanged =  bidPriceTop != value;
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
            IsAskPriceTopUpdated |= askPriceTop != value || SequenceId == 0;
            IsAskPriceTopChanged =  askPriceTop != value;
            askPriceTop          =  value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable
    {
        get => (QuoteBooleanFields & PQQuoteBooleanValues.IsExecutableSetFlag) > 0;
        set
        {
            IsExecutableUpdated |= Executable != value || SequenceId == 0;
            if (value)
                QuoteBooleanFields |= PQQuoteBooleanValues.IsExecutableSetFlag;

            else if (Executable) QuoteBooleanFields ^= PQQuoteBooleanValues.IsExecutableSetFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidFrom
    {
        get => validFromTime;
        set
        {
            IsValidFromTimeDateUpdated |= validFromTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsValidFromTimeSub2MinUpdated |= validFromTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            validFromTime = value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValidTo
    {
        get => validToTime;
        set
        {
            IsValidToTimeDateUpdated    |= validToTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsValidToTimeSub2MinUpdated |= validToTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            validToTime                 =  value == DateTime.UnixEpoch ? default : value;
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

    [JsonIgnore]
    public bool IsSourceAskTimeDateUpdated
    {
        get => !QuoteBehavior.HasNoSideDateTimesUpdatesFlag() && (UpdatedFlags & QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag;

            else if (IsSourceAskTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceAskTimeSub2MinUpdated
    {
        get => !QuoteBehavior.HasNoSideDateTimesUpdatesFlag() && (UpdatedFlags & QuoteFieldUpdatedFlags.SourceAskTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceAskTimeSub2MinUpdatedFlag;

            else if (IsSourceAskTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceAskTimeSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceBidTimeDateUpdated
    {
        get => !QuoteBehavior.HasNoSideDateTimesUpdatesFlag() && (UpdatedFlags & QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag;

            else if (IsSourceBidTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceBidTimeSub2MinUpdated
    {
        get => !QuoteBehavior.HasNoSideDateTimesUpdatesFlag() && (UpdatedFlags & QuoteFieldUpdatedFlags.SourceBidTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceBidTimeSub2MinUpdatedFlag;

            else if (IsSourceBidTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceBidTimeSub2MinUpdatedFlag;
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
        get => QuoteBooleanFields.HasBidTopPriceChangedSet();
        set
        {
            IsBidPriceTopChangedUpdated |= value != IsBidPriceTopChanged || SequenceId == 0;
            if (value)
                QuoteBooleanFields |= PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag;

            else if (IsBidPriceTopChanged) QuoteBooleanFields ^= PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag;
        }
    }

    [JsonIgnore]
    public bool IsAskPriceTopChanged
    {
        get => QuoteBooleanFields.HasAskTopPriceChangedSet();
        set
        {
            IsAskPriceTopChangedUpdated |= value != IsAskPriceTopChanged || SequenceId == 0;
            if (value)
                QuoteBooleanFields |= PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag;

            else if (IsAskPriceTopChanged) QuoteBooleanFields ^= PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag;
        }
    }
    [JsonIgnore]
    public bool IsBidPriceTopChangedUpdated
    {
        get => QuoteBooleanFields.HasBidTopPriceChangedUpdated();
        set
        {
            if (value)
                QuoteBooleanFields |= PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag;

            else if (IsBidPriceTopChangedUpdated) QuoteBooleanFields ^= PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAskPriceTopChangedUpdated
    {
        get => QuoteBooleanFields.HasAskTopPriceChangedUpdated();
        set
        {
            if (value)
                QuoteBooleanFields |= PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag;

            else if (IsAskPriceTopChangedUpdated) QuoteBooleanFields ^= PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidFromTimeDateUpdated
    {
        get =>
            !QuoteBehavior.HasNoValidDateTimeUpdatesFlag()
         && (UpdatedFlags & QuoteFieldUpdatedFlags.ValidFromDateUpdatedFlag) > 0 && ValidFrom != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidFromDateUpdatedFlag;

            else if (IsValidFromTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidFromDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidFromTimeSub2MinUpdated
    {
        get =>
            !QuoteBehavior.HasNoValidDateTimeUpdatesFlag()
         && (UpdatedFlags & QuoteFieldUpdatedFlags.ValidFromTimeSub2MinUpdatedFlag) > 0 && ValidFrom != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidFromTimeSub2MinUpdatedFlag;

            else if (IsValidFromTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidFromTimeSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidToTimeDateUpdated
    {
        get =>
            !QuoteBehavior.HasNoValidDateTimeUpdatesFlag()
         && (UpdatedFlags & QuoteFieldUpdatedFlags.ValidToDateUpdatedFlag) > 0 && ValidTo != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidToDateUpdatedFlag;

            else if (IsValidToTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidToDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsValidToTimeSub2MinUpdated
    {
        get =>
            !QuoteBehavior.HasNoValidDateTimeUpdatesFlag()
         && (UpdatedFlags & QuoteFieldUpdatedFlags.ValidToSub2MinUpdatedFlag) > 0 && ValidTo != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValidToSub2MinUpdatedFlag;

            else if (IsValidToTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValidToSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Previous { get; set; }
    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Next     { get; set; }

    public override void IncrementTimeBy(TimeSpan toChangeBy)
    {
        base.IncrementTimeBy(toChangeBy);

        SourceBidTime += toChangeBy;
        SourceAskTime += toChangeBy;
        ValidFrom     += toChangeBy;
        ValidTo       += toChangeBy;
    }

    IMutableLevel1Quote ITrackableReset<IMutableLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel1Quote IMutableLevel1Quote.ResetWithTracking() => ResetWithTracking();

    IPQLevel1Quote ITrackableReset<IPQLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IPQLevel1Quote IPQLevel1Quote.ResetWithTracking() => ResetWithTracking();

    public override PQLevel1Quote ResetWithTracking()
    {
        QuoteBooleanFields = PQQuoteBooleanValues.IsExecutableSetFlag;

        sourceAskTime = default;
        sourceBidTime = default;
        validFromTime = default;
        validToTime   = default;

        // Executable updated when BooleanFields flags are reset in base

        IsBidPriceTopUpdated = IsAskPriceTopUpdated = false;

        bidPriceTop = askPriceTop = 0m;

        base.ResetWithTracking();
        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var precisionSettings = quotePublicationPrecisionSettings;
        var fullPicture       = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) > 0;
        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, messageFlags, precisionSettings)) yield return updatedField;

        var allowSideTimes  = !QuoteBehavior.HasNoSideDateTimesUpdatesFlag();
        var allowValidTimes = !QuoteBehavior.HasNoValidDateTimeUpdatesFlag();
        if (allowSideTimes && fullPicture || IsSourceBidTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteBidDateTime, sourceBidTime.Get2MinIntervalsFromUnixEpoch());
        if (allowSideTimes && fullPicture || IsSourceBidTimeSub2MinUpdated)
        {
            var extended = sourceBidTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteBidSub2MinTime, value, extended);
        }
        if (allowSideTimes && fullPicture || IsSourceAskTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteAskDateTime, sourceAskTime.Get2MinIntervalsFromUnixEpoch());
        if (allowSideTimes && fullPicture || IsSourceAskTimeSub2MinUpdated)
        {
            var extended = sourceAskTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteAskSub2MinTime, value, extended);
        }
        if (allowValidTimes && fullPicture || IsValidFromTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteValidFromDate, validFromTime.Get2MinIntervalsFromUnixEpoch());
        if (allowValidTimes && fullPicture || IsValidFromTimeSub2MinUpdated)
        {
            var extended = validFromTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteValidFromSub2MinTime, value, extended);
        }
        if (allowValidTimes && fullPicture || IsValidToTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteValidToDate, validToTime.Get2MinIntervalsFromUnixEpoch());
        if (allowValidTimes && fullPicture || IsValidToTimeSub2MinUpdated)
        {
            var extended = validToTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.QuoteValidToSub2MinTime, value, extended);
        }

        foreach (var updatedField in GetDeltaUpdateTopBookPriceFields(snapShotTime, fullPicture, precisionSettings)) yield return updatedField;
    }

    protected virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateTopBookPriceFields
        (DateTime snapShotTime, bool fullPicture, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        if (fullPicture || IsBidPriceTopUpdated)
            yield return new PQFieldUpdate
                (PQFeedFields.QuoteLayerPrice, PQDepthKey.None, BidPriceTop
               , quotePublicationPrecisionSettings?.PriceScalingPrecision ?? PriceScalingPrecision);
        if (fullPicture || IsAskPriceTopUpdated)
            yield return new PQFieldUpdate
                (PQFeedFields.QuoteLayerPrice, PQDepthKey.AskSide, AskPriceTop
               , quotePublicationPrecisionSettings?.PriceScalingPrecision ?? PriceScalingPrecision);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.SourceQuoteBidDateTime:
                if (QuoteBehavior.HasNoSideDateTimesUpdatesFlag()) return 0;
                IsSourceBidTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceBidTime, pqFieldUpdate.Payload);
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
                return 0;
            case PQFeedFields.SourceQuoteBidSub2MinTime:
                if (QuoteBehavior.HasNoSideDateTimesUpdatesFlag()) return 0;
                IsSourceBidTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref sourceBidTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
                return 0;
            case PQFeedFields.SourceQuoteAskDateTime:
                if (QuoteBehavior.HasNoSideDateTimesUpdatesFlag()) return 0;
                IsSourceAskTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceAskTime, pqFieldUpdate.Payload);
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
                return 0;
            case PQFeedFields.SourceQuoteAskSub2MinTime:
                if (QuoteBehavior.HasNoSideDateTimesUpdatesFlag()) return 0;
                IsSourceAskTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref sourceAskTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
                return 0;
            case PQFeedFields.QuoteValidFromDate:
                if (QuoteBehavior.HasNoValidDateTimeUpdatesFlag()) return 0;
                IsValidFromTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref validFromTime, pqFieldUpdate.Payload);
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
                return 0;
            case PQFeedFields.QuoteValidFromSub2MinTime:
                if (QuoteBehavior.HasNoValidDateTimeUpdatesFlag()) return 0;
                IsValidFromTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref validFromTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
                return 0;
            case PQFeedFields.QuoteValidToDate:
                if (QuoteBehavior.HasNoValidDateTimeUpdatesFlag()) return 0;
                IsValidToTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref validToTime, pqFieldUpdate.Payload);
                if (validToTime == DateTime.UnixEpoch) validToTime = default;
                return 0;
            case PQFeedFields.QuoteValidToSub2MinTime:
                if (QuoteBehavior.HasNoValidDateTimeUpdatesFlag()) return 0;
                IsValidToTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref validToTime, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (validToTime == DateTime.UnixEpoch) validToTime = default;
                return 0;
            case PQFeedFields.QuoteLayerPrice:
                if (pqFieldUpdate.IsBid() && pqFieldUpdate.DepthId.KeyToDepth() == 0)
                {
                    bidPriceTop          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsBidPriceTopUpdated = true;
                }
                else if (pqFieldUpdate.IsAsk() && pqFieldUpdate.DepthId.KeyToDepth() == 0)
                {
                    askPriceTop          = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                    IsAskPriceTopUpdated = true;
                }
                return 0;
            default: return base.UpdateField(pqFieldUpdate);
        }
    }

    protected override bool IsBooleanFlagsChanged() =>
        base.IsBooleanFlagsChanged() || IsExecutableUpdated || IsBidPriceTopChangedUpdated || IsAskPriceTopChangedUpdated;

    protected override PQQuoteBooleanValues GenerateBooleanFlags(bool fullUpdate)
    {
        var resultSoFar = base.GenerateBooleanFlags(fullUpdate);
        return resultSoFar | (IsExecutableUpdated || fullUpdate ? PQQuoteBooleanValues.IsExecutableUpdatedFlag : PQQuoteBooleanValues.None)
                           | (Executable ? PQQuoteBooleanValues.IsExecutableSetFlag : PQQuoteBooleanValues.None)
                           | (IsBidPriceTopChangedUpdated || fullUpdate
                                 ? PQQuoteBooleanValues.IsBidPriceTopChangedUpdatedFlag
                                 : PQQuoteBooleanValues.None)
                           | (IsBidPriceTopChanged ? PQQuoteBooleanValues.IsBidPriceTopChangedSetFlag : PQQuoteBooleanValues.None)
                           | (IsAskPriceTopChangedUpdated || fullUpdate
                                 ? PQQuoteBooleanValues.IsAskPriceTopChangedUpdatedFlag
                                 : PQQuoteBooleanValues.None)
                           | (IsAskPriceTopChanged ? PQQuoteBooleanValues.IsAskPriceTopChangedSetFlag : PQQuoteBooleanValues.None);
    }

    protected override void SetBooleanFields(PQQuoteBooleanValues quoteBooleanFlags)
    {
        base.SetBooleanFields(quoteBooleanFlags);
        IsExecutableUpdated = (quoteBooleanFlags & PQQuoteBooleanValues.IsExecutableUpdatedFlag) > 0;
        Executable          = (quoteBooleanFlags & PQQuoteBooleanValues.IsExecutableSetFlag) > 0;
        IsBidPriceTopChangedUpdated = quoteBooleanFlags.HasBidTopPriceChangedUpdated()
                                   || IsBidPriceTopChanged != quoteBooleanFlags.HasBidTopPriceChangedSet();
        IsBidPriceTopChanged = quoteBooleanFlags.HasBidTopPriceChangedSet();
        IsAskPriceTopChangedUpdated = quoteBooleanFlags.HasAskTopPriceChangedUpdated()
                                   || IsAskPriceTopChanged != quoteBooleanFlags.HasAskTopPriceChangedSet();
        IsAskPriceTopChanged = quoteBooleanFlags.HasAskTopPriceChangedSet();
    }

    IBidAskInstant ICloneable<IBidAskInstant>.Clone() => Clone();

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => Clone();

    ILevel1Quote ILevel1Quote.Clone() => Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => Clone();

    IPQLevel1Quote IPQLevel1Quote.Clone() => Clone();

    public override PQLevel1Quote Clone() => Recycler?.Borrow<PQLevel1Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQLevel1Quote(this);

    IReusableObject<IBidAskInstant> ITransferState<IReusableObject<IBidAskInstant>>.CopyFrom
        (IReusableObject<IBidAskInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableLevel1Quote)source, copyMergeFlags);

    IBidAskInstant ITransferState<IBidAskInstant>.CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableLevel1Quote)source, copyMergeFlags);

    IPQLevel1Quote IPQLevel1Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQLevel1Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IPQLevel1Quote pq1)
        {
            // between types only copy the changed parts not everything.
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pq1.IsSourceBidTimeDateUpdated || isFullReplace)
            {
                var originalSourceBidTime = sourceBidTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceBidTime,
                                                                      pq1.SourceBidTime.Get2MinIntervalsFromUnixEpoch());
                IsSourceBidTimeDateUpdated |= originalSourceBidTime != sourceBidTime;
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
            }
            if (pq1.IsSourceBidTimeSub2MinUpdated || isFullReplace)
            {
                var originalSourceBidTime = sourceBidTime;
                PQFieldConverters.UpdateSub2MinComponent(ref sourceBidTime,
                                                         pq1.SourceBidTime.GetSub2MinComponent());
                IsSourceBidTimeSub2MinUpdated |= originalSourceBidTime != sourceBidTime;
                if (sourceBidTime == DateTime.UnixEpoch) sourceBidTime = default;
            }
            if (pq1.IsSourceAskTimeDateUpdated || isFullReplace)
            {
                var originalSourceAskTime = sourceAskTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceAskTime,
                                                                      pq1.SourceAskTime.Get2MinIntervalsFromUnixEpoch());
                IsSourceAskTimeDateUpdated |= originalSourceAskTime != sourceAskTime;
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
            }
            if (pq1.IsSourceAskTimeSub2MinUpdated || isFullReplace)
            {
                var originalSourceAskTime = sourceAskTime;
                PQFieldConverters.UpdateSub2MinComponent(ref sourceAskTime,
                                                         pq1.SourceAskTime.GetSub2MinComponent());
                IsSourceAskTimeSub2MinUpdated |= originalSourceAskTime != sourceAskTime;
                if (sourceAskTime == DateTime.UnixEpoch) sourceAskTime = default;
            }
            if (pq1.IsValidFromTimeDateUpdated || isFullReplace)
            {
                var originalValidFrom = validFromTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref validFromTime,
                                                                      pq1.ValidFrom.Get2MinIntervalsFromUnixEpoch());
                IsValidFromTimeDateUpdated |= originalValidFrom != validFromTime;
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
            }
            if (pq1.IsValidFromTimeSub2MinUpdated || isFullReplace)
            {
                var originalValidFrom = validFromTime;
                PQFieldConverters.UpdateSub2MinComponent(ref validFromTime,
                                                         pq1.ValidFrom.GetSub2MinComponent());
                IsValidFromTimeSub2MinUpdated |= originalValidFrom != validFromTime;
                if (validFromTime == DateTime.UnixEpoch) validFromTime = default;
            }
            if (pq1.IsValidToTimeDateUpdated || isFullReplace)
            {
                var originalValidTo = validToTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref validToTime,
                                                                      pq1.ValidTo.Get2MinIntervalsFromUnixEpoch());
                IsValidToTimeDateUpdated |= originalValidTo != validToTime;
                if (validToTime == DateTime.UnixEpoch) validToTime = default;
            }
            if (pq1.IsValidToTimeSub2MinUpdated || isFullReplace)
            {
                var originalValidTo = validToTime;
                PQFieldConverters.UpdateSub2MinComponent(ref validToTime,
                                                         pq1.ValidTo.GetSub2MinComponent());
                IsValidToTimeSub2MinUpdated |= originalValidTo != validToTime;
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
                Executable          = pq1.Executable;
                IsExecutableUpdated = true;
            }

            // ensure flags still match source
            if (isFullReplace && pq1 is PQLevel1Quote pqLevel1Quote)
            {
                UpdatedFlags                = pqLevel1Quote.UpdatedFlags;
                IsBidPriceTopChangedUpdated = pq1.IsBidPriceTopChangedUpdated;
                IsAskPriceTopChangedUpdated = pq1.IsAskPriceTopChangedUpdated;
                IsExecutableUpdated         = pq1.IsExecutableUpdated;
            } else if (isFullReplace)
            {
                SetFlagsSame(pq1);
            }
        }
        else if (source is ILevel1Quote l1Q) // normal copy
        {
            IsExecutableUpdated         = Executable != l1Q.Executable;
            IsBidPriceTopChangedUpdated = IsBidPriceTopChanged != l1Q.IsBidPriceTopChanged;
            IsAskPriceTopChangedUpdated = IsAskPriceTopChanged != l1Q.IsAskPriceTopChanged;
            SourceBidTime               = l1Q.SourceBidTime;
            SourceAskTime               = l1Q.SourceAskTime;
            ValidFrom                   = l1Q.ValidFrom;
            ValidTo                     = l1Q.ValidTo;
            if (this is not ILevel2Quote)
            {
                BidPriceTop = l1Q.BidPriceTop;
                AskPriceTop = l1Q.AskPriceTop;
            }
            IsAskPriceTopChanged = l1Q.IsAskPriceTopChanged;
            IsBidPriceTopChanged = l1Q.IsBidPriceTopChanged;

            Executable = l1Q.Executable;
        }

        return this;
    }

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
        if (other is not IPQLevel1Quote && exactTypes) return false;
        if (other is not ILevel1Quote otherL1) return false;
        var baseSame          = base.AreEquivalent(other, exactTypes);
        var sourceAskTimeSame = sourceAskTime.Equals(otherL1.SourceAskTime);
        var sourceBidTimeSame = sourceBidTime.Equals(otherL1.SourceBidTime);
        var validFromTimeSame = ValidFrom.Equals(otherL1.ValidFrom);
        var validToTimeSame   = ValidTo.Equals(otherL1.ValidTo);
        var executableSame    = Executable == otherL1.Executable;
        var bidPriceTopSame   = BidPriceTop == otherL1.BidPriceTop;
        var askPriceTopSame   = AskPriceTop == otherL1.AskPriceTop;
        var bidPriceTopChange = IsBidPriceTopChanged == otherL1.IsBidPriceTopChanged;
        var askPriceTopChange = IsAskPriceTopChanged == otherL1.IsAskPriceTopChanged;

        var isExecutableUpdatedSame = true;

        if (exactTypes && other is IPQLevel1Quote pqL1Q) isExecutableUpdatedSame = IsExecutableUpdated == pqL1Q.IsExecutableUpdated;

        var allAreSame = baseSame && sourceAskTimeSame && validFromTimeSame && validToTimeSame
                      && sourceBidTimeSame && executableSame && bidPriceTopSame && askPriceTopSame && bidPriceTopChange && askPriceTopChange
                      && isExecutableUpdatedSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPublishableTickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
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

    public override string QuoteToStringMembers =>
        $"{base.QuoteToStringMembers}, {nameof(SourceTime)}: {SourceTime}, " +
        $"{nameof(SourceAskTime)}: {SourceAskTime}, {nameof(SourceBidTime)}: {SourceBidTime}, " +
        $"{nameof(BidPriceTop)}: {BidPriceTop}, {nameof(IsBidPriceTopChanged)}: {IsBidPriceTopChanged}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop}, {nameof(IsAskPriceTopChanged)}: {IsAskPriceTopChanged}, " +
        $"{nameof(Executable)}: {Executable}";

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {UpdatedFlagsToString})";
}

public class PQPublishableLevel1Quote : PQPublishableTickInstant, IPQPublishableLevel1Quote, ICloneable<PQPublishableLevel1Quote>
  , IDoublyLinkedListNode<PQPublishableLevel1Quote>
{
    public PQPublishableLevel1Quote() { }

    // Reflection invoked constructor (PQServer<T>)
    public PQPublishableLevel1Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    public PQPublishableLevel1Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, decimal bidPriceTop = 0m, decimal askPriceTop = 0m
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null
      , DateTime? validFrom = null
      , DateTime? validTo = null, bool executable = true, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None, decimal singlePrice = 0m
      , ICandle? conflationTicksCandle = null)
        : this(new PQLevel1Quote(sourceTickerInfo, sourceTime, bidPriceTop, askPriceTop, isBidPriceTopChanged, isAskPriceTopChanged
                               , sourceBidTime, sourceAskTime, validFrom, validTo, executable, singlePrice),
               sourceTickerInfo, feedSyncStatus, feedConnectivityStatus, conflationTicksCandle) { }

    protected PQPublishableLevel1Quote
    (IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , ICandle? conflationTicksCandle = null)
        : base(initializedQuoteContainer, sourceTickerInfo, feedSyncStatus, feedConnectivityStatus)
    {
        if (conflationTicksCandle is IPQCandle pqCandle)
        {
            ConflatedTicksCandle = pqCandle;
        }
        else
        {
            ConflatedTicksCandle = conflationTicksCandle != null ? new PQCandle(conflationTicksCandle) : new PQCandle();
        }
    }

    public PQPublishableLevel1Quote(IPublishableTickInstant toClone) : this(toClone, null) { }

    protected PQPublishableLevel1Quote
        (IPublishableTickInstant toClone, IPQTickInstant? initializedQuoteContainer) : base(toClone, initializedQuoteContainer)
    {
        if (toClone is IPublishableLevel1Quote l1QToClone)
        {
            if (l1QToClone.ConflatedTicksCandle is { IsEmpty: false }) ConflatedTicksCandle = new PQCandle(l1QToClone.ConflatedTicksCandle);
            if (toClone is IPQPublishableLevel1Quote ipqL1)
            {
                IsAdapterSentTimeDateUpdated        = ipqL1.IsAdapterSentTimeDateUpdated;
                IsAdapterSentTimeSub2MinUpdated     = ipqL1.IsAdapterSentTimeSub2MinUpdated;
                IsAdapterReceivedTimeDateUpdated    = ipqL1.IsAdapterReceivedTimeDateUpdated;
                IsAdapterReceivedTimeSub2MinUpdated = ipqL1.IsAdapterReceivedTimeSub2MinUpdated;
            }
        }
        SetFlagsSame(toClone);
    }

    protected override IPQLevel1Quote CreateEmptyQuoteContainerInstant() => new PQLevel1Quote();

    protected override IPQLevel1Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new PQLevel1Quote(tickInstant);

    protected override IPQLevel1Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new PQLevel1Quote(tickerInfo);


    ILevel1Quote IPublishableLevel1Quote.AsNonPublishable => AsNonPublishable;

    IMutableLevel1Quote IMutablePublishableLevel1Quote.AsNonPublishable => AsNonPublishable;

    public override IPQLevel1Quote AsNonPublishable => (IPQLevel1Quote)PQQuoteContainer;

    [JsonIgnore] decimal IBidAskPair.BidPrice => AsNonPublishable.BidPriceTop;

    decimal IBidAskPair.AskPrice => AsNonPublishable.AskPrice;

    [JsonIgnore] DateTime IBidAskInstant.AtTime => AsNonPublishable.AtTime;

    public BidAskPair BidAskTop => AsNonPublishable.BidAskTop;
    public DateTime SourceBidTime
    {
        get => AsNonPublishable.SourceBidTime;
        set => AsNonPublishable.SourceBidTime = value;
    }
    public decimal BidPriceTop
    {
        get => AsNonPublishable.BidPriceTop;
        set => AsNonPublishable.BidPriceTop = value;
    }
    public bool IsBidPriceTopChanged
    {
        get => AsNonPublishable.IsBidPriceTopChanged;
        set => AsNonPublishable.IsBidPriceTopChanged = value;
    }
    public DateTime SourceAskTime
    {
        get => AsNonPublishable.SourceAskTime;
        set => AsNonPublishable.SourceAskTime = value;
    }
    public decimal AskPriceTop
    {
        get => AsNonPublishable.AskPriceTop;
        set => AsNonPublishable.AskPriceTop = value;
    }
    public bool IsAskPriceTopChanged
    {
        get => AsNonPublishable.IsAskPriceTopChanged;
        set => AsNonPublishable.IsAskPriceTopChanged = value;
    }
    public bool Executable
    {
        get => AsNonPublishable.Executable;
        set => AsNonPublishable.Executable = value;
    }
    public DateTime ValidFrom
    {
        get => AsNonPublishable.ValidFrom;
        set => AsNonPublishable.ValidFrom = value;
    }
    public DateTime ValidTo
    {
        get => AsNonPublishable.ValidTo;
        set => AsNonPublishable.ValidTo = value;
    }
    public bool IsSourceAskTimeDateUpdated
    {
        get => AsNonPublishable.IsSourceAskTimeDateUpdated;
        set => AsNonPublishable.IsSourceAskTimeDateUpdated = value;
    }
    public bool IsSourceAskTimeSub2MinUpdated
    {
        get => AsNonPublishable.IsSourceAskTimeSub2MinUpdated;
        set => AsNonPublishable.IsSourceAskTimeSub2MinUpdated = value;
    }
    public bool IsSourceBidTimeDateUpdated
    {
        get => AsNonPublishable.IsSourceBidTimeDateUpdated;
        set => AsNonPublishable.IsSourceBidTimeDateUpdated = value;
    }
    public bool IsSourceBidTimeSub2MinUpdated
    {
        get => AsNonPublishable.IsSourceBidTimeSub2MinUpdated;
        set => AsNonPublishable.IsSourceBidTimeSub2MinUpdated = value;
    }
    public bool IsValidFromTimeDateUpdated
    {
        get => AsNonPublishable.IsValidFromTimeDateUpdated;
        set => AsNonPublishable.IsValidFromTimeDateUpdated = value;
    }
    public bool IsValidFromTimeSub2MinUpdated
    {
        get => AsNonPublishable.IsValidFromTimeSub2MinUpdated;
        set => AsNonPublishable.IsValidFromTimeSub2MinUpdated = value;
    }
    public bool IsValidToTimeDateUpdated
    {
        get => AsNonPublishable.IsValidToTimeDateUpdated;
        set => AsNonPublishable.IsValidToTimeDateUpdated = value;
    }
    public bool IsValidToTimeSub2MinUpdated
    {
        get => AsNonPublishable.IsValidToTimeSub2MinUpdated;
        set => AsNonPublishable.IsValidToTimeSub2MinUpdated = value;
    }
    public bool IsBidPriceTopUpdated
    {
        get => AsNonPublishable.IsBidPriceTopUpdated;
        set => AsNonPublishable.IsBidPriceTopUpdated = value;
    }
    public bool IsAskPriceTopUpdated
    {
        get => AsNonPublishable.IsAskPriceTopUpdated;
        set => AsNonPublishable.IsAskPriceTopUpdated = value;
    }
    public bool IsBidPriceTopChangedUpdated
    {
        get => AsNonPublishable.IsBidPriceTopChangedUpdated;
        set => AsNonPublishable.IsBidPriceTopChangedUpdated = value;
    }
    public bool IsAskPriceTopChangedUpdated
    {
        get => AsNonPublishable.IsAskPriceTopChangedUpdated;
        set => AsNonPublishable.IsAskPriceTopChangedUpdated = value;
    }
    public bool IsExecutableUpdated
    {
        get => AsNonPublishable.IsExecutableUpdated;
        set => AsNonPublishable.IsExecutableUpdated = value;
    }

    [JsonIgnore] public override TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.Level1Quote;

    [JsonIgnore] ICandle? IPublishableLevel1Quote.ConflatedTicksCandle => ConflatedTicksCandle;


    [JsonIgnore]
    IMutableCandle? IMutablePublishableLevel1Quote.ConflatedTicksCandle
    {
        get => ConflatedTicksCandle;
        set => ConflatedTicksCandle = value as IPQCandle;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IPQCandle? ConflatedTicksCandle { get; set; }

    QuoteInstantBehaviorFlags ILevel1Quote.QuoteBehavior => AsNonPublishable.QuoteBehavior;

    QuoteInstantBehaviorFlags IMutableLevel1Quote.QuoteBehavior
    {
        get => AsNonPublishable.QuoteBehavior;
        set => AsNonPublishable.QuoteBehavior = value;
    }

    public override PublishableQuoteInstantBehaviorFlags QuoteBehavior
    {
        get => base.QuoteBehavior;
        set
        {
            if (value.HasRestoreAndOverlayOriginalQuoteFlagsFlag())
            {
                AsNonPublishable.QuoteBehavior |= (QuoteInstantBehaviorFlags)value;
            }
            base.QuoteBehavior = value;
        }
    }

    [JsonIgnore]
    public new PQPublishableLevel1Quote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PQPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    public new PQPublishableLevel1Quote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PQPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableLevel1Quote? IPQPublishableLevel1Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableLevel1Quote? IPQPublishableLevel1Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IPublishableLevel1Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IPublishableLevel1Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IDoublyLinkedListNode<IPublishableLevel1Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel1Quote? IDoublyLinkedListNode<IPublishableLevel1Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQPublishableLevel1Quote? IDoublyLinkedListNode<IPQPublishableLevel1Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableLevel1Quote? IDoublyLinkedListNode<IPQPublishableLevel1Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQPublishableLevel1Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Previous { get; set; }

    [JsonIgnore] IBidAskInstant? IDoublyLinkedListNode<IBidAskInstant>.Next { get; set; }


    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || (ConflatedTicksCandle?.HasUpdates ?? false) || PQQuoteContainer.HasUpdates;
        set
        {
            if (ConflatedTicksCandle != null) ConflatedTicksCandle.HasUpdates = value;
            base.HasUpdates = value;
        }
    }

    public override void UpdateStarted(uint updateSequenceId = 0)
    {
        ConflatedTicksCandle?.UpdateStarted(updateSequenceId);
        base.UpdateStarted(updateSequenceId);
    }

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        ConflatedTicksCandle?.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
    }


    IMutableLevel1Quote ITrackableReset<IMutableLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel1Quote IMutableLevel1Quote.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel1Quote ITrackableReset<IMutablePublishableLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel1Quote IMutablePublishableLevel1Quote.ResetWithTracking() => ResetWithTracking();

    IPQPublishableLevel1Quote ITrackableReset<IPQPublishableLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IPQPublishableLevel1Quote IPQPublishableLevel1Quote.ResetWithTracking() => ResetWithTracking();

    IPQLevel1Quote ITrackableReset<IPQLevel1Quote>.ResetWithTracking() => ResetWithTracking();

    IPQLevel1Quote IPQLevel1Quote.ResetWithTracking() => ResetWithTracking();

    public override PQPublishableLevel1Quote ResetWithTracking()
    {
        if (ConflatedTicksCandle != null) ConflatedTicksCandle.StateReset();
        base.ResetWithTracking();

        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var precisionSettings = quotePublicationPrecisionSettings ?? PQSourceTickerInfo;

        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, messageFlags, precisionSettings)) yield return updatedField;
        if (ConflatedTicksCandle != null)
            foreach (var conflationTicksCandleUpdate in ConflatedTicksCandle.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                                                  precisionSettings))
                yield return conflationTicksCandleUpdate.WithFieldId(PQFeedFields.CandleConflationSummary);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQFeedFields.CandleConflationSummary)
        {
            ConflatedTicksCandle ??= new PQCandle();
            return ConflatedTicksCandle.UpdateField(pqFieldUpdate);
        }
        return base.UpdateField(pqFieldUpdate);
    }

    IReusableObject<IBidAskInstant> ITransferState<IReusableObject<IBidAskInstant>>.CopyFrom
        (IReusableObject<IBidAskInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    IBidAskInstant ITransferState<IBidAskInstant>.CopyFrom(IBidAskInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    IPublishableLevel1Quote ICloneable<IPublishableLevel1Quote>.Clone() => Clone();

    IPublishableLevel1Quote IPublishableLevel1Quote.Clone() => Clone();

    IMutablePublishableLevel1Quote IMutablePublishableLevel1Quote.Clone() => Clone();

    IPQPublishableLevel1Quote IPQPublishableLevel1Quote.Clone() => Clone();

    IBidAskInstant ICloneable<IBidAskInstant>.Clone() => Clone();

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => Clone();

    ILevel1Quote ILevel1Quote.Clone() => Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => Clone();

    IPQLevel1Quote IPQLevel1Quote.Clone() => Clone();

    public override PQPublishableLevel1Quote Clone() =>
        Recycler?.Borrow<PQPublishableLevel1Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQPublishableLevel1Quote(this, PQQuoteContainer.Clone());

    IPQLevel1Quote IPQLevel1Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPQMessage pubTickInstant)
        {
            CopyFrom(pubTickInstant, copyMergeFlags);
        }
        else
        {
            AsNonPublishable.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    IPQPublishableLevel1Quote IPQPublishableLevel1Quote.CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQPublishableLevel1Quote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IPQPublishableLevel1Quote pq1)
        {
            // between types only copy the changed parts not everything.
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pq1.ConflatedTicksCandle is { IsEmpty: false, HasUpdates: true })
            {
                ConflatedTicksCandle ??= new PQCandle();
                ConflatedTicksCandle.CopyFrom(pq1.ConflatedTicksCandle);
            }
            else if (ConflatedTicksCandle is { IsEmpty: false } && pq1.ConflatedTicksCandle is { HasUpdates: true })
            {
                ConflatedTicksCandle.IsEmpty = true;
            }
            // ensure flags still match source
            if (isFullReplace && pq1 is PQPublishableLevel1Quote pqLevel1Quote)
            {
                UpdatedFlags = pqLevel1Quote.UpdatedFlags;
            }
        }
        else if (source is IPublishableLevel1Quote l1Q) // normal copy
        {
            if (l1Q.ConflatedTicksCandle is { IsEmpty: false })
            {
                ConflatedTicksCandle ??= new PQCandle();
                ConflatedTicksCandle.CopyFrom(l1Q.ConflatedTicksCandle);
            }
            else if (ConflatedTicksCandle is { IsEmpty: false })
            {
                ConflatedTicksCandle.IsEmpty = true;
            }
        }

        return this;
    }

    bool IInterfacesComparable<IBidAskInstant>.AreEquivalent(IBidAskInstant? other, bool exactTypes) =>
        AreEquivalent(other as IPublishableLevel1Quote, exactTypes);

    public override bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (other is not IPublishableLevel1Quote otherL1) return false;
        var baseSame = base.AreEquivalent(otherL1, exactTypes);
        var conflationTicksCandleSame
            = ((ConflatedTicksCandle == null || ConflatedTicksCandle.IsEmpty) &&
               (otherL1.ConflatedTicksCandle == null || otherL1.ConflatedTicksCandle.IsEmpty)) ||
              (ConflatedTicksCandle?.AreEquivalent(otherL1.ConflatedTicksCandle, exactTypes) ?? otherL1.ConflatedTicksCandle == null);

        var allAreSame = baseSame && conflationTicksCandleSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPublishableTickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (ConflatedTicksCandle?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public override string QuoteToStringMembers =>
        $"{base.QuoteToStringMembers}, {nameof(AdapterSentTime)}: {AdapterSentTime}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime}, {nameof(ConflatedTicksCandle)}: {ConflatedTicksCandle}";

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {PQQuoteContainer.QuoteToStringMembers}, {UpdatedFlagsToString})";
}
