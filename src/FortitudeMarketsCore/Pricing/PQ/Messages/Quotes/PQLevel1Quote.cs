#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Conflation;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Conflation;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public class PQLevel1Quote : PQLevel0Quote, IPQLevel1Quote
{
    private DateTime adapterReceivedTime = DateTimeConstants.UnixEpoch;
    private DateTime adapterSentTime = DateTimeConstants.UnixEpoch;
    private decimal askPriceTop;
    private decimal bidPriceTop;

    private bool executable = true;

    protected DateTime sourceAskTime = DateTimeConstants.UnixEpoch;
    protected DateTime sourceBidTime = DateTimeConstants.UnixEpoch;

    public PQLevel1Quote() { }

    public PQLevel1Quote(ISourceTickerQuoteInfo sourceTickerInfo)
        : base(sourceTickerInfo) =>
        PeriodSummary = new PQPeriodSummary();

    public PQLevel1Quote(ILevel0Quote toClone) : base(toClone)
    {
        if (toClone is ILevel1Quote l1QToClone)
        {
            adapterSentTime = l1QToClone.AdapterSentTime;
            adapterReceivedTime = l1QToClone.AdapterReceivedTime;
            sourceAskTime = l1QToClone.SourceAskTime;
            sourceBidTime = l1QToClone.SourceBidTime;
            executable = l1QToClone.Executable;
            bidPriceTop = l1QToClone.BidPriceTop;
            askPriceTop = l1QToClone.AskPriceTop;
            if (l1QToClone.PeriodSummary is PQPeriodSummary)
                PeriodSummary = (PQPeriodSummary?)l1QToClone.PeriodSummary?.Clone();
            else if (l1QToClone.PeriodSummary != null) PeriodSummary = new PQPeriodSummary(l1QToClone.PeriodSummary);
        }
    }

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
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceBidTimeDateUpdatedFlag;
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

    protected string Level1ToStringMembers =>
        $"{base.ToString()}, {nameof(IsSourceAskTimeDateUpdated)}: {IsSourceAskTimeDateUpdated}, " +
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
        $"{nameof(BidPriceTop)}: {BidPriceTop}, {nameof(IsBidPriceTopUpdated)}: {IsBidPriceTopUpdated}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop}, {nameof(IsAskPriceTopUpdated)}: {IsAskPriceTopUpdated}, " +
        $"{nameof(Executable)}: {Executable}";

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
            IsSourceAskTimeDateUpdated |= sourceAskTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsSourceAskTimeSubHourUpdated |= sourceAskTime.GetSubHourComponent() != value.GetSubHourComponent();
            sourceAskTime = value;
        }
    }

    public DateTime SourceBidTime
    {
        get => sourceBidTime;
        set
        {
            if (value == sourceBidTime) return;
            IsSourceBidTimeDateUpdated |= sourceBidTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsSourceBidTimeSubHourUpdated |= sourceBidTime.GetSubHourComponent() != value.GetSubHourComponent();
            sourceBidTime = value;
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
            bidPriceTop = value;
        }
    }

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

    public virtual decimal AskPriceTop
    {
        get => askPriceTop;
        set
        {
            if (askPriceTop == value) return;
            IsAskPriceTopUpdated = true;
            askPriceTop = value;
        }
    }

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

    public bool Executable
    {
        get => executable;
        set
        {
            if (value == executable) return;
            IsExecutableUpdated = true;
            executable = value;
        }
    }

    IPeriodSummary? ILevel1Quote.PeriodSummary => PeriodSummary;

    IMutablePeriodSummary? IMutableLevel1Quote.PeriodSummary
    {
        get => PeriodSummary;
        set => PeriodSummary = value as IPQPeriodSummary;
    }

    public IPQPeriodSummary? PeriodSummary { get; set; }

    public override bool HasUpdates
    {
        get => base.HasUpdates || (PeriodSummary?.HasUpdates ?? false);
        set
        {
            if (PeriodSummary != null) PeriodSummary.HasUpdates = value;
            base.HasUpdates = value;
        }
    }

    public override void ResetFields()
    {
        executable = true;

        adapterSentTime = DateTimeConstants.UnixEpoch;
        adapterReceivedTime = DateTimeConstants.UnixEpoch;
        sourceAskTime = DateTimeConstants.UnixEpoch;
        sourceBidTime = DateTimeConstants.UnixEpoch;

        bidPriceTop = askPriceTop = 0m;
        base.ResetFields();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, PQMessageFlags messageFlags,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var precisionSettings = quotePublicationPrecisionSettings ?? PQSourceTickerQuoteInfo;
        var updatedOnly = (messageFlags & PQMessageFlags.Complete) == 0;
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
        if (PeriodSummary != null)
            foreach (var periodSummaryUpdates in PeriodSummary.GetDeltaUpdateFields(snapShotTime, messageFlags,
                         precisionSettings))
                yield return periodSummaryUpdates;
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id >= PQFieldKeys.PeriodStartDateTime &&
            pqFieldUpdate.Id <= PQFieldKeys.PeriodVolumeUpperBytes)
            return PeriodSummary?.UpdateField(pqFieldUpdate) ?? -1;
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
                    BidPriceTop = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                else
                    AskPriceTop = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            default:
                return base.UpdateField(pqFieldUpdate);
        }
    }

    public override ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source);

        if (source is PQLevel1Quote pq1)
        {
            // between types only copy the changed parts not everything.
            if (pq1.IsAdapterReceivedTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterReceivedTime,
                    pq1.adapterReceivedTime.GetHoursFromUnixEpoch());
            if (pq1.IsAdapterReceivedTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref adapterReceivedTime,
                    pq1.adapterReceivedTime.GetSubHourComponent());
            if (pq1.IsAdapterSentTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref adapterSentTime,
                    pq1.adapterSentTime.GetHoursFromUnixEpoch());
            if (pq1.IsAdapterSentTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref adapterSentTime,
                    pq1.adapterSentTime.GetSubHourComponent());
            if (pq1.IsSourceBidTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceBidTime,
                    pq1.sourceBidTime.GetHoursFromUnixEpoch());
            if (pq1.IsSourceBidTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref sourceBidTime,
                    pq1.sourceBidTime.GetSubHourComponent());
            if (pq1.IsSourceAskTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceAskTime,
                    pq1.sourceAskTime.GetHoursFromUnixEpoch());
            if (pq1.IsSourceAskTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref sourceAskTime,
                    pq1.sourceAskTime.GetSubHourComponent());
            if (pq1.IsBidPriceTopUpdated) bidPriceTop = pq1.bidPriceTop;
            if (pq1.IsAskPriceTopUpdated) askPriceTop = pq1.askPriceTop;
            if (pq1.IsExecutableUpdated) executable = pq1.executable;
            // ensure flags still match source
            UpdatedFlags = pq1.UpdatedFlags;
        }
        else if (source is ILevel1Quote l1Q) // normal copy
        {
            AdapterReceivedTime = l1Q.AdapterReceivedTime;
            AdapterSentTime = l1Q.AdapterSentTime;
            SourceBidTime = l1Q.SourceBidTime;
            SourceAskTime = l1Q.SourceAskTime;
            BidPriceTop = l1Q.BidPriceTop;
            AskPriceTop = l1Q.AskPriceTop;
            IsAskPriceTopUpdated = l1Q.IsAskPriceTopUpdated;
            IsBidPriceTopUpdated = l1Q.IsBidPriceTopUpdated;
            Executable = l1Q.Executable;
            PeriodSummary?.CopyFrom(new PQPeriodSummary(l1Q.PeriodSummary!));
        }

        return this;
    }

    ILevel1Quote ICloneable<ILevel1Quote>.Clone() => (ILevel1Quote)Clone();

    ILevel1Quote ILevel1Quote.Clone() => (ILevel1Quote)Clone();

    IMutableLevel1Quote IMutableLevel1Quote.Clone() => (IMutableLevel1Quote)Clone();

    IPQLevel1Quote IPQLevel1Quote.Clone() => (IPQLevel1Quote)Clone();

    public override IPQLevel0Quote Clone() => (IPQLevel0Quote?)Recycler?.Borrow<PQLevel1Quote>().CopyFrom(this) ?? new PQLevel1Quote(this);

    public override bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (!(other is ILevel1Quote otherL1)) return false;
        var baseSame = base.AreEquivalent(otherL1, exactTypes);
        var adapterSentSame = adapterSentTime.Equals(otherL1.AdapterSentTime);
        var adapterReceivedSame = adapterReceivedTime.Equals(otherL1.AdapterReceivedTime);
        var sourceAskTimeSame = sourceAskTime.Equals(otherL1.SourceAskTime);
        var sourceBidTimeSame = sourceBidTime.Equals(otherL1.SourceBidTime);
        var executableSame = executable == otherL1.Executable;
        var bidPriceTopSame = BidPriceTop == otherL1.BidPriceTop;
        var askPriceTopSame = AskPriceTop == otherL1.AskPriceTop;
        var bidPriceTopChange = true;
        var askPriceTopChange = true;
        if (exactTypes)
        {
            bidPriceTopChange = IsBidPriceTopUpdated == otherL1.IsBidPriceTopUpdated;
            askPriceTopChange = IsBidPriceTopUpdated == otherL1.IsBidPriceTopUpdated;
        }

        return baseSame && adapterSentSame && adapterReceivedSame && sourceAskTimeSame
               && sourceBidTimeSame && executableSame && bidPriceTopSame && askPriceTopSame
               && bidPriceTopChange && askPriceTopChange;
    }

    protected override bool IsBooleanFlagsChanged() => base.IsBooleanFlagsChanged() || IsExecutableUpdated;

    protected override uint GenerateBooleanFlags()
    {
        var resultSoFar = base.GenerateBooleanFlags();
        return resultSoFar | (Executable ? (uint)PQBooleanValues.IsExecutableFlag : 0);
    }

    protected override void SetBooleanFields(uint booleanFlags)
    {
        base.SetBooleanFields(booleanFlags);
        Executable = (booleanFlags & PQBooleanValues.IsExecutableFlag) == PQBooleanValues.IsExecutableFlag;
    }

    protected virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateTopBookPriceFields(DateTime snapShotTime,
        bool updatedOnly, IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        if (!updatedOnly || IsBidPriceTopUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerPriceOffset, BidPriceTop
                , PQSourceTickerQuoteInfo!.PriceScalingPrecision);
        if (!updatedOnly || IsAskPriceTopUpdated)
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
            hashCode = (hashCode * 397) ^ executable.GetHashCode();
            hashCode = (hashCode * 397) ^ bidPriceTop.GetHashCode();
            hashCode = (hashCode * 397) ^ askPriceTop.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level0ToStringMembers}, {Level1ToStringMembers})";
}
