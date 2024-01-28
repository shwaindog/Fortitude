#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Quotes;

public class PQLevel0Quote : ReusableObject<ILevel0Quote>, IPQLevel0Quote
{
    protected readonly ISyncLock SyncLock = new SpinLockLight();
    protected byte BooleanFields;
    private DateTime clientReceivedTime;
    private DateTime dispatchedTime;
    protected PQSourceTickerQuoteInfo? PQSourceTickerQuoteInfo;
    private PQSyncStatus pqSyncStatus = PQSyncStatus.OutOfSync;
    private DateTime processedTime;
    private decimal singlePrice;
    private DateTime socketReceivingTime;
    private DateTime sourceTime = DateTimeConstants.UnixEpoch;

    protected QuoteFieldUpdatedFlags UpdatedFlags;

    public PQLevel0Quote() { }

    public PQLevel0Quote(ISourceTickerQuoteInfo sourceTickerInfo) =>
        SourceTickerQuoteInfo = (IMutableSourceTickerQuoteInfo)sourceTickerInfo;

    public PQLevel0Quote(ILevel0Quote toClone)
    {
        singlePrice = toClone.SinglePrice;
        IsReplay = toClone.IsReplay;
        sourceTime = toClone.SourceTime;
        ClientReceivedTime = toClone.ClientReceivedTime;
        SourceTickerQuoteInfo = new PQSourceTickerQuoteInfo(toClone.SourceTickerQuoteInfo!);
        if (toClone is IPQLevel0Quote pqLevel0Quote)
        {
            SourceTickerQuoteInfo = pqLevel0Quote.SourceTickerQuoteInfo;
            PQSequenceId = pqLevel0Quote.PQSequenceId;
            PQSyncStatus = pqLevel0Quote.PQSyncStatus;
            LastPublicationTime = pqLevel0Quote.LastPublicationTime;
            SocketReceivingTime = pqLevel0Quote.SocketReceivingTime;
            ProcessedTime = pqLevel0Quote.ProcessedTime;
            DispatchedTime = pqLevel0Quote.DispatchedTime;
        }

        SyncLock = new SpinLockLight();
        if (toClone is PQLevel0Quote plLevel0Quote) UpdatedFlags = plLevel0Quote.UpdatedFlags;
    }

    protected string Level0ToStringMembers =>
        $"{nameof(PQSourceTickerQuoteInfo)}: {PQSourceTickerQuoteInfo}, {nameof(PQSequenceId)}: {PQSequenceId}, " +
        $"{nameof(PQSyncStatus)}: {PQSyncStatus}, {nameof(LastPublicationTime)}: {LastPublicationTime}, " +
        $"{nameof(SourceTime)}: {SourceTime}, {nameof(DispatchedTime)}: {DispatchedTime}, " +
        $"{nameof(ProcessedTime)}: {ProcessedTime}, {nameof(IsSourceTimeDateUpdated)}: {IsSourceTimeDateUpdated}, " +
        $"{nameof(IsSourceTimeSubHourUpdated)}: {IsSourceTimeSubHourUpdated}, " +
        $"{nameof(IsSyncStatusUpdated)}: {IsSyncStatusUpdated}, {nameof(SinglePrice)}: {SinglePrice}, " +
        $"{nameof(IsSinglePriceUpdated)}: {IsSinglePriceUpdated}, {nameof(IsReplay)}: {IsReplay}, " +
        $"{nameof(IsReplayUpdated)}: {IsReplayUpdated}, {nameof(HasUpdates)}: {HasUpdates}";

    public uint MessageId => (uint)PricingMessageIds.PricingMessage;

    public uint PQSequenceId { get; set; }
    public ISyncLock Lock => SyncLock;

    public DateTime LastPublicationTime { get; set; }

    public byte Version => 1;

    ISourceTickerQuoteInfo? ILevel0Quote.SourceTickerQuoteInfo => PQSourceTickerQuoteInfo;

    public IMutableSourceTickerQuoteInfo? SourceTickerQuoteInfo
    {
        get => PQSourceTickerQuoteInfo;
        set
        {
            if (ReferenceEquals(value, PQSourceTickerQuoteInfo)) return;
            if (value is PQSourceTickerQuoteInfo pqSourceTickerInfo) // share SourceTickerInfo if possible
                PQSourceTickerQuoteInfo = pqSourceTickerInfo;
            if (PQSourceTickerQuoteInfo is PQSourceTickerQuoteInfo pqSourceTickInfo)
            {
                PQSourceTickerQuoteInfo.CopyFrom((IUniqueSourceTickerIdentifier)pqSourceTickInfo);
                return;
            }

            PQSourceTickerQuoteInfo = ConvertToPQSourceTickerInfo(value!, PQSourceTickerQuoteInfo);
        }
    }

    public IPQLevel0Quote? Previous { get; set; }
    public IPQLevel0Quote? Next { get; set; }

    public virtual DateTime SourceTime
    {
        get => sourceTime;
        set
        {
            if (sourceTime == value) return;
            IsSourceTimeDateUpdated |= sourceTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsSourceTimeSubHourUpdated |= sourceTime.GetSubHourComponent() != value.GetSubHourComponent();
            sourceTime = value;
        }
    }

    public DateTime DispatchedTime
    {
        get => dispatchedTime;
        set => dispatchedTime = value;
    }

    public DateTime ProcessedTime
    {
        get => processedTime;
        set => processedTime = value;
    }

    public DateTime SocketReceivingTime
    {
        get => socketReceivingTime;
        set => socketReceivingTime = value;
    }

    public DateTime ClientReceivedTime
    {
        get => clientReceivedTime;
        set => clientReceivedTime = value;
    }

    public bool IsSourceTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag;
            else if (IsSourceTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag;
        }
    }

    public bool IsSourceTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentSubSecondUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentSubSecondUpdatedFlag;
            else if (IsSourceTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentSubSecondUpdatedFlag;
        }
    }

    public PQSyncStatus PQSyncStatus
    {
        get => pqSyncStatus;
        set
        {
            if (pqSyncStatus == value) return;
            IsSyncStatusUpdated = true;
            pqSyncStatus = value;
        }
    }

    public bool IsSyncStatusUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.PublicationStatusUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.PublicationStatusUpdatedFlag;
            else if (IsSyncStatusUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.PublicationStatusUpdatedFlag;
        }
    }

    public virtual decimal SinglePrice
    {
        get => singlePrice;
        set
        {
            if (singlePrice == value) return;
            IsSinglePriceUpdated = true;
            singlePrice = value;
        }
    }

    public bool IsSinglePriceUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SinglePriceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SinglePriceUpdatedFlag;
            else
                UpdatedFlags ^= QuoteFieldUpdatedFlags.SinglePriceUpdatedFlag;
        }
    }

    public bool IsReplay
    {
        get => (BooleanFields & PQBooleanValues.IsReplayFlag) > 0;
        set
        {
            if (IsReplay == value) return;
            IsReplayUpdated = true;
            if (value)
                BooleanFields |= PQBooleanValues.IsReplayFlag;
            else if (IsReplay) BooleanFields ^= PQBooleanValues.IsReplayFlag;
        }
    }

    public bool IsReplayUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.IsReplayUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.IsReplayUpdatedFlag;
            else if (IsReplayUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.IsReplayUpdatedFlag;
        }
    }

    public virtual bool HasUpdates
    {
        get => UpdatedFlags > 0;
        set
        {
            if (PQSourceTickerQuoteInfo != null) PQSourceTickerQuoteInfo.HasUpdates = value;
            UpdatedFlags = value ? UpdatedFlags.AllFlags() : 0;
        }
    }

    public virtual void ResetFields()
    {
        PQSequenceId = 0;
        singlePrice = 0;
        sourceTime = DateTimeConstants.UnixEpoch;
        DispatchedTime = LastPublicationTime = ProcessedTime = SocketReceivingTime = DateTime.MinValue;
        PQSyncStatus = PQSyncStatus.OutOfSync;
        IsReplay = false;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var persistenceOrReplay = (updateStyle & (UpdateStyle.Persistence | UpdateStyle.Replay)) > 0;
        if (persistenceOrReplay)
        {
            yield return new PQFieldUpdate(PQFieldKeys.PQSequenceId, PQSequenceId);

            yield return new PQFieldUpdate(PQFieldKeys.SocketReceivingDateTime
                , SocketReceivingTime.GetHoursFromUnixEpoch());
            var fifthByte = SocketReceivingTime.GetSubHourComponent().BreakLongToByteAndUint(out var lower4Bytes);
            yield return new PQFieldUpdate(PQFieldKeys.SocketReceivingSubHourTime, lower4Bytes, fifthByte);

            yield return new PQFieldUpdate(PQFieldKeys.ProcessedDateTime, ProcessedTime.GetHoursFromUnixEpoch());
            fifthByte = ProcessedTime.GetSubHourComponent().BreakLongToByteAndUint(out lower4Bytes);
            yield return new PQFieldUpdate(PQFieldKeys.ProcessedSubHourTime, lower4Bytes, fifthByte);

            yield return new PQFieldUpdate(PQFieldKeys.DispatchedDateTime, DispatchedTime.GetHoursFromUnixEpoch());
            fifthByte = DispatchedTime.GetSubHourComponent().BreakLongToByteAndUint(out lower4Bytes);
            yield return new PQFieldUpdate(PQFieldKeys.DispatchedSubHourTime, lower4Bytes, fifthByte);

            yield return new PQFieldUpdate(PQFieldKeys.ClientReceivedDateTime
                , ClientReceivedTime.GetHoursFromUnixEpoch());
            fifthByte = ClientReceivedTime.GetSubHourComponent().BreakLongToByteAndUint(out lower4Bytes);
            yield return new PQFieldUpdate(PQFieldKeys.ClientReceivedSubHourTime, lower4Bytes, fifthByte);
        }

        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        if (PQSourceTickerQuoteInfo != null)
            foreach (var field in PQSourceTickerQuoteInfo.GetDeltaUpdateFields(snapShotTime, updateStyle,
                         quotePublicationPrecisionSettings))
                yield return field;
        if (!updatedOnly || IsSyncStatusUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.PQSyncStatus, (byte)PQSyncStatus);
        if (!updatedOnly || IsSinglePriceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.SinglePrice, SinglePrice,
                PQSourceTickerQuoteInfo?.PriceScalingPrecision ?? 1);
        if (!updatedOnly || IsSourceTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.SourceSentDateTime, sourceTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsSourceTimeSubHourUpdated)
        {
            var fifthByte = sourceTime.GetSubHourComponent().BreakLongToByteAndUint(out var lower4Bytes);
            yield return new PQFieldUpdate(PQFieldKeys.SourceSentSubHourTime, lower4Bytes, fifthByte);
        }

        if (!updatedOnly || IsBooleanFlagsChanged())
            yield return new PQFieldUpdate(PQFieldKeys.QuoteBooleanFlags, GenerateBooleanFlags());
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        var infoResult = PQSourceTickerQuoteInfo!.UpdateField(pqFieldUpdate);
        if (infoResult > 0) return infoResult;
        switch (pqFieldUpdate.Id)
        {
            case PQFieldKeys.PQSequenceId:
                PQSequenceId = pqFieldUpdate.Value;
                return 0;
            case PQFieldKeys.SinglePrice:
                IsSinglePriceUpdated = true;
                SinglePrice = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            case PQFieldKeys.SourceSentDateTime:
                IsSourceTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.SourceSentSubHourTime:
                IsSourceTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref sourceTime,
                    pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.QuoteBooleanFlags:
                SetBooleanFields(pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.PQSyncStatus:
                PQSyncStatus = (PQSyncStatus)pqFieldUpdate.Value;
                return 0;
            case PQFieldKeys.SocketReceivingDateTime:
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref socketReceivingTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.SocketReceivingSubHourTime:
                PQFieldConverters.UpdateSubHourComponent(ref socketReceivingTime,
                    pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.ProcessedDateTime:
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref processedTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.ProcessedSubHourTime:
                PQFieldConverters.UpdateSubHourComponent(ref processedTime,
                    pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.DispatchedDateTime:
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref dispatchedTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.DispatchedSubHourTime:
                PQFieldConverters.UpdateSubHourComponent(ref dispatchedTime,
                    pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
            case PQFieldKeys.ClientReceivedDateTime:
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref clientReceivedTime, pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.ClientReceivedSubHourTime:
                PQFieldConverters.UpdateSubHourComponent(ref clientReceivedTime,
                    pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                return 0;
        }

        return -1;
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
    {
        if (PQSourceTickerQuoteInfo != null)
            foreach (var field in PQSourceTickerQuoteInfo.GetStringUpdates(snapShotTime, updatedStyle))
                yield return field;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate updates) =>
        PQSourceTickerQuoteInfo != null && PQSourceTickerQuoteInfo.UpdateFieldString(updates);

    public override ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQLevel0Quote ipq0)
        {
            if (PQSourceTickerQuoteInfo != null)
                PQSourceTickerQuoteInfo.CopyFrom(ipq0.SourceTickerQuoteInfo!);
            else
                SourceTickerQuoteInfo = ipq0.SourceTickerQuoteInfo;
            // only copy if changed
            if (ipq0.IsSourceTimeDateUpdated)
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceTime, ipq0.SourceTime.GetHoursFromUnixEpoch());
            if (ipq0.IsSourceTimeSubHourUpdated)
                PQFieldConverters.UpdateSubHourComponent(ref sourceTime, ipq0.SourceTime.GetSubHourComponent());
            if (ipq0.IsReplayUpdated) IsReplay = ipq0.IsReplay;
            if (ipq0.IsSinglePriceUpdated) SinglePrice = ipq0.SinglePrice;
            if (ipq0.IsSyncStatusUpdated) PQSyncStatus = ipq0.PQSyncStatus;
            //PQ tracks its own changes only copy explicit changes

            PQSequenceId = ipq0.PQSequenceId;
            SocketReceivingTime = ipq0.SocketReceivingTime;
            DispatchedTime = ipq0.DispatchedTime;
            ProcessedTime = ipq0.ProcessedTime;
            ClientReceivedTime = ipq0.ClientReceivedTime;

            if (source is PQLevel0Quote pq0)
            {
                UpdatedFlags = pq0.UpdatedFlags;
                LastPublicationTime = pq0.LastPublicationTime;
            }
        }
        else
        {
            ClientReceivedTime = source.ClientReceivedTime;
            SourceTickerQuoteInfo = source.SourceTickerQuoteInfo as IMutableSourceTickerQuoteInfo;
            SourceTime = source.SourceTime;
            IsReplay = source.IsReplay;
            SinglePrice = source.SinglePrice;
        }

        return this;
    }

    IVersionedMessage IStoreState<IVersionedMessage>.CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags) =>
        (IVersionedMessage)CopyFrom((ILevel0Quote)source, copyMergeFlags);

    public IReusableObject<IVersionedMessage> CopyFrom(IReusableObject<IVersionedMessage> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IVersionedMessage)CopyFrom((ILevel0Quote)source, copyMergeFlags);

    public virtual void EnsureRelatedItemsAreConfigured(ILevel0Quote? referenceInstance)
    {
        if (referenceInstance?.SourceTickerQuoteInfo is IPQSourceTickerQuoteInfo pqSrcTkrQuoteInfo)
            SourceTickerQuoteInfo = pqSrcTkrQuoteInfo;
    }

    ILevel0Quote ICloneable<ILevel0Quote>.Clone() => Clone();
    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IMutableLevel0Quote IMutableLevel0Quote.Clone() => Clone();

    public override IPQLevel0Quote Clone() =>
        (IPQLevel0Quote?)Recycler?.Borrow<PQLevel0Quote>().CopyFrom(this) ?? new PQLevel0Quote(this);

    public virtual bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var isReplaySame = IsReplay == other.IsReplay;
        var tickerQuoteInfoSame = PQSourceTickerQuoteInfo?.AreEquivalent(other.SourceTickerQuoteInfo, exactTypes) ??
                                  other.SourceTickerQuoteInfo == null;
        var singlePriceSame = singlePrice == other.SinglePrice;
        var sourceTimeSame = SourceTime.Equals(other.SourceTime);

        var sequenceIdSame = true;
        var publicationStatusSame = true;

        var socketReceivingTimeSame = true;
        var lastPubTimeSame = true;
        var processingTimeSame = true;
        var dispatchTimeSame = true;
        var clientReceivedSame = true;
        var updatedFlagsSame = true;
        var booleanFieldsSame = true;
        var pqLevel0Quote = other as PQLevel0Quote;
        if (exactTypes)
        {
            sequenceIdSame = PQSequenceId == pqLevel0Quote!.PQSequenceId;
            publicationStatusSame = PQSyncStatus == pqLevel0Quote.PQSyncStatus;

            socketReceivingTimeSame = SocketReceivingTime.Equals(pqLevel0Quote.SocketReceivingTime);
            lastPubTimeSame = LastPublicationTime.Equals(pqLevel0Quote.LastPublicationTime);
            processingTimeSame = ProcessedTime.Equals(pqLevel0Quote.ProcessedTime);
            dispatchTimeSame = DispatchedTime.Equals(pqLevel0Quote.DispatchedTime);
            clientReceivedSame = ClientReceivedTime == other.ClientReceivedTime;

            updatedFlagsSame = UpdatedFlags == pqLevel0Quote.UpdatedFlags;
            booleanFieldsSame = BooleanFields == pqLevel0Quote.BooleanFields;
        }

        return clientReceivedSame && isReplaySame && tickerQuoteInfoSame && singlePriceSame && sourceTimeSame
               && updatedFlagsSame && booleanFieldsSame && dispatchTimeSame && processingTimeSame && lastPubTimeSame
               && socketReceivingTimeSame && sequenceIdSame && publicationStatusSame;
    }

    private PQSourceTickerQuoteInfo ConvertToPQSourceTickerInfo(ISourceTickerQuoteInfo value,
        PQSourceTickerQuoteInfo? originalQuoteInfo)
    {
        if (originalQuoteInfo == null)
        {
            originalQuoteInfo = new PQSourceTickerQuoteInfo(value);
            return originalQuoteInfo;
        }

        originalQuoteInfo.CopyFrom(value);
        return originalQuoteInfo;
    }

    protected virtual bool IsBooleanFlagsChanged() => IsReplayUpdated;

    protected virtual uint GenerateBooleanFlags() => IsReplay ? (uint)PQBooleanValues.IsReplayFlag : 0;

    protected virtual void SetBooleanFields(uint booleanFlags)
    {
        IsReplay = (booleanFlags & PQBooleanValues.IsReplayFlag) == PQBooleanValues.IsReplayFlag;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((PQLevel0Quote?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)UpdatedFlags;
            hashCode = (hashCode * 397) ^ (int)PQSequenceId;
            hashCode = (hashCode * 397) ^ SourceTickerQuoteInfo?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ BooleanFields.GetHashCode();
            hashCode = (hashCode * 397) ^ singlePrice.GetHashCode();
            hashCode = (hashCode * 397) ^ sourceTime.GetHashCode();
            hashCode = (hashCode * 397) ^ LastPublicationTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SocketReceivingTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ProcessedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ DispatchedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ IsReplay.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)PQSyncStatus;
            hashCode = (hashCode * 397) ^ (SourceTickerQuoteInfo?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level0ToStringMembers})";
}
