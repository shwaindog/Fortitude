// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public interface IPQLevel1Quote : IPQLevel0Quote, IMutableLevel1Quote
{
    bool IsSourceAskTimeDateUpdated          { get; }
    bool IsSourceAskTimeSubHourUpdated       { get; }
    bool IsSourceBidTimeDateUpdated          { get; }
    bool IsSourceBidTimeSubHourUpdated       { get; }
    bool IsAdapterSentTimeDateUpdated        { get; }
    bool IsAdapterSentTimeSubHourUpdated     { get; }
    bool IsAdapterReceivedTimeDateUpdated    { get; }
    bool IsAdapterReceivedTimeSubHourUpdated { get; }

    bool IsBidPriceTopUpdatedChanged { get; }
    bool IsAskPriceTopUpdatedChanged { get; }

    bool IsBidPriceTopChanged { get; }
    bool IsAskPriceTopChanged { get; }
    bool IsExecutableUpdated  { get; }

    new IPQPricePeriodSummary? SummaryPeriod { get; set; }
    new IPQLevel1Quote         Clone();
}

public class PQLevel1Quote : PQLevel0Quote, IPQLevel1Quote
{
    private DateTime adapterReceivedTime = DateTimeConstants.UnixEpoch;
    private DateTime adapterSentTime     = DateTimeConstants.UnixEpoch;
    private decimal  askPriceTop;
    private decimal  bidPriceTop;

    protected DateTime sourceAskTime = DateTimeConstants.UnixEpoch;
    protected DateTime sourceBidTime = DateTimeConstants.UnixEpoch;

    public PQLevel1Quote() => BooleanFields |= PQBooleanValues.IsExecutableSetFlag;

    public PQLevel1Quote(ISourceTickerQuoteInfo sourceTickerInfo)
        : base(sourceTickerInfo) =>
        BooleanFields |= PQBooleanValues.IsExecutableSetFlag;

    public PQLevel1Quote(ILevel0Quote toClone) : base(toClone)
    {
        if (toClone is ILevel1Quote l1QToClone)
        {
            adapterSentTime     = l1QToClone.AdapterSentTime;
            adapterReceivedTime = l1QToClone.AdapterReceivedTime;
            sourceAskTime       = l1QToClone.SourceAskTime;
            sourceBidTime       = l1QToClone.SourceBidTime;
            Executable          = l1QToClone.Executable;
            if (this is not ILevel2Quote)
            {
                bidPriceTop          = l1QToClone.BidPriceTop;
                askPriceTop          = l1QToClone.AskPriceTop;
                IsBidPriceTopUpdated = l1QToClone.IsBidPriceTopUpdated;
                IsAskPriceTopUpdated = l1QToClone.IsAskPriceTopUpdated;
            }
            if (l1QToClone.SummaryPeriod is { IsEmpty: false })
                SummaryPeriod = new PQPricePeriodSummary(l1QToClone.SummaryPeriod);
            if (toClone is IPQLevel1Quote ipqL1)
            {
                IsExecutableUpdated         = ipqL1.IsExecutableUpdated;
                IsBidPriceTopUpdatedChanged = ipqL1.IsBidPriceTopUpdatedChanged;
                IsAskPriceTopUpdatedChanged = ipqL1.IsAskPriceTopUpdatedChanged;
                IsBidPriceTopChanged        = ipqL1.IsBidPriceTopChanged;
                IsAskPriceTopChanged        = ipqL1.IsAskPriceTopChanged;

                IsSourceAskTimeDateUpdated          = ipqL1.IsSourceAskTimeDateUpdated;
                IsSourceAskTimeSubHourUpdated       = ipqL1.IsSourceAskTimeSubHourUpdated;
                IsSourceBidTimeDateUpdated          = ipqL1.IsSourceBidTimeDateUpdated;
                IsSourceBidTimeSubHourUpdated       = ipqL1.IsSourceBidTimeSubHourUpdated;
                IsAdapterSentTimeDateUpdated        = ipqL1.IsAdapterSentTimeDateUpdated;
                IsAdapterSentTimeSubHourUpdated     = ipqL1.IsAdapterSentTimeSubHourUpdated;
                IsAdapterReceivedTimeDateUpdated    = ipqL1.IsAdapterReceivedTimeDateUpdated;
                IsAdapterReceivedTimeSubHourUpdated = ipqL1.IsAdapterReceivedTimeSubHourUpdated;
            }
        }
    }

    protected string Level1ToStringMembers =>
        $"{Level0ToStringMembers}, {nameof(IsSourceAskTimeDateUpdated)}: {IsSourceAskTimeDateUpdated}, " +
        $"{nameof(IsSourceAskTimeSubHourUpdated)}: {IsSourceAskTimeSubHourUpdated}, " +
        $"{nameof(IsSourceBidTimeDateUpdated)}: {IsSourceBidTimeDateUpdated}, " +
        $"{nameof(IsSourceBidTimeSubHourUpdated)}: {IsSourceBidTimeSubHourUpdated}, " +
        $"{nameof(IsAdapterSentTimeDateUpdated)}: {IsAdapterSentTimeDateUpdated}, " +
        $"{nameof(IsAdapterSentTimeSubHourUpdated)}: {IsAdapterSentTimeSubHourUpdated}, " +
        $"{nameof(IsAdapterReceivedTimeDateUpdated)}: {IsAdapterReceivedTimeDateUpdated}, " +
        $"{nameof(IsAdapterReceivedTimeSubHourUpdated)}: {IsAdapterReceivedTimeSubHourUpdated}, " +
        $"{nameof(IsExecutableUpdated)}: {IsExecutableUpdated}, {nameof(SourceTime)}: {SourceTime}, " +
        $"{nameof(SourceAskTime)}: {SourceAskTime}, {nameof(SourceBidTime)}: {SourceBidTime}, " +
        $"{nameof(AdapterSentTime)}: {AdapterSentTime}, {nameof(AdapterReceivedTime)}: {AdapterReceivedTime}, " +
        $"{nameof(BidPriceTop)}: {BidPriceTop}, {nameof(IsBidPriceTopChanged)}: {IsBidPriceTopChanged}, " +
        $"{nameof(IsBidPriceTopUpdated)}: {IsBidPriceTopUpdated}, {nameof(AskPriceTop)}: {AskPriceTop}, " +
        $"{nameof(IsAskPriceTopChanged)}: {IsAskPriceTopChanged}, {nameof(IsAskPriceTopUpdated)}: {IsAskPriceTopUpdated}, " +
        $"{nameof(Executable)}: {Executable}, {nameof(SummaryPeriod)}: {SummaryPeriod}";

    public bool IsSourceAskTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags                                  |= QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag;
            else if (IsSourceAskTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceAskTimeDateUpdatedFlag;
        }
    }

    public bool IsSourceAskTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceAskTimeSubSecondUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceAskTimeSubSecondUpdatedFlag;
            else if (IsSourceAskTimeSubHourUpdated)
                UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceAskTimeSubSecondUpdatedFlag;
        }
    }


    public bool IsSourceBidTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags                                  |= QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag;
            else if (IsSourceBidTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag;
        }
    }

    public bool IsSourceBidTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceBidTimeSubSecondUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceBidTimeSubSecondUpdatedFlag;
            else if (IsSourceBidTimeSubHourUpdated)
                UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceBidTimeSubSecondUpdatedFlag;
        }
    }

    public bool IsAdapterSentTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag;
            else if (IsAdapterSentTimeDateUpdated)
                UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterSentTimeDateUpdatedFlag;
        }
    }

    public bool IsAdapterSentTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterSentTimeSubSecondUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterSentTimeSubSecondUpdatedFlag;
            else if (IsAdapterSentTimeSubHourUpdated)
                UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterSentTimeSubSecondUpdatedFlag;
        }
    }


    public bool IsAdapterReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;
            else if (IsAdapterReceivedTimeDateUpdated)
                UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;
        }
    }

    public bool IsAdapterReceivedTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AdapterReceivedTimeSubSecondUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AdapterReceivedTimeSubSecondUpdatedFlag;
            else if (IsAdapterReceivedTimeSubHourUpdated)
                UpdatedFlags ^= QuoteFieldUpdatedFlags.AdapterReceivedTimeSubSecondUpdatedFlag;
        }
    }

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

    public bool IsBidPriceTopChanged
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.BidTopPriceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.BidTopPriceUpdatedFlag;

            else if (IsBidPriceTopChanged) UpdatedFlags ^= QuoteFieldUpdatedFlags.BidTopPriceUpdatedFlag;
        }
    }

    public bool IsAskPriceTopChanged
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.AskTopPriceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.AskTopPriceUpdatedFlag;

            else if (IsAskPriceTopChanged) UpdatedFlags ^= QuoteFieldUpdatedFlags.AskTopPriceUpdatedFlag;
        }
    }

    public bool IsBidPriceTopUpdatedChanged
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.PreviousQuoteBidTopUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.PreviousQuoteBidTopUpdatedFlag;

            else if (IsBidPriceTopUpdatedChanged) UpdatedFlags ^= QuoteFieldUpdatedFlags.PreviousQuoteBidTopUpdatedFlag;
        }
    }

    public bool IsAskPriceTopUpdatedChanged
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.PreviousQuoteAskTopUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.PreviousQuoteAskTopUpdatedFlag;

            else if (IsAskPriceTopUpdatedChanged) UpdatedFlags ^= QuoteFieldUpdatedFlags.PreviousQuoteAskTopUpdatedFlag;
        }
    }

    public override QuoteLevel QuoteLevel => QuoteLevel.Level1;

    public override DateTime SourceTime
    {
        get =>
            new(Math.Max(base.SourceTime.Ticks,
                         Math.Max(sourceBidTime.Ticks, sourceAskTime.Ticks)));
        set => base.SourceTime = value;
    }

    public DateTime SourceAskTime
    {
        get => sourceAskTime;
        set
        {
            if (value == sourceAskTime) return;
            IsSourceAskTimeDateUpdated    |= sourceAskTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsSourceAskTimeSubHourUpdated |= sourceAskTime.GetSubHourComponent() != value.GetSubHourComponent();
            sourceAskTime                 =  value;
        }
    }

    public DateTime SourceBidTime
    {
        get => sourceBidTime;
        set
        {
            if (value == sourceBidTime) return;
            IsSourceBidTimeDateUpdated    |= sourceBidTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsSourceBidTimeSubHourUpdated |= sourceBidTime.GetSubHourComponent() != value.GetSubHourComponent();
            sourceBidTime                 =  value;
        }
    }

    public DateTime AdapterSentTime
    {
        get => adapterSentTime;
        set
        {
            if (value == adapterSentTime) return;
            IsAdapterSentTimeDateUpdated |= adapterSentTime.GetHoursFromUnixEpoch()
                                         != value.GetHoursFromUnixEpoch();
            IsAdapterSentTimeSubHourUpdated |= adapterSentTime.GetSubHourComponent()
                                            != value.GetSubHourComponent();
            adapterSentTime = value;
        }
    }

    public DateTime AdapterReceivedTime
    {
        get => adapterReceivedTime;
        set
        {
            if (adapterReceivedTime == value) return;
            IsAdapterReceivedTimeDateUpdated |= adapterReceivedTime.GetHoursFromUnixEpoch()
                                             != value.GetHoursFromUnixEpoch();
            IsAdapterReceivedTimeSubHourUpdated |= adapterReceivedTime.GetSubHourComponent()
                                                != value.GetSubHourComponent();
            adapterReceivedTime = value;
        }
    }

    public virtual decimal BidPriceTop
    {
        get => bidPriceTop;
        set
        {
            if (BidPriceTop == value) return;
            IsBidPriceTopUpdated = true;
            IsBidPriceTopChanged = true;
            bidPriceTop          = value;
        }
    }

    public bool IsBidPriceTopUpdated
    {
        get => (BooleanFields & PQBooleanValues.IsBidPriceTopUpdatedSetFlag) > 0;
        set
        {
            if (IsBidPriceTopUpdated == value) return;
            IsBidPriceTopUpdatedChanged = true;
            if (value)
                BooleanFields |= PQBooleanValues.IsBidPriceTopUpdatedSetFlag;

            else if (IsBidPriceTopUpdated) BooleanFields ^= PQBooleanValues.IsBidPriceTopUpdatedSetFlag;
        }
    }

    public virtual decimal AskPriceTop
    {
        get => askPriceTop;
        set
        {
            if (askPriceTop == value) return;
            IsAskPriceTopUpdated = true;
            IsAskPriceTopChanged = true;
            askPriceTop          = value;
        }
    }

    public bool IsAskPriceTopUpdated
    {
        get => (BooleanFields & PQBooleanValues.IsAskPriceTopUpdatedSetFlag) > 0;
        set
        {
            if (IsAskPriceTopUpdated == value) return;
            IsAskPriceTopUpdatedChanged = true;
            if (value)
                BooleanFields |= PQBooleanValues.IsAskPriceTopUpdatedSetFlag;

            else if (IsAskPriceTopUpdated) BooleanFields ^= PQBooleanValues.IsAskPriceTopUpdatedSetFlag;
        }
    }

    public bool Executable
    {
        get => (BooleanFields & PQBooleanValues.IsExecutableSetFlag) > 0;
        set
        {
            if (Executable == value) return;
            IsExecutableUpdated = true;
            if (value)
                BooleanFields |= PQBooleanValues.IsExecutableSetFlag;

            else if (Executable) BooleanFields ^= PQBooleanValues.IsExecutableSetFlag;
        }
    }

    IPricePeriodSummary? ILevel1Quote.SummaryPeriod => SummaryPeriod;

    IMutablePricePeriodSummary? IMutableLevel1Quote.SummaryPeriod
    {
        get => SummaryPeriod;
        set => SummaryPeriod = value as IPQPricePeriodSummary;
    }

    public IPQPricePeriodSummary? SummaryPeriod { get; set; }

    public override bool HasUpdates
    {
        get => base.HasUpdates || (SummaryPeriod?.HasUpdates ?? false);
        set
        {
            if (SummaryPeriod != null) SummaryPeriod.HasUpdates = value;
            base.HasUpdates = value;
        }
    }

    public override void ResetFields()
    {
        BooleanFields = PQBooleanValues.IsExecutableSetFlag;

        adapterSentTime     = DateTimeConstants.UnixEpoch;
        adapterReceivedTime = DateTimeConstants.UnixEpoch;
        sourceAskTime       = DateTimeConstants.UnixEpoch;
        sourceBidTime       = DateTimeConstants.UnixEpoch;

        IsBidPriceTopUpdated = IsAskPriceTopUpdated = false;

        bidPriceTop = askPriceTop = 0m;

        if (SummaryPeriod != null) SummaryPeriod.IsEmpty = true;
        base.ResetFields();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, StorageFlags messageFlags,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var precisionSettings = quotePublicationPrecisionSettings ?? PQSourceTickerQuoteInfo;
        var updatedOnly       = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                               precisionSettings).Where(pqfield => pqfield.Flag != PQFieldKeys.SinglePrice))
            yield return updatedField;
        if (!updatedOnly || IsSourceBidTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.SourceBidDateTime, sourceBidTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsSourceBidTimeSubHourUpdated)
        {
            var flag = sourceBidTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
            yield return new PQFieldUpdate(PQFieldKeys.SourceBidSubHourTime, value, flag);
        }

        if (!updatedOnly || IsSourceAskTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.SourceAskDateTime, sourceAskTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsSourceAskTimeSubHourUpdated)
        {
            var flag = sourceAskTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
            yield return new PQFieldUpdate(PQFieldKeys.SourceAskSubHourTime, value, flag);
        }

        if (!updatedOnly || IsAdapterReceivedTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.AdapterReceivedDateTime,
                                           adapterReceivedTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsAdapterReceivedTimeSubHourUpdated)
        {
            var flag = adapterReceivedTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
            yield return new PQFieldUpdate(PQFieldKeys.AdapterReceivedSubHourTime, value, flag);
        }

        if (!IsReplay) AdapterSentTime = snapShotTime;
        if (!updatedOnly || IsAdapterSentTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.AdapterSentDateTime,
                                           adapterSentTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsAdapterSentTimeSubHourUpdated)
        {
            var flag = adapterSentTime.GetSubHourComponent().BreakLongToByteAndUint(out var value);
            yield return new PQFieldUpdate(PQFieldKeys.AdapterSentSubHourTime, value, flag);
        }

        foreach (var updatedField in GetDeltaUpdateTopBookPriceFields(snapShotTime, updatedOnly,
                                                                      precisionSettings).Where(pqfield => pqfield.Flag != PQFieldKeys.SinglePrice))
            yield return updatedField;
        if (SummaryPeriod != null)
            foreach (var periodSummaryUpdates in SummaryPeriod.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                                    precisionSettings))
                yield return periodSummaryUpdates;
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id >= PQFieldKeys.PeriodStartDateTime &&
            pqFieldUpdate.Id <= PQFieldKeys.PeriodVolumeUpperBytes)
        {
            SummaryPeriod ??= new PQPricePeriodSummary();
            return SummaryPeriod.UpdateField(pqFieldUpdate);
        }
        switch (pqFieldUpdate.Id)
        {
            case PQFieldKeys.SourceBidDateTime:
                IsSourceBidTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceBidTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.SourceBidSubHourTime:
                IsSourceBidTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref sourceBidTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.SourceAskDateTime:
                IsSourceAskTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceAskTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.SourceAskSubHourTime:
                IsSourceAskTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref sourceAskTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.AdapterReceivedDateTime:
                IsAdapterReceivedTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterReceivedTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.AdapterReceivedSubHourTime:
                IsAdapterReceivedTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref adapterReceivedTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.AdapterSentDateTime:
                IsAdapterSentTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterSentTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.AdapterSentSubHourTime:
                IsAdapterSentTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref adapterSentTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.LayerPriceOffset:
                if (pqFieldUpdate.IsBid())
                {
                    var previousBidPriceTopUpdatedChanged = IsBidPriceTopUpdatedChanged;
                    var previousBidPriceTopUpdated        = IsBidPriceTopUpdated;
                    BidPriceTop                 = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    IsBidPriceTopUpdated        = previousBidPriceTopUpdated;
                    IsBidPriceTopUpdatedChanged = previousBidPriceTopUpdatedChanged;
                }
                else
                {
                    var previousAskPriceTopUpdatedChanged = IsAskPriceTopUpdatedChanged;
                    var previousAskPriceTopUpdated        = IsAskPriceTopUpdated;
                    AskPriceTop                 = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                    IsAskPriceTopUpdated        = previousAskPriceTopUpdated;
                    IsAskPriceTopUpdatedChanged = previousAskPriceTopUpdatedChanged;
                }
                return 0;
            default:
                return base.UpdateField(pqFieldUpdate);
        }
    }

    public override ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IPQLevel1Quote pq1)
        {
            // between types only copy the changed parts not everything.
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pq1.IsAdapterReceivedTimeDateUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = adapterReceivedTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterReceivedTime,
                                                           pq1.AdapterReceivedTime.GetHoursFromUnixEpoch());
                IsAdapterReceivedTimeDateUpdated = originalAdapterReceivedTime != adapterReceivedTime;
            }
            if (pq1.IsAdapterReceivedTimeSubHourUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = adapterReceivedTime;
                PQFieldConverters.UpdateSubHourComponent(ref adapterReceivedTime,
                                                         pq1.AdapterReceivedTime.GetSubHourComponent());
                IsAdapterReceivedTimeSubHourUpdated = originalAdapterReceivedTime != adapterReceivedTime;
            }
            if (pq1.IsAdapterSentTimeDateUpdated || isFullReplace)
            {
                var originalAdapterSentTime = adapterSentTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterSentTime,
                                                           pq1.AdapterSentTime.GetHoursFromUnixEpoch());
                IsAdapterSentTimeDateUpdated = originalAdapterSentTime != adapterSentTime;
            }
            if (pq1.IsAdapterSentTimeSubHourUpdated || isFullReplace)
            {
                var originalAdapterSentTime = adapterSentTime;
                PQFieldConverters.UpdateSubHourComponent(ref adapterSentTime,
                                                         pq1.AdapterSentTime.GetSubHourComponent());
                IsAdapterSentTimeSubHourUpdated = originalAdapterSentTime != adapterSentTime;
            }
            if (pq1.IsSourceBidTimeDateUpdated || isFullReplace)
            {
                var originalSourceBidTime = sourceBidTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceBidTime,
                                                           pq1.SourceBidTime.GetHoursFromUnixEpoch());
                IsSourceBidTimeDateUpdated = originalSourceBidTime != sourceBidTime;
            }
            if (pq1.IsSourceBidTimeSubHourUpdated || isFullReplace)
            {
                var originalSourceBidTime = sourceBidTime;
                PQFieldConverters.UpdateSubHourComponent(ref sourceBidTime,
                                                         pq1.SourceBidTime.GetSubHourComponent());
                IsSourceBidTimeSubHourUpdated = originalSourceBidTime != sourceBidTime;
            }
            if (pq1.IsSourceAskTimeDateUpdated || isFullReplace)
            {
                var originalSourceAskTime = sourceAskTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceAskTime,
                                                           pq1.SourceAskTime.GetHoursFromUnixEpoch());
                IsSourceAskTimeDateUpdated = originalSourceAskTime != sourceAskTime;
            }
            if (pq1.IsSourceAskTimeSubHourUpdated || isFullReplace)
            {
                var originalSourceAskTime = sourceAskTime;
                PQFieldConverters.UpdateSubHourComponent(ref sourceAskTime,
                                                         pq1.SourceAskTime.GetSubHourComponent());
                IsSourceAskTimeSubHourUpdated = originalSourceAskTime != sourceAskTime;
            }
            if (this is not ILevel2Quote)
            {
                if (pq1.IsBidPriceTopChanged || isFullReplace) bidPriceTop = pq1.BidPriceTop;
                if (pq1.IsAskPriceTopChanged || isFullReplace) askPriceTop = pq1.AskPriceTop;

                if (pq1.IsBidPriceTopUpdatedChanged || isFullReplace) IsBidPriceTopUpdated = pq1.IsBidPriceTopUpdated;
                if (pq1.IsAskPriceTopUpdatedChanged || isFullReplace) IsAskPriceTopUpdated = pq1.IsAskPriceTopUpdated;
            }

            if (pq1.IsExecutableUpdated || isFullReplace) Executable = pq1.Executable;

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
            if (isFullReplace && pq1 is PQLevel1Quote pqLevel1Quote) UpdatedFlags = pqLevel1Quote.UpdatedFlags;
        }
        else if (source is ILevel1Quote l1Q) // normal copy
        {
            AdapterReceivedTime = l1Q.AdapterReceivedTime;
            AdapterSentTime     = l1Q.AdapterSentTime;
            SourceBidTime       = l1Q.SourceBidTime;
            SourceAskTime       = l1Q.SourceAskTime;
            if (this is not ILevel2Quote)
            {
                BidPriceTop          = l1Q.BidPriceTop;
                AskPriceTop          = l1Q.AskPriceTop;
                IsAskPriceTopUpdated = l1Q.IsAskPriceTopUpdated;
                IsBidPriceTopUpdated = l1Q.IsBidPriceTopUpdated;
            }

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

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => (ILevel1Quote)Clone();

    ILevel1Quote ILevel1Quote.Clone() => (ILevel1Quote)Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => (IMutableLevel1Quote)Clone();

    IPQLevel1Quote IPQLevel1Quote.Clone() => (IPQLevel1Quote)Clone();

    public override IPQLevel0Quote Clone() =>
        (IPQLevel0Quote?)Recycler?.Borrow<PQLevel1Quote>().CopyFrom(this, CopyMergeFlags.FullReplace)
     ?? new PQLevel1Quote(this);

    public override bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (!(other is ILevel1Quote otherL1)) return false;
        var baseSame            = base.AreEquivalent(otherL1, exactTypes);
        var adapterSentSame     = adapterSentTime.Equals(otherL1.AdapterSentTime);
        var adapterReceivedSame = adapterReceivedTime.Equals(otherL1.AdapterReceivedTime);
        var sourceAskTimeSame   = sourceAskTime.Equals(otherL1.SourceAskTime);
        var sourceBidTimeSame   = sourceBidTime.Equals(otherL1.SourceBidTime);
        var executableSame      = Executable == otherL1.Executable;
        var bidPriceTopSame     = BidPriceTop == otherL1.BidPriceTop;
        var askPriceTopSame     = AskPriceTop == otherL1.AskPriceTop;
        var bidPriceTopChange   = IsBidPriceTopUpdated == otherL1.IsBidPriceTopUpdated;
        var askPriceTopChange   = IsAskPriceTopUpdated == otherL1.IsAskPriceTopUpdated;
        var periodSummarySame
            = ((SummaryPeriod == null || SummaryPeriod.IsEmpty) && (otherL1.SummaryPeriod == null || otherL1.SummaryPeriod.IsEmpty)) ||
              (SummaryPeriod?.AreEquivalent(otherL1.SummaryPeriod, exactTypes) ?? otherL1.SummaryPeriod == null);

        var isExecutableUpdatedSame = true;

        if (exactTypes && otherL1 is IPQLevel1Quote pqL1) isExecutableUpdatedSame = IsExecutableUpdated == pqL1.IsExecutableUpdated;

        var allAreSame = baseSame && adapterSentSame && adapterReceivedSame && sourceAskTimeSame
                      && sourceBidTimeSame && executableSame && bidPriceTopSame && askPriceTopSame
                      && bidPriceTopChange && askPriceTopChange && periodSummarySame && isExecutableUpdatedSame;
        // if (!allAreSame) Debugger.Break();
        return allAreSame;
    }

    public DateTime StorageTime(IStorageTimeResolver<ILevel1Quote>? resolver = null)
    {
        resolver ??= QuoteStorageTimeResolver.Instance;
        return resolver.ResolveStorageTime(this);
    }

    protected override bool IsBooleanFlagsChanged() =>
        base.IsBooleanFlagsChanged() || IsExecutableUpdated || IsBidPriceTopUpdatedChanged || IsAskPriceTopUpdatedChanged;

    protected override PQBooleanValues GenerateBooleanFlags(bool fullUpdate)
    {
        var resultSoFar = base.GenerateBooleanFlags(fullUpdate);
        return resultSoFar | (IsExecutableUpdated || fullUpdate ? PQBooleanValues.IsExecutableUpdatedFlag : PQBooleanValues.None)
                           | (Executable ? PQBooleanValues.IsExecutableSetFlag : PQBooleanValues.None)
                           | (IsBidPriceTopUpdatedChanged || fullUpdate ? PQBooleanValues.IsBidPriceTopUpdatedChangedFlag : PQBooleanValues.None)
                           | (IsBidPriceTopUpdated ? PQBooleanValues.IsBidPriceTopUpdatedSetFlag : PQBooleanValues.None)
                           | (IsAskPriceTopUpdatedChanged || fullUpdate ? PQBooleanValues.IsAskPriceTopUpdatedChangedFlag : PQBooleanValues.None)
                           | (IsAskPriceTopUpdated ? PQBooleanValues.IsAskPriceTopUpdatedSetFlag : PQBooleanValues.None);
    }

    protected override void SetBooleanFields(PQBooleanValues booleanFlags)
    {
        base.SetBooleanFields(booleanFlags);
        IsExecutableUpdated = (booleanFlags & PQBooleanValues.IsExecutableUpdatedFlag) > 0;
        if (IsExecutableUpdated) Executable = (booleanFlags & PQBooleanValues.IsExecutableSetFlag) > 0;
        IsBidPriceTopUpdatedChanged = (booleanFlags & PQBooleanValues.IsBidPriceTopUpdatedChangedFlag) > 0;
        if (IsBidPriceTopUpdatedChanged) IsBidPriceTopUpdated = (booleanFlags & PQBooleanValues.IsBidPriceTopUpdatedSetFlag) > 0;
        IsAskPriceTopUpdatedChanged = (booleanFlags & PQBooleanValues.IsAskPriceTopUpdatedChangedFlag) > 0;
        if (IsAskPriceTopUpdatedChanged) IsAskPriceTopUpdated = (booleanFlags & PQBooleanValues.IsAskPriceTopUpdatedSetFlag) > 0;
    }

    protected virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateTopBookPriceFields(DateTime snapShotTime,
        bool updatedOnly, IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        if (!updatedOnly || IsBidPriceTopChanged)
            yield return new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, BidPriceTop
                                         , PQSourceTickerQuoteInfo!.PriceScalingPrecision);
        if (!updatedOnly || IsAskPriceTopChanged)
            yield return new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, AskPriceTop, (byte)(PQFieldFlags.IsAskSideFlag
                                                                                           | PQSourceTickerQuoteInfo!.PriceScalingPrecision));
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ILevel0Quote?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ adapterSentTime.GetHashCode();
            hashCode = (hashCode * 397) ^ adapterReceivedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ sourceAskTime.GetHashCode();
            hashCode = (hashCode * 397) ^ sourceBidTime.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            hashCode = (hashCode * 397) ^ bidPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ askPriceTop.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level0ToStringMembers}, {Level1ToStringMembers})";
}
