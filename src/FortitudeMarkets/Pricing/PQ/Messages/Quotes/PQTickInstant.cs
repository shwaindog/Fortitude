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
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.TimeSeries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public interface IPQTickInstant : IDoublyLinkedListNode<IPQTickInstant>, IMutableTickInstant,
    IPQSupportsFieldUpdates<ITickInstant>, IPQSupportsStringUpdates<ITickInstant>, IRelatedItems<ITickInstant>,
    IVersionedMessage
{
    PQMessageFlags? OverrideSerializationFlags { get; set; }

    bool IsSourceTimeDateUpdated            { get; set; }
    bool IsSourceTimeSub2MinUpdated         { get; set; }
    bool IsSocketReceivedTimeDateUpdated    { get; set; }
    bool IsSocketReceivedTimeSub2MinUpdated { get; set; }
    bool IsProcessedTimeDateUpdated         { get; set; }
    bool IsProcessedTimeSub2MinUpdated      { get; set; }
    bool IsDispatchedTimeDateUpdated        { get; set; }
    bool IsDispatchedTimeSub2MinUpdated     { get; set; }
    bool IsClientReceivedTimeDateUpdated    { get; set; }
    bool IsClientReceivedTimeSub2MinUpdated { get; set; }
    bool IsFeedSyncStatusUpdated            { get; set; }
    bool IsReplayUpdated                    { get; set; }
    bool IsSingleValueUpdated               { get; set; }

    ISyncLock Lock                { get; }
    uint      PQSequenceId        { get; set; }
    DateTime  LastPublicationTime { get; set; }
    DateTime  SocketReceivingTime { get; set; }
    DateTime  ProcessedTime       { get; set; }
    DateTime  DispatchedTime      { get; set; }

    new FeedSyncStatus  FeedSyncStatus { get; set; }
    new IPQTickInstant? Next           { get; set; }
    new IPQTickInstant? Previous       { get; set; }

    void ResetFields();

    new IPQTickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);

    new IPQTickInstant Clone();
}

public class PQTickInstant : ReusableObject<ITickInstant>, IPQTickInstant, ICloneable<PQTickInstant>, IDoublyLinkedListNode<PQTickInstant>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQTickInstant));

    protected readonly ISyncLock SyncLock = new SpinLockLight();

    protected PQBooleanValues BooleanFields;

    private DateTime clientReceivedTime;
    private DateTime dispatchedTime;

    private   FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good;
    protected uint           NumOfUpdates   = uint.MaxValue;

    protected PQSourceTickerInfo? PQSourceTickerInfo;

    private DateTime processedTime;
    private decimal  singleValue;
    private DateTime socketReceivingTime;
    private DateTime sourceTime;

    protected QuoteFieldUpdatedFlags UpdatedFlags;

    public PQTickInstant()
    {
        if (GetType() == typeof(PQTickInstant)) NumOfUpdates = 0;
    }
    
    // Reflection invoked constructor (PQServer<T>)
    public PQTickInstant(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m)
    {
    }

    public PQTickInstant(ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false, 
        FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, decimal singlePrice = 0m, DateTime? clientReceivedTime = null)
    {
        SourceTickerInfo = sourceTickerInfo;

        SourceTime              = sourceTime ?? DateTime.MinValue;
        IsReplay                = isReplay;
        FeedSyncStatus          = feedSyncStatus;
        SingleTickValue         = singlePrice;
        ClientReceivedTime      = clientReceivedTime ?? DateTime.MinValue;
        
        if (GetType() == typeof(PQTickInstant)) NumOfUpdates = 0;
    }

    public PQTickInstant(ITickInstant toClone)
    {
        SingleTickValue         = toClone.SingleTickValue;
        IsReplay                = toClone.IsReplay;
        SourceTime              = toClone.SourceTime;
        ClientReceivedTime      = toClone.ClientReceivedTime;
        FeedSyncStatus          = toClone.FeedSyncStatus;

        SourceTickerInfo = new PQSourceTickerInfo(toClone.SourceTickerInfo!);
        if (toClone is IPQTickInstant ipqTickInstant)
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
        if (GetType() == typeof(PQTickInstant)) NumOfUpdates = 0;
    }

    protected string TickInstantToStringMembers =>
        $"{nameof(PQSourceTickerInfo)}: {PQSourceTickerInfo}, {nameof(PQSequenceId)}: {PQSequenceId}, " +
        $"{nameof(FeedSyncStatus)}: {FeedSyncStatus}, {nameof(LastPublicationTime)}: {LastPublicationTime}, " +
        $"{nameof(SourceTime)}: {SourceTime}, {nameof(DispatchedTime)}: {DispatchedTime}, " +
        $"{nameof(ProcessedTime)}: {ProcessedTime}, {nameof(IsSourceTimeDateUpdated)}: {IsSourceTimeDateUpdated}, " +
        $"{nameof(IsSourceTimeSub2MinUpdated)}: {IsSourceTimeSub2MinUpdated}, " +
        $"{nameof(IsFeedSyncStatusUpdated)}: {IsFeedSyncStatusUpdated}, {nameof(SingleTickValue)}: {SingleTickValue}, " +
        $"{nameof(IsSingleValueUpdated)}: {IsSingleValueUpdated}, {nameof(IsReplay)}: {IsReplay}, " +
        $"{nameof(IsReplayUpdated)}: {IsReplayUpdated}, {nameof(HasUpdates)}: {HasUpdates}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override PQTickInstant Clone() =>
        Recycler?.Borrow<PQTickInstant>().CopyFrom(this, CopyMergeFlags.FullReplace) as PQTickInstant ?? new PQTickInstant(this);

    [JsonIgnore]
    public PQTickInstant? Previous
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Previous as PQTickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public PQTickInstant? Next
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Next as PQTickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Next = value;
    }

    IVersionedMessage ITransferState<IVersionedMessage>.CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ITickInstant)source, copyMergeFlags);

    [JsonIgnore]
    IPQTickInstant? IPQTickInstant.Previous
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Previous as IPQTickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQTickInstant? IPQTickInstant.Next
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Next as IPQTickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQTickInstant? IDoublyLinkedListNode<IPQTickInstant>.Previous
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Previous as IPQTickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQTickInstant? IDoublyLinkedListNode<IPQTickInstant>.Next
    {
        get => ((IDoublyLinkedListNode<ITickInstant>)this).Next as IPQTickInstant;
        set => ((IDoublyLinkedListNode<ITickInstant>)this).Next = value;
    }

    [JsonIgnore] public virtual TickerDetailLevel TickerDetailLevel => TickerDetailLevel.SingleValue;

    [JsonIgnore] public PQMessageFlags? OverrideSerializationFlags { get; set; }

    [JsonIgnore] public uint MessageId => (uint)PQMessageIds.Quote;

    public uint PQSequenceId { get; set; }

    [JsonIgnore] public ISyncLock Lock => SyncLock;

    public DateTime LastPublicationTime { get; set; }

    [JsonIgnore] public byte Version => 1;

    [JsonIgnore] ISourceTickerInfo? ITickInstant.SourceTickerInfo => PQSourceTickerInfo;

    public ISourceTickerInfo? SourceTickerInfo
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

    [JsonIgnore] ITickInstant? IDoublyLinkedListNode<ITickInstant>.Previous { get; set; }
    [JsonIgnore] ITickInstant? IDoublyLinkedListNode<ITickInstant>.Next     { get; set; }

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime DispatchedTime
    {
        get => dispatchedTime;
        set
        {
            if (dispatchedTime == value) return;
            IsDispatchedTimeDateUpdated    |= dispatchedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
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
            IsProcessedTimeDateUpdated    |= processedTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsProcessedTimeSub2MinUpdated |= processedTime.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            processedTime                 =  value == DateTime.UnixEpoch ? default : value;
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
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentSubHourUpdatedFlag;

            else if (IsSourceTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag;

            else if (IsSocketReceivedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SocketReceivedSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SocketReceivedSubHourUpdatedFlag;

            else if (IsSocketReceivedTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SocketReceivedSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag;

            else if (IsProcessedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ProcessedSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ProcessedSubHourUpdatedFlag;

            else if (IsProcessedTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ProcessedSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag;

            else if (IsDispatchedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.DispatchedSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.DispatchedSubHourUpdatedFlag;

            else if (IsDispatchedTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.DispatchedSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag;

            else if (IsClientReceivedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeSub2MinUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ClientReceivedSubHourUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ClientReceivedSubHourUpdatedFlag;

            else if (IsClientReceivedTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ClientReceivedSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsFeedSyncStatusUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.FeedSyncStatusFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.FeedSyncStatusFlag;

            else if (IsFeedSyncStatusUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.FeedSyncStatusFlag;
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
        get => singleValue;
        set
        {
            IsSingleValueUpdated = singleValue != value || NumOfUpdates == 0;
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
            IsReplayUpdated = IsReplay != value || NumOfUpdates == 0;
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

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != QuoteFieldUpdatedFlags.None;
        set
        {
            if (PQSourceTickerInfo != null) PQSourceTickerInfo.HasUpdates = value;
            UpdatedFlags  =  value ? UpdatedFlags.AllFlags() : QuoteFieldUpdatedFlags.None;
            if (!value)
            {
                BooleanFields &= PQBooleanValues.BooleanValuesMask;
            }
        }
    }

    public uint UpdateCount => NumOfUpdates;

    public virtual void UpdateComplete()
    {
        PQSourceTickerInfo?.UpdateComplete();
        if (HasUpdates) NumOfUpdates++;
        HasUpdates = false;
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        SourceTime          += toChangeBy;
        ClientReceivedTime  += toChangeBy;
        DispatchedTime      += toChangeBy;
        ProcessedTime       += toChangeBy;
        SocketReceivingTime += toChangeBy;
        ClientReceivedTime  += toChangeBy;
    }

    public virtual void ResetFields()
    {
        OverrideSerializationFlags = null;

        NumOfUpdates = 0;

        clientReceivedTime  = default;
        LastPublicationTime = default;
        socketReceivingTime = default;

        PQSequenceId   = 0;
        singleValue    = 0;
        sourceTime     = default;
        processedTime  = default;
        dispatchedTime = default;
        FeedSyncStatus = FeedSyncStatus.Good;
        IsReplay       = false;
        BooleanFields  = PQBooleanValues.DefaultEmptyQuoteFlags;
        UpdatedFlags   = QuoteFieldUpdatedFlags.None;
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

        if (!updatedOnly || IsSingleValueUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.SingleTickValue, SingleTickValue,
                                           PQSourceTickerInfo?.PriceScalingPrecision ?? (PQFieldFlags)1);
        if (!updatedOnly || IsSourceTimeDateUpdated)
            yield return new PQFieldUpdate(PQQuoteFields.SourceSentDateTime, sourceTime.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsSourceTimeSub2MinUpdated)
        {
            var extended = sourceTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
            yield return new PQFieldUpdate(PQQuoteFields.SourceSentSubHourTime, lower4Bytes, extended);
        }
        if (!updatedOnly || IsBooleanFlagsChanged())
        {
            var booleanFields = GenerateBooleanFlags(!updatedOnly);
            yield return new PQFieldUpdate(PQQuoteFields.QuoteBooleanFlags, (uint)booleanFields);
        }

        if (!updatedOnly || IsFeedSyncStatusUpdated) yield return new PQFieldUpdate(PQQuoteFields.PQSyncStatus, (byte)FeedSyncStatus);

        var includeReceiverTimes = (messageFlags & StorageFlags.IncludeReceiverTimes) > 0;
        if (includeReceiverTimes)
        {
            if (!updatedOnly || IsSocketReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQQuoteFields.SocketReceivingDateTime
                                             , SocketReceivingTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsSocketReceivedTimeSub2MinUpdated)
            {
                var extended = SocketReceivingTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQQuoteFields.SocketReceivingSubHourTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsProcessedTimeDateUpdated)
                yield return new PQFieldUpdate(PQQuoteFields.ProcessedDateTime, ProcessedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsProcessedTimeSub2MinUpdated)
            {
                var extended = ProcessedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQQuoteFields.ProcessedSubHourTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsDispatchedTimeDateUpdated)
                yield return new PQFieldUpdate(PQQuoteFields.DispatchedDateTime, DispatchedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsDispatchedTimeSub2MinUpdated)
            {
                var extended = DispatchedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQQuoteFields.DispatchedSubHourTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsClientReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQQuoteFields.ClientReceivedDateTime
                                             , ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsClientReceivedTimeSub2MinUpdated)
            {
                var extended = ClientReceivedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQQuoteFields.ClientReceivedSubHourTime, lower4Bytes, extended);
            }
        }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        var infoResult = PQSourceTickerInfo!.UpdateField(pqFieldUpdate);
        if (infoResult > 0) return infoResult;
        switch (pqFieldUpdate.Id)
        {
            case PQQuoteFields.SingleTickValue:
                IsSingleValueUpdated = true; // incase of reset and sending 0;
                SingleTickValue      = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                return 0;
            case PQQuoteFields.SourceSentDateTime:
                IsSourceTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceTime, pqFieldUpdate.Payload);
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQQuoteFields.SourceSentSubHourTime:
                IsSourceTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref sourceTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQQuoteFields.QuoteBooleanFlags:
                SetBooleanFields((PQBooleanValues)pqFieldUpdate.Payload);
                return 0;
            case PQQuoteFields.PQSyncStatus:
                IsFeedSyncStatusUpdated = true; // incase of reset and sending 0;
                FeedSyncStatus          = (FeedSyncStatus)pqFieldUpdate.Payload;
                return 0;
            case PQQuoteFields.SocketReceivingDateTime:
                IsSocketReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref socketReceivingTime, pqFieldUpdate.Payload);
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
                return 0;
            case PQQuoteFields.SocketReceivingSubHourTime:
                IsSocketReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref socketReceivingTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
                return 0;
            case PQQuoteFields.ProcessedDateTime:
                IsProcessedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref processedTime, pqFieldUpdate.Payload);
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
                return 0;
            case PQQuoteFields.ProcessedSubHourTime:
                IsProcessedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref processedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
                return 0;
            case PQQuoteFields.DispatchedDateTime:
                IsDispatchedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref dispatchedTime, pqFieldUpdate.Payload);
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
                return 0;
            case PQQuoteFields.DispatchedSubHourTime:
                IsDispatchedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref dispatchedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
                return 0;
            case PQQuoteFields.ClientReceivedDateTime:
                IsClientReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref clientReceivedTime, pqFieldUpdate.Payload);
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
                return 0;
            case PQQuoteFields.ClientReceivedSubHourTime:
                IsClientReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref clientReceivedTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
                return 0;
        }

        return -1;
    }


    IPQTickInstant IPQTickInstant.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ITickInstant)source, copyMergeFlags);

    public virtual void EnsureRelatedItemsAreConfigured(ITickInstant? referenceInstance)
    {
        if (referenceInstance?.SourceTickerInfo is IPQSourceTickerInfo pqSrcTkrQuoteInfo) SourceTickerInfo = pqSrcTkrQuoteInfo;
    }

    ITickInstant ICloneable<ITickInstant>.          Clone() => Clone();
    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    IPQTickInstant IPQTickInstant.Clone() => Clone();

    public virtual bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var isReplaySame = IsReplay == other.IsReplay;
        var tickerInfoSame =
            PQSourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes)
         ?? other.SourceTickerInfo == null;
        var singlePriceSame = singleValue == other.SingleTickValue;
        var sourceTimeSame  = SourceTime.Equals(other.SourceTime);

        var sequenceIdSame        = true;
        var publicationStatusSame = true;

        var socketReceivingTimeSame = true;
        var lastPubTimeSame         = true;
        var processingTimeSame      = true;
        var dispatchTimeSame        = true;
        var clientReceivedSame      = true;
        var updatedFlagsSame        = true;
        var booleanFieldsSame       = true;
        var pqTickInstant           = other as PQTickInstant;
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
            booleanFieldsSame = BooleanFields == pqTickInstant.BooleanFields;
        }

        var allAreSame = clientReceivedSame && isReplaySame && tickerInfoSame && singlePriceSame && sourceTimeSame
                      && updatedFlagsSame && booleanFieldsSame && dispatchTimeSame && processingTimeSame && lastPubTimeSame
                      && socketReceivingTimeSame && sequenceIdSame && publicationStatusSame;
        return allAreSame;
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ITickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
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


    public override PQTickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQTickInstant ipq0)
        {
            if (PQSourceTickerInfo != null)
                PQSourceTickerInfo.CopyFrom(ipq0.SourceTickerInfo!, copyMergeFlags);
            else
                SourceTickerInfo = ipq0.SourceTickerInfo;
            // only copy if changed
            var isFullReplace = copyMergeFlags.HasFullReplace();
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
            if (ipq0.IsSocketReceivedTimeDateUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = socketReceivingTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref socketReceivingTime, ipq0.SocketReceivingTime.Get2MinIntervalsFromUnixEpoch());
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
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref clientReceivedTime, ipq0.ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());
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

            if (source is PQTickInstant pq0)
            {
                LastPublicationTime = pq0.LastPublicationTime;
                if (isFullReplace) UpdatedFlags = pq0.UpdatedFlags;
            }
        }
        else
        {
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
            SourceTime      = source.SourceTime;
            IsReplay        = source.IsReplay;
            SingleTickValue = source.SingleTickValue;
            FeedSyncStatus  = source.FeedSyncStatus;
        }

        return this;
    }


    public virtual PQTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        SourceTickerInfo = toSet;
        return this;
    }

    private PQSourceTickerInfo ConvertToPQSourceTickerInfo
        (ISourceTickerInfo value, PQSourceTickerInfo? originalQuoteInfo)
    {
        if (originalQuoteInfo == null)
        {
            originalQuoteInfo = new PQSourceTickerInfo(value);
            return originalQuoteInfo;
        }

        originalQuoteInfo.CopyFrom(value);
        return originalQuoteInfo;
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

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((PQTickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)UpdatedFlags;
            hashCode = (hashCode * 397) ^ (int)PQSequenceId;
            hashCode = (hashCode * 397) ^ SourceTickerInfo?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ BooleanFields.GetHashCode();
            hashCode = (hashCode * 397) ^ singleValue.GetHashCode();
            hashCode = (hashCode * 397) ^ sourceTime.GetHashCode();
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

    public override string ToString() => $"{GetType().Name}({TickInstantToStringMembers}, {UpdatedFlagsToString})";

    protected void SetFlagsSame(ITickInstant toCopyFlags)
    {
        if (toCopyFlags is PQTickInstant pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }
}
