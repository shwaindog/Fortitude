// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.TimeSeries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

public interface IPQTickInstant : IMutableTickInstant, IPQSupportsFieldUpdates<ITickInstant>,
    IPQSupportsStringUpdates<ITickInstant>, IRelatedItems<ITickInstant>, IRelatedItems<ISourceTickerInfo>
{
    bool IsSourceTimeDateUpdated    { get; set; }
    bool IsSourceTimeSub2MinUpdated { get; set; }
    bool IsSingleValueUpdated       { get; set; }
    bool IsReplayUpdated            { get; set; }

    void ResetFields();

    new IPQTickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);

    new IPQTickInstant Clone();
}

public interface IPQPublishableTickInstant : IPQTickInstant, IDoublyLinkedListNode<IPQPublishableTickInstant>, IMutablePublishableTickInstant,
    IPQSupportsFieldUpdates<IPublishableTickInstant>, IPQSupportsStringUpdates<IPublishableTickInstant>, IVersionedMessage
{
    PQMessageFlags? OverrideSerializationFlags { get; set; }

    bool IsSocketReceivedTimeDateUpdated    { get; set; }
    bool IsSocketReceivedTimeSub2MinUpdated { get; set; }
    bool IsProcessedTimeDateUpdated         { get; set; }
    bool IsProcessedTimeSub2MinUpdated      { get; set; }
    bool IsDispatchedTimeDateUpdated        { get; set; }
    bool IsDispatchedTimeSub2MinUpdated     { get; set; }
    bool IsClientReceivedTimeDateUpdated    { get; set; }
    bool IsClientReceivedTimeSub2MinUpdated { get; set; }
    bool IsFeedSyncStatusUpdated            { get; set; }

    ISyncLock Lock                { get; }
    uint      PQSequenceId        { get; set; }
    DateTime  LastPublicationTime { get; set; }
    DateTime  SocketReceivingTime { get; set; }
    DateTime  ProcessedTime       { get; set; }
    DateTime  DispatchedTime      { get; set; }

    new FeedSyncStatus             FeedSyncStatus { get; set; }
    new IPQPublishableTickInstant? Next           { get; set; }
    new IPQPublishableTickInstant? Previous       { get; set; }

    new IPQSourceTickerInfo? SourceTickerInfo { get; set; }

    new IPQTickInstant AsNonPublishable { get; }

    new bool HasUpdates { get; set; }

    new int UpdateField(PQFieldUpdate pqFieldUpdate);

    new IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null);

    new IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags);

    new bool UpdateFieldString(PQFieldStringUpdate stringUpdate);

    new IPQPublishableTickInstant CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags);

    new IPQPublishableTickInstant Clone();
}

public class PQTickInstant : ReusableObject<ITickInstant>, IPQTickInstant, ICloneable<IPQTickInstant>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublishableTickInstant));

    protected QuoteFieldUpdatedFlags UpdatedFlags;
    protected PQBooleanValues        BooleanFields;

    protected PQFieldFlags PriceScalingPrecision;
    protected PQFieldFlags VolumeScalingPrecision;

    protected uint NumOfUpdates = uint.MaxValue;

    private decimal  singleValue;
    private DateTime sourceTime;

    public PQTickInstant()
    {
        if (GetType() == typeof(PQPublishableTickInstant)) NumOfUpdates = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQTickInstant(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m)
    {
    }

    public PQTickInstant(ISourceTickerInfo sourceTickerInfo, decimal singlePrice = 0m, bool isReplay = false, DateTime? sourceTime  = null)
    {
        SingleTickValue = singlePrice;
        IsReplay        = isReplay;
        SourceTime      = sourceTime ?? DateTime.MinValue;
        
        PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(sourceTickerInfo.RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor(Math.Min(sourceTickerInfo.MinSubmitSize, sourceTickerInfo.IncrementSize));

        if (GetType() == typeof(PQTickInstant)) NumOfUpdates = 0;
    }

    public PQTickInstant(ITickInstant toClone)
    {
        SingleTickValue = toClone.SingleTickValue;
        IsReplay = toClone.IsReplay;
        SourceTime = toClone.SourceTime;

        SetFlagsSame(toClone);
        if (GetType() == typeof(PQTickInstant)) NumOfUpdates = 0;
    }

    public virtual string QuoteToStringMembers => $"{nameof(SingleTickValue)}: {SingleTickValue}, {nameof(IsReplay)}, {IsReplay}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";


    public override PQTickInstant Clone() =>
        Recycler?.Borrow<PQTickInstant>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQTickInstant(this);

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    IPQTickInstant IPQTickInstant.Clone() => Clone();

    IPQTickInstant ICloneable<IPQTickInstant>.Clone() => Clone();


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual decimal SingleTickValue
    {
        get => singleValue;
        set
        {
            IsSingleValueUpdated |= singleValue != value || NumOfUpdates == 0;
            singleValue          = value;
        }
    }


    [JsonIgnore]
    public bool IsSingleValueUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SingleValueUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SingleValueUpdatedFlag;
            else
                UpdatedFlags &= ~QuoteFieldUpdatedFlags.SingleValueUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsReplay
    {
        get => (BooleanFields & PQBooleanValues.IsReplaySetFlag) > 0;
        set
        {
            IsReplayUpdated |= IsReplay != value || NumOfUpdates == 0;
            if (value)
                BooleanFields |= PQBooleanValues.IsReplaySetFlag;

            else if (IsReplay) BooleanFields ^= PQBooleanValues.IsReplaySetFlag;
        }
    }

    [JsonIgnore]
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
    

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual DateTime SourceTime
    {
        get => sourceTime;
        set
        {
            if (sourceTime == value) return;
            IsSourceTimeDateUpdated    |= sourceTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsSourceTimeSub2MinUpdated |= sourceTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            sourceTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        sourceTime += toChangeBy;
    }
    
    [JsonIgnore]
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

    [JsonIgnore]
    public bool IsSourceTimeSub2MinUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentSub2MinUpdatedFlag;

            else if (IsSourceTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != QuoteFieldUpdatedFlags.None;
        set
        {
            UpdatedFlags = value ? UpdatedFlags.AllFlags() : QuoteFieldUpdatedFlags.None;
            if (value) return;
            BooleanFields &= PQBooleanValues.BooleanValuesMask;
        }
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ITickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
        return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    public uint UpdateCount => NumOfUpdates;

    public virtual void UpdateComplete()
    {
        if (HasUpdates) NumOfUpdates++;
        HasUpdates = false;
    }

    public virtual void ResetFields()
    {
        NumOfUpdates  = 0;
        singleValue   = 0m;
        sourceTime    = DateTime.MinValue;
        BooleanFields = PQBooleanValues.DefaultEmptyQuoteFlags;
        UpdatedFlags  = QuoteFieldUpdatedFlags.None;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;

        if (!updatedOnly || IsSingleValueUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SingleTickValue, SingleTickValue, quotePublicationPrecisionSettings?.PriceScalingPrecision ?? PriceScalingPrecision);
        if (!updatedOnly || IsSourceTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteSentDateTime, sourceTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsSourceTimeSub2MinUpdated)
        {
            var extended = sourceTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteSentSub2MinTime, lower4Bytes, extended);
        }
        if (!updatedOnly || IsBooleanFlagsChanged())
        {
            var booleanFields = GenerateBooleanFlags(!updatedOnly);
            yield return new PQFieldUpdate(PQFeedFields.QuoteBooleanFlags, (uint)booleanFields);
        }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.SingleTickValue:
                IsSingleValueUpdated = true; // incase of reset and sending 0;
                SingleTickValue      = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                return 0;
            case PQFeedFields.SourceQuoteSentDateTime:
                IsSourceTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceTime, pqFieldUpdate.Payload);
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQFeedFields.SourceQuoteSentSub2MinTime:
                IsSourceTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref sourceTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQFeedFields.QuoteBooleanFlags:
                SetBooleanFields((PQBooleanValues)pqFieldUpdate.Payload);
                return 0;
        }

        return -1;
    }

    public virtual void EnsureRelatedItemsAreConfigured(ITickInstant? item)
    {
        if (item is PQTickInstant pqTickInstant)
        {
            PriceScalingPrecision  = pqTickInstant.PriceScalingPrecision;
            VolumeScalingPrecision = pqTickInstant.VolumeScalingPrecision;
        }
        if (item is IPublishableTickInstant { SourceTickerInfo: not null } pubInstance)
        {
            PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(pubInstance.SourceTickerInfo.RoundingPrecision);
            VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor
                ( Math.Min(pubInstance.SourceTickerInfo.MinSubmitSize, pubInstance.SourceTickerInfo.IncrementSize));
        }
    }

    public virtual void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? srcTickerInfo)
    {
        if (srcTickerInfo is IPQSourceTickerInfo pqSrcTickerInfo)
        {
            PriceScalingPrecision  = pqSrcTickerInfo.PriceScalingPrecision;
            VolumeScalingPrecision = pqSrcTickerInfo.VolumeScalingPrecision;
        } else if (srcTickerInfo is not null)
        {
            PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(srcTickerInfo.RoundingPrecision);
            VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor
                ( Math.Min(srcTickerInfo.MinSubmitSize, srcTickerInfo.IncrementSize));
        }
    }


    public virtual bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var singlePriceSame = singleValue == other.SingleTickValue;
        var sourceTimeSame = SourceTime == other.SourceTime;
        var isReplaySame = IsReplay == other.IsReplay;

        var updatedFlagsSame  = true;
        var booleanFieldsSame = true;
        var pqTickInstant     = other as PQTickInstant;
        if (exactTypes)
        {
            updatedFlagsSame  = UpdatedFlags == pqTickInstant.UpdatedFlags;
            booleanFieldsSame = BooleanFields == pqTickInstant.BooleanFields;
        }

        var allAreSame = singlePriceSame && isReplaySame && sourceTimeSame && updatedFlagsSame && booleanFieldsSame;
        return allAreSame;
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        return [];
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => false;

    IPQTickInstant IPQTickInstant.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQTickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQTickInstant ipq0)
        {
            // only copy if changed
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (ipq0.IsSingleValueUpdated || isFullReplace)
            {
                IsSingleValueUpdated = true;
                SingleTickValue      = ipq0.SingleTickValue;
            }
            if (ipq0.IsSourceTimeDateUpdated || isFullReplace)
            {
                var originalSourceTime = sourceTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceTime, ipq0.SourceTime.Get2MinIntervalsFromUnixEpoch());
                IsSourceTimeDateUpdated = originalSourceTime != sourceTime;
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
            }
            if (ipq0.IsSourceTimeSub2MinUpdated || isFullReplace)
            {
                var originalSourceTime = sourceTime;
                PQFieldConverters.UpdateSub2MinComponent(ref sourceTime, ipq0.SourceTime.GetSub2MinComponent());
                IsSourceTimeSub2MinUpdated = originalSourceTime != sourceTime;
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
            }
            if (ipq0.IsReplayUpdated || isFullReplace)
            {
                IsReplayUpdated = true;
                IsReplay        = ipq0.IsReplay;
            }

            if(isFullReplace) SetFlagsSame(ipq0);
        }
        else
        {
            SingleTickValue = source.SingleTickValue;
            SourceTime = source.SourceTime;
            IsReplay = source.IsReplay;
        }

        return this;
    }


    public virtual PQTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        if (toSet is IPQSourceTickerInfo pqSourceTickerInfo)
        {
            PriceScalingPrecision  = pqSourceTickerInfo.PriceScalingPrecision;
            VolumeScalingPrecision = pqSourceTickerInfo.VolumeScalingPrecision;
        }
        else
        {
            PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(toSet.RoundingPrecision);
            VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor(Math.Min(toSet.MinSubmitSize, toSet.IncrementSize));
        }
        return this;
    }

    protected virtual bool IsBooleanFlagsChanged() => IsReplayUpdated;
    
    protected virtual PQBooleanValues GenerateBooleanFlags(bool fullUpdate) =>
        (IsReplayUpdated || fullUpdate ? PQBooleanValues.IsReplayUpdatedFlag : 0)
      | (IsReplay ? PQBooleanValues.IsReplaySetFlag : 0);

    protected virtual void SetBooleanFields(PQBooleanValues booleanFlags)
    {
        IsReplayUpdated = (booleanFlags & PQBooleanValues.IsReplayUpdatedFlag) > 0;
        if (IsReplayUpdated) IsReplay = (booleanFlags & PQBooleanValues.IsReplaySetFlag) == PQBooleanValues.IsReplaySetFlag;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)UpdatedFlags;
            hashCode = (hashCode * 397) ^ BooleanFields.GetHashCode();
            hashCode = (hashCode * 397) ^ singleValue.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {UpdatedFlagsToString})";

    protected void SetFlagsSame(ITickInstant toCopyFlags)
    {
        if (toCopyFlags is PQTickInstant pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }
}

public class PQPublishableTickInstant : ReusableObject<IPublishableTickInstant>, IPQPublishableTickInstant, ICloneable<PQPublishableTickInstant>
  , IDoublyLinkedListNode<PQPublishableTickInstant>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublishableTickInstant));

    protected IPQTickInstant PQQuoteContainer;

    protected PublishableQuoteFieldUpdatedFlags UpdatedFlags;

    protected readonly ISyncLock SyncLock = new SpinLockLight();

    private DateTime clientReceivedTime;
    private DateTime dispatchedTime;

    private   FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good;
    protected uint           NumOfUpdates   = uint.MaxValue;

    protected IPQSourceTickerInfo? PQSourceTickerInfo;

    private DateTime processedTime;
    private DateTime socketReceivingTime;

    public PQPublishableTickInstant()
    {
        PQQuoteContainer = CreateEmptyQuoteContainerInstant();

        if (GetType() == typeof(PQPublishableTickInstant)) NumOfUpdates = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQPublishableTickInstant(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    protected PQPublishableTickInstant(IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo)
        : this(initializedQuoteContainer, sourceTickerInfo, feedSyncStatus: FeedSyncStatus.Good)
    {
    }

    public PQPublishableTickInstant
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false,
        FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, decimal singlePrice = 0m, DateTime? clientReceivedTime = null)
        : this(new PQTickInstant(sourceTickerInfo, singlePrice, isReplay, sourceTime), sourceTickerInfo, feedSyncStatus, clientReceivedTime) { }

    protected PQPublishableTickInstant
    (IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, DateTime? clientReceivedTime = null)
    {
        PQQuoteContainer = initializedQuoteContainer ?? CreateQuoteContainerFromTickerInfo(sourceTickerInfo);

        if (sourceTickerInfo is IPQSourceTickerInfo pqSourceTickerInfo)
        {
            SourceTickerInfo = pqSourceTickerInfo;
        }
        else
        {
            SourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);
        }
        PQQuoteContainer.EnsureRelatedItemsAreConfigured(SourceTickerInfo);
        FeedSyncStatus     = feedSyncStatus;
        ClientReceivedTime = clientReceivedTime ?? DateTime.MinValue;

        if (GetType() == typeof(PQPublishableTickInstant)) NumOfUpdates = 0;
    }

    public PQPublishableTickInstant(IPublishableTickInstant toClone) : this(toClone, null) { }

    public PQPublishableTickInstant(IPublishableTickInstant toClone, IPQTickInstant? initializedQuoteContainer)
    {
        if (toClone is PQPublishableTickInstant pqToClone)
        {
            PQQuoteContainer = initializedQuoteContainer ?? pqToClone.PQQuoteContainer.Clone();
        }
        else
        {
            PQQuoteContainer = initializedQuoteContainer ?? CreateCloneQuoteContainerInstant(toClone);
        }
        IsReplay           = toClone.IsReplay;
        ClientReceivedTime = toClone.ClientReceivedTime;
        FeedSyncStatus     = toClone.FeedSyncStatus;

        SourceTickerInfo = new PQSourceTickerInfo(toClone.SourceTickerInfo!);
        if (toClone is IPQPublishableTickInstant ipqTickInstant)
        {
            OverrideSerializationFlags = ipqTickInstant.OverrideSerializationFlags;

            SourceTickerInfo    = ipqTickInstant.SourceTickerInfo;
            PQSequenceId        = ipqTickInstant.PQSequenceId;
            FeedSyncStatus      = ipqTickInstant.FeedSyncStatus;
            LastPublicationTime = ipqTickInstant.LastPublicationTime;
            SocketReceivingTime = ipqTickInstant.SocketReceivingTime;
            ProcessedTime       = ipqTickInstant.ProcessedTime;
            DispatchedTime      = ipqTickInstant.DispatchedTime;
        }

        SyncLock = new SpinLockLight();
        SetFlagsSame(toClone);
        if (GetType() == typeof(PQPublishableTickInstant)) NumOfUpdates = 0;
    }

    protected virtual IPQTickInstant CreateEmptyQuoteContainerInstant() => new PQTickInstant();

    protected virtual IPQTickInstant CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new PQTickInstant(tickInstant);

    protected virtual IPQTickInstant CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new PQTickInstant(tickerInfo);

    public virtual string QuoteToStringMembers =>
        $"{nameof(PQSourceTickerInfo)}: {PQSourceTickerInfo}, {nameof(PQSequenceId)}: {PQSequenceId}, " +
        $"{nameof(FeedSyncStatus)}: {FeedSyncStatus}, {nameof(LastPublicationTime)}: {LastPublicationTime}, " +
        $"{nameof(DispatchedTime)}: {DispatchedTime}, {nameof(ProcessedTime)}: {ProcessedTime}" +
        $"{nameof(IsFeedSyncStatusUpdated)}: {IsFeedSyncStatusUpdated}, {nameof(HasUpdates)}: {HasUpdates}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    ITickInstant IPublishableTickInstant.              AsNonPublishable => AsNonPublishable;
    IMutableTickInstant IMutablePublishableTickInstant.AsNonPublishable => AsNonPublishable;
    public virtual IPQTickInstant                              AsNonPublishable => PQQuoteContainer;

    public override PQPublishableTickInstant Clone() =>
        Recycler?.Borrow<PQPublishableTickInstant>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQPublishableTickInstant(this);

    IPublishableTickInstant ICloneable<IPublishableTickInstant>.Clone() => Clone();

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IMutablePublishableTickInstant IMutablePublishableTickInstant.Clone() => Clone();

    IPQPublishableTickInstant IPQPublishableTickInstant.Clone() => Clone();

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    IPQTickInstant IPQTickInstant.Clone() => Clone();


    [JsonIgnore]
    public PQPublishableTickInstant? Previous
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous as PQPublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public PQPublishableTickInstant? Next
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next as PQPublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next = value;
    }

    IVersionedMessage ITransferState<IVersionedMessage>.CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    [JsonIgnore]
    IPQPublishableTickInstant? IPQPublishableTickInstant.Previous
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous as IPQPublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableTickInstant? IPQPublishableTickInstant.Next
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next as IPQPublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQPublishableTickInstant? IDoublyLinkedListNode<IPQPublishableTickInstant>.Previous
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous as IPQPublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableTickInstant? IDoublyLinkedListNode<IPQPublishableTickInstant>.Next
    {
        get => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next as IPQPublishableTickInstant;
        set => ((IDoublyLinkedListNode<IPublishableTickInstant>)this).Next = value;
    }

    [JsonIgnore] public virtual TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.SingleValue;

    [JsonIgnore] public PQMessageFlags? OverrideSerializationFlags { get; set; }

    [JsonIgnore] public uint MessageId => (uint)PQMessageIds.Quote;

    public uint PQSequenceId { get; set; }

    [JsonIgnore] public ISyncLock Lock => SyncLock;

    public DateTime LastPublicationTime { get; set; }

    [JsonIgnore] public byte Version => 1;

    [JsonIgnore] ISourceTickerInfo? IPublishableTickInstant.SourceTickerInfo => PQSourceTickerInfo;

    ISourceTickerInfo? IMutablePublishableTickInstant.SourceTickerInfo
    {
        get => SourceTickerInfo;
        set => SourceTickerInfo = ConvertToPQSourceTickerInfo(value!, PQSourceTickerInfo);
    }

    public IPQSourceTickerInfo? SourceTickerInfo
    {
        get => PQSourceTickerInfo;
        set
        {
            if (ReferenceEquals(value, PQSourceTickerInfo)) return;
            if (value is PQSourceTickerInfo pqSourceTickerInfo) // share SourceTickerInfo if possible
                PQSourceTickerInfo = pqSourceTickerInfo;
            if (value != null && PQSourceTickerInfo != null)
            {
                PQSourceTickerInfo.CopyFrom(value);
                return;
            }

            PQSourceTickerInfo = ConvertToPQSourceTickerInfo(value!, PQSourceTickerInfo);
        }
    }

    [JsonIgnore] IPublishableTickInstant? IDoublyLinkedListNode<IPublishableTickInstant>.Previous { get; set; }
    [JsonIgnore] IPublishableTickInstant? IDoublyLinkedListNode<IPublishableTickInstant>.Next     { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime DispatchedTime
    {
        get => dispatchedTime;
        set
        {
            if (dispatchedTime == value) return;
            IsDispatchedTimeDateUpdated
                |= dispatchedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsDispatchedTimeSub2MinUpdated |= dispatchedTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            dispatchedTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ProcessedTime
    {
        get => processedTime;
        set
        {
            if (processedTime == value) return;
            IsProcessedTimeDateUpdated |= processedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsProcessedTimeSub2MinUpdated |= processedTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            processedTime = value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SocketReceivingTime
    {
        get => socketReceivingTime;
        set
        {
            IsSocketReceivedTimeDateUpdated
                |= socketReceivingTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsSocketReceivedTimeSub2MinUpdated
                |= socketReceivingTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            socketReceivingTime = value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ClientReceivedTime
    {
        get => clientReceivedTime;
        set
        {
            IsClientReceivedTimeDateUpdated
                |= clientReceivedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsClientReceivedTimeSub2MinUpdated
                |= clientReceivedTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            clientReceivedTime = value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag;

            else if (IsSocketReceivedTimeDateUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.SocketReceivedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.SocketReceivedSub2MinUpdatedFlag;

            else if (IsSocketReceivedTimeSub2MinUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.SocketReceivedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeDateUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag;

            else if (IsProcessedTimeDateUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.ProcessedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.ProcessedSub2MinUpdatedFlag;

            else if (IsProcessedTimeSub2MinUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.ProcessedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeDateUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag;

            else if (IsDispatchedTimeDateUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.DispatchedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.DispatchedSub2MinUpdatedFlag;

            else if (IsDispatchedTimeSub2MinUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.DispatchedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag;

            else if (IsClientReceivedTimeDateUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.ClientReceivedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.ClientReceivedSub2MinUpdatedFlag;

            else if (IsClientReceivedTimeSub2MinUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.ClientReceivedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsFeedSyncStatusUpdated
    {
        get => (UpdatedFlags & PublishableQuoteFieldUpdatedFlags.FeedSyncStatusFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= PublishableQuoteFieldUpdatedFlags.FeedSyncStatusFlag;

            else if (IsFeedSyncStatusUpdated) UpdatedFlags ^= PublishableQuoteFieldUpdatedFlags.FeedSyncStatusFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FeedSyncStatus FeedSyncStatus
    {
        get => feedSyncStatus;
        set
        {
            if (feedSyncStatus == value) return;
            IsFeedSyncStatusUpdated = true;
            feedSyncStatus          = value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual decimal SingleTickValue
    {
        get => PQQuoteContainer.SingleTickValue;
        set => PQQuoteContainer.SingleTickValue = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsSingleValueUpdated
    {
        get => PQQuoteContainer.IsSingleValueUpdated;
        set => PQQuoteContainer.IsSingleValueUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsReplay
    {
        get => PQQuoteContainer.IsReplay;
        set => PQQuoteContainer.IsReplay = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsReplayUpdated
    {
        get => PQQuoteContainer.IsReplayUpdated;
        set => PQQuoteContainer.IsReplayUpdated = value;
    }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceTime
    {
        get => PQQuoteContainer.SourceTime;
        set => PQQuoteContainer.SourceTime = value;
    }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsSourceTimeDateUpdated
    {
        get => PQQuoteContainer.IsSourceTimeDateUpdated;
        set => PQQuoteContainer.IsSourceTimeDateUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsSourceTimeSub2MinUpdated
    {
        get => PQQuoteContainer.IsSourceTimeSub2MinUpdated;
        set => PQQuoteContainer.IsSourceTimeSub2MinUpdated = value;
    }

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != PublishableQuoteFieldUpdatedFlags.None || PQQuoteContainer.HasUpdates;
        set
        {
            if (PQSourceTickerInfo != null) PQSourceTickerInfo.HasUpdates = value;
            UpdatedFlags                = value ? UpdatedFlags.AllFlags() : PublishableQuoteFieldUpdatedFlags.None;
            PQQuoteContainer.HasUpdates = value;
        }
    }

    public uint UpdateCount => NumOfUpdates;

    public virtual void UpdateComplete()
    {
        PQSourceTickerInfo?.UpdateComplete();
        if (HasUpdates) NumOfUpdates++;
        PQQuoteContainer?.UpdateComplete();
        HasUpdates = false;
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        PQQuoteContainer.IncrementTimeBy(toChangeBy);

        ClientReceivedTime  += toChangeBy;
        DispatchedTime      += toChangeBy;
        ProcessedTime       += toChangeBy;
        SocketReceivingTime += toChangeBy;
        ClientReceivedTime  += toChangeBy;
    }

    public virtual void ResetFields()
    {
        OverrideSerializationFlags = null;
        PQQuoteContainer.ResetFields();

        NumOfUpdates = 0;

        clientReceivedTime  = default;
        LastPublicationTime = default;
        socketReceivingTime = default;

        PQSequenceId   = 0;
        processedTime  = default;
        dispatchedTime = default;
        FeedSyncStatus = FeedSyncStatus.Good;
        UpdatedFlags   = PublishableQuoteFieldUpdatedFlags.None;
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;

        if (PQSourceTickerInfo != null)
            foreach (var field in PQSourceTickerInfo.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                          quotePublicationPrecisionSettings))
                yield return field;
        
        if (!updatedOnly || IsFeedSyncStatusUpdated) yield return new PQFieldUpdate(PQFeedFields.PQSyncStatus, (byte)FeedSyncStatus);

        var includeReceiverTimes = (messageFlags & StorageFlags.IncludeReceiverTimes) > 0;
        if (includeReceiverTimes)
        {
            if (!updatedOnly || IsSocketReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientSocketReceivingDateTime
                                             , SocketReceivingTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsSocketReceivedTimeSub2MinUpdated)
            {
                var extended = SocketReceivingTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientSocketReceivingSub2MinTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsProcessedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientProcessedDateTime, ProcessedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsProcessedTimeSub2MinUpdated)
            {
                var extended = ProcessedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientProcessedSub2MinTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsDispatchedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientDispatchedDateTime, DispatchedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsDispatchedTimeSub2MinUpdated)
            {
                var extended = DispatchedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientDispatchedSub2MinTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsClientReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientReceivedDateTime
                                             , ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsClientReceivedTimeSub2MinUpdated)
            {
                var extended = ClientReceivedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientReceivedSub2MinTime, lower4Bytes, extended);
            }
        }

        foreach (var quoteContainerUpdates in PQQuoteContainer.GetDeltaUpdateFields
                     (snapShotTime, messageFlags, quotePublicationPrecisionSettings ?? PQSourceTickerInfo))
        {
            yield return quoteContainerUpdates;
        }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        var infoResult = PQSourceTickerInfo!.UpdateField(pqFieldUpdate);
        if (infoResult >= 0) return infoResult;
        infoResult = PQQuoteContainer!.UpdateField(pqFieldUpdate);
        if (infoResult >= 0) return infoResult;
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.PQSyncStatus:
                IsFeedSyncStatusUpdated = true; // incase of reset and sending 0;
                FeedSyncStatus          = (FeedSyncStatus)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.ClientSocketReceivingDateTime:
                IsSocketReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref socketReceivingTime, pqFieldUpdate.Payload);
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
                return 0;
            case PQFeedFields.ClientSocketReceivingSub2MinTime:
                IsSocketReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref socketReceivingTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
                return 0;
            case PQFeedFields.ClientProcessedDateTime:
                IsProcessedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref processedTime, pqFieldUpdate.Payload);
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
                return 0;
            case PQFeedFields.ClientProcessedSub2MinTime:
                IsProcessedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref processedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
                return 0;
            case PQFeedFields.ClientDispatchedDateTime:
                IsDispatchedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref dispatchedTime, pqFieldUpdate.Payload);
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
                return 0;
            case PQFeedFields.ClientDispatchedSub2MinTime:
                IsDispatchedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref dispatchedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
                return 0;
            case PQFeedFields.ClientReceivedDateTime:
                IsClientReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref clientReceivedTime, pqFieldUpdate.Payload);
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
                return 0;
            case PQFeedFields.ClientReceivedSub2MinTime:
                IsClientReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref clientReceivedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
                return 0;
        }

        return -1;
    }


    public virtual void EnsureRelatedItemsAreConfigured(ITickInstant? referenceInstance)
    {
        if (referenceInstance is IPublishableTickInstant { SourceTickerInfo: IPQSourceTickerInfo pqSrcTkrQuoteInfo }) 
            SourceTickerInfo = pqSrcTkrQuoteInfo;
        PQQuoteContainer.EnsureRelatedItemsAreConfigured(referenceInstance);
    }

    public virtual void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? srcTickerInfo)
    {
        if (srcTickerInfo == null || ReferenceEquals(SourceTickerInfo, srcTickerInfo)) return;
        if (srcTickerInfo.AreEquivalent(SourceTickerInfo)) return;

        SourceTickerInfo = new PQSourceTickerInfo(srcTickerInfo);
        PQQuoteContainer.EnsureRelatedItemsAreConfigured(srcTickerInfo);
    }

    public bool AreEquivalent(ITickInstant? other, bool exactTypes = false) => AreEquivalent(other as IPublishableTickInstant, exactTypes);

    public virtual bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        
        var quoteValuesSame = PQQuoteContainer.AreEquivalent(
                                                             (ITickInstant?)((other as PQPublishableTickInstant)?.PQQuoteContainer) ?? other, exactTypes);
        var tickerInfoSame =
            PQSourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes)
         ?? other.SourceTickerInfo == null;

        var sequenceIdSame        = true;
        var publicationStatusSame = true;

        var socketReceivingTimeSame = true;
        var lastPubTimeSame         = true;
        var processingTimeSame      = true;
        var dispatchTimeSame        = true;
        var clientReceivedSame      = true;
        var updatedFlagsSame        = true;
        var booleanFieldsSame       = true;
        var pqTickInstant           = other as PQPublishableTickInstant;
        if (exactTypes)
        {
            sequenceIdSame        = PQSequenceId == pqTickInstant!.PQSequenceId;
            publicationStatusSame = FeedSyncStatus == pqTickInstant.FeedSyncStatus;

            socketReceivingTimeSame = SocketReceivingTime.Equals(pqTickInstant.SocketReceivingTime);
            lastPubTimeSame         = LastPublicationTime.Equals(pqTickInstant.LastPublicationTime);
            processingTimeSame      = ProcessedTime.Equals(pqTickInstant.ProcessedTime);
            dispatchTimeSame        = DispatchedTime.Equals(pqTickInstant.DispatchedTime);
            clientReceivedSame      = ClientReceivedTime == other.ClientReceivedTime;

            updatedFlagsSame  = UpdatedFlags == pqTickInstant.UpdatedFlags;
        }

        var allAreSame = clientReceivedSame && quoteValuesSame && tickerInfoSame && updatedFlagsSame && booleanFieldsSame 
                      && dispatchTimeSame && processingTimeSame && lastPubTimeSame && socketReceivingTimeSame 
                      && sequenceIdSame && publicationStatusSame;
        return allAreSame;
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<IPublishableTickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
        return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (PQSourceTickerInfo != null)
            foreach (var field in PQSourceTickerInfo.GetStringUpdates(snapShotTime, messageFlags))
                yield return field;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate) =>
        PQSourceTickerInfo != null && PQSourceTickerInfo.UpdateFieldString(stringUpdate);

    IReusableObject<ITickInstant> ITransferState<IReusableObject<ITickInstant>>.CopyFrom
        (IReusableObject<ITickInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    ITickInstant ITransferState<ITickInstant>.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    IPQTickInstant IPQTickInstant.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    IPQPublishableTickInstant IPQPublishableTickInstant.CopyFrom
        (IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    public override PQPublishableTickInstant CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQPublishableTickInstant ipq0)
        {
            if (PQSourceTickerInfo != null)
                PQSourceTickerInfo.CopyFrom(ipq0.SourceTickerInfo!, copyMergeFlags);
            else
                SourceTickerInfo = ipq0.SourceTickerInfo;
            // only copy if changed
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (ipq0.IsSocketReceivedTimeDateUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = socketReceivingTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref socketReceivingTime
                                                                    , ipq0.SocketReceivingTime.Get2MinIntervalsFromUnixEpoch());
                IsSocketReceivedTimeDateUpdated = originalSocketReceivingTime != socketReceivingTime;
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
            }
            if (ipq0.IsSocketReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = socketReceivingTime;
                PQFieldConverters.UpdateSub2MinComponent(ref socketReceivingTime, ipq0.SocketReceivingTime.GetSub2MinComponent());
                IsSocketReceivedTimeSub2MinUpdated = originalSocketReceivingTime != socketReceivingTime;
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
            }
            if (ipq0.IsProcessedTimeDateUpdated || isFullReplace)
            {
                var originalProcessedTime = processedTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref processedTime, ipq0.ProcessedTime.Get2MinIntervalsFromUnixEpoch());
                IsProcessedTimeDateUpdated = originalProcessedTime != processedTime;
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
            }
            if (ipq0.IsProcessedTimeSub2MinUpdated || isFullReplace)
            {
                var originalProcessedTime = processedTime;
                PQFieldConverters.UpdateSub2MinComponent(ref processedTime, ipq0.ProcessedTime.GetSub2MinComponent());
                IsProcessedTimeSub2MinUpdated = originalProcessedTime != processedTime;
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
            }
            if (ipq0.IsDispatchedTimeDateUpdated || isFullReplace)
            {
                var originalDispatchedTime = dispatchedTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref dispatchedTime, ipq0.DispatchedTime.Get2MinIntervalsFromUnixEpoch());
                IsDispatchedTimeDateUpdated = originalDispatchedTime != dispatchedTime;
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
            }
            if (ipq0.IsDispatchedTimeSub2MinUpdated || isFullReplace)
            {
                var originalDispatchedTime = dispatchedTime;
                PQFieldConverters.UpdateSub2MinComponent(ref dispatchedTime, ipq0.DispatchedTime.GetSub2MinComponent());
                IsDispatchedTimeSub2MinUpdated = originalDispatchedTime != dispatchedTime;
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
            }
            if (ipq0.IsClientReceivedTimeDateUpdated || isFullReplace)
            {
                var originalClientReceivedTime = clientReceivedTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref clientReceivedTime
                                                                    , ipq0.ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());
                IsClientReceivedTimeDateUpdated = originalClientReceivedTime != clientReceivedTime;
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
            }
            if (ipq0.IsClientReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalClientReceivedTime = clientReceivedTime;
                PQFieldConverters.UpdateSub2MinComponent(ref clientReceivedTime, ipq0.ClientReceivedTime.GetSub2MinComponent());
                IsClientReceivedTimeSub2MinUpdated = originalClientReceivedTime != clientReceivedTime;
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
            }

            if (ipq0.IsReplayUpdated || isFullReplace) IsReplay               = ipq0.IsReplay;
            if (ipq0.IsSingleValueUpdated || isFullReplace) SingleTickValue   = ipq0.SingleTickValue;
            if (ipq0.IsFeedSyncStatusUpdated || isFullReplace) FeedSyncStatus = ipq0.FeedSyncStatus;
            //PQ tracks its own changes only copy explicit changes

            OverrideSerializationFlags = ipq0.OverrideSerializationFlags;

            PQSequenceId = ipq0.PQSequenceId;

            if (source is PQPublishableTickInstant pq0)
            {
                PQQuoteContainer.CopyFrom(pq0.PQQuoteContainer, copyMergeFlags);
                LastPublicationTime = pq0.LastPublicationTime;
                if (isFullReplace) UpdatedFlags = pq0.UpdatedFlags;
            }
            else
            {
                PQQuoteContainer.CopyFrom(ipq0, copyMergeFlags);
            }
        }
        else
        {
            PQQuoteContainer.CopyFrom(source, copyMergeFlags);
            OverrideSerializationFlags = null;

            ClientReceivedTime = source.ClientReceivedTime;
            if (source.SourceTickerInfo != null)
            {
                SourceTickerInfo ??= new PQSourceTickerInfo(source.SourceTickerInfo);
                SourceTickerInfo.CopyFrom(source.SourceTickerInfo);
            }
            else
            {
                SourceTickerInfo?.StateReset();
            }
        }

        return this;
    }


    public virtual PQPublishableTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        ((IMutablePublishableTickInstant)this).SourceTickerInfo = toSet;
        return this;
    }

    private IPQSourceTickerInfo ConvertToPQSourceTickerInfo
        (ISourceTickerInfo value, IPQSourceTickerInfo? originalQuoteInfo)
    {
        if (originalQuoteInfo == null)
        {
            originalQuoteInfo = new PQSourceTickerInfo(value);
            return originalQuoteInfo;
        }

        originalQuoteInfo.CopyFrom(value);
        return originalQuoteInfo;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((PQPublishableTickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)UpdatedFlags;
            hashCode = (hashCode * 397) ^ (int)PQSequenceId;
            hashCode = (hashCode * 397) ^ SourceTickerInfo?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ PQQuoteContainer.GetHashCode();
            hashCode = (hashCode * 397) ^ LastPublicationTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SocketReceivingTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ProcessedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ DispatchedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ IsReplay.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)FeedSyncStatus;
            hashCode = (hashCode * 397) ^ (SourceTickerInfo?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {PQQuoteContainer.QuoteToStringMembers}, {UpdatedFlagsToString})";

    protected void SetFlagsSame(IPublishableTickInstant toCopyFlags)
    {
        if (toCopyFlags is PQPublishableTickInstant pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }
}
