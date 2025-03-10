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
    IPQSupportsFieldUpdates<ITickInstant>, IPQSupportsStringUpdates<ITickInstant>, IRelatedItem<ITickInstant>,
    IVersionedMessage
{
    PQMessageFlags? OverrideSerializationFlags { get; set; }

    bool IsSourceTimeDateUpdated            { get; set; }
    bool IsSourceTimeSubHourUpdated         { get; set; }
    bool IsSocketReceivedTimeDateUpdated    { get; set; }
    bool IsSocketReceivedTimeSubHourUpdated { get; set; }
    bool IsProcessedTimeDateUpdated         { get; set; }
    bool IsProcessedTimeSubHourUpdated      { get; set; }
    bool IsDispatchedTimeDateUpdated        { get; set; }
    bool IsDispatchedTimeSubHourUpdated     { get; set; }
    bool IsClientReceivedTimeDateUpdated    { get; set; }
    bool IsClientReceivedTimeSubHourUpdated { get; set; }
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

    new IPQTickInstant Clone();
}

public class PQTickInstant : ReusableObject<ITickInstant>, IPQTickInstant, ICloneable<PQTickInstant>, IDoublyLinkedListNode<PQTickInstant>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQTickInstant));

    protected readonly ISyncLock SyncLock = new SpinLockLight();

    protected PQBooleanValues BooleanFields;

    private DateTime clientReceivedTime;
    private DateTime dispatchedTime;

    private FeedSyncStatus feedSyncStatus = FeedSyncStatus.OutOfSync;

    protected PQSourceTickerInfo? PQSourceTickerInfo;

    private DateTime processedTime;
    private decimal  singleValue;
    private DateTime socketReceivingTime;
    private DateTime sourceTime;

    protected QuoteFieldUpdatedFlags UpdatedFlags;

    public PQTickInstant() { }

    public PQTickInstant(ISourceTickerInfo sourceTickerInfo) => SourceTickerInfo = sourceTickerInfo;

    public PQTickInstant(ITickInstant toClone)
    {
        singleValue        = toClone.SingleTickValue;
        IsReplay           = toClone.IsReplay;
        sourceTime         = toClone.SourceTime;
        ClientReceivedTime = toClone.ClientReceivedTime;
        FeedSyncStatus     = toClone.FeedSyncStatus;

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
        if (toClone is PQTickInstant pqTickInstant) UpdatedFlags = pqTickInstant.UpdatedFlags;
    }

    protected string TickInstantToStringMembers =>
        $"{nameof(PQSourceTickerInfo)}: {PQSourceTickerInfo}, {nameof(PQSequenceId)}: {PQSequenceId}, " +
        $"{nameof(FeedSyncStatus)}: {FeedSyncStatus}, {nameof(LastPublicationTime)}: {LastPublicationTime}, " +
        $"{nameof(SourceTime)}: {SourceTime}, {nameof(DispatchedTime)}: {DispatchedTime}, " +
        $"{nameof(ProcessedTime)}: {ProcessedTime}, {nameof(IsSourceTimeDateUpdated)}: {IsSourceTimeDateUpdated}, " +
        $"{nameof(IsSourceTimeSubHourUpdated)}: {IsSourceTimeSubHourUpdated}, " +
        $"{nameof(IsFeedSyncStatusUpdated)}: {IsFeedSyncStatusUpdated}, {nameof(SingleTickValue)}: {SingleTickValue}, " +
        $"{nameof(IsSingleValueUpdated)}: {IsSingleValueUpdated}, {nameof(IsReplay)}: {IsReplay}, " +
        $"{nameof(IsReplayUpdated)}: {IsReplayUpdated}, {nameof(HasUpdates)}: {HasUpdates}";

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
            IsSourceTimeDateUpdated    |= sourceTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsSourceTimeSubHourUpdated |= sourceTime.GetSubHourComponent() != value.GetSubHourComponent();
            sourceTime                 =  value == DateTime.UnixEpoch ? default : value;
            ;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime DispatchedTime
    {
        get => dispatchedTime;
        set
        {
            if (dispatchedTime == value) return;
            IsDispatchedTimeDateUpdated    |= dispatchedTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsDispatchedTimeSubHourUpdated |= dispatchedTime.GetSubHourComponent() != value.GetSubHourComponent();
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
            IsProcessedTimeDateUpdated    |= processedTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsProcessedTimeSubHourUpdated |= processedTime.GetSubHourComponent() != value.GetSubHourComponent();
            processedTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SocketReceivingTime
    {
        get => socketReceivingTime;
        set
        {
            if (socketReceivingTime == value) return;
            IsSocketReceivedTimeDateUpdated    |= socketReceivingTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsSocketReceivedTimeSubHourUpdated |= socketReceivingTime.GetSubHourComponent() != value.GetSubHourComponent();
            socketReceivingTime                =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ClientReceivedTime
    {
        get => clientReceivedTime;
        set
        {
            if (clientReceivedTime == value) return;
            IsClientReceivedTimeDateUpdated    |= clientReceivedTime.GetHoursFromUnixEpoch() != value.GetHoursFromUnixEpoch();
            IsClientReceivedTimeSubHourUpdated |= clientReceivedTime.GetSubHourComponent() != value.GetSubHourComponent();
            clientReceivedTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore]
    public bool IsSourceTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag) > 0 && SourceTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag;

            else if (IsSourceTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentSubHourUpdatedFlag) > 0 && SourceTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentSubHourUpdatedFlag;

            else if (IsSourceTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag) > 0 && SocketReceivingTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag;

            else if (IsSocketReceivedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SocketReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SocketReceivedSubHourUpdatedFlag) > 0 && SocketReceivingTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SocketReceivedSubHourUpdatedFlag;

            else if (IsSocketReceivedTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SocketReceivedSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag) > 0 && ProcessedTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag;

            else if (IsProcessedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ProcessedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ProcessedSubHourUpdatedFlag) > 0 && ProcessedTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ProcessedSubHourUpdatedFlag;

            else if (IsProcessedTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ProcessedSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag) > 0 && DispatchedTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag;

            else if (IsDispatchedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.DispatchedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.DispatchedSubHourUpdatedFlag) > 0 && DispatchedTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.DispatchedSubHourUpdatedFlag;

            else if (IsDispatchedTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.DispatchedSubHourUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag) > 0 && ClientReceivedTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag;

            else if (IsClientReceivedTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ClientReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeSubHourUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ClientReceivedSubHourUpdatedFlag) > 0 && ClientReceivedTime != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ClientReceivedSubHourUpdatedFlag;

            else if (IsClientReceivedTimeSubHourUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ClientReceivedSubHourUpdatedFlag;
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
            if (singleValue == value) return;
            IsSingleValueUpdated = true;
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
                UpdatedFlags ^= QuoteFieldUpdatedFlags.SingleValueUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsReplay
    {
        get => (BooleanFields & PQBooleanValues.IsReplaySetFlag) > 0;
        set
        {
            if (IsReplay == value) return;
            IsReplayUpdated = true;
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
        get => UpdatedFlags > 0;
        set
        {
            if (PQSourceTickerInfo != null) PQSourceTickerInfo.HasUpdates = value;
            UpdatedFlags = value ? UpdatedFlags.AllFlags() : QuoteFieldUpdatedFlags.None;
        }
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

        PQSequenceId        = 0;
        singleValue         = 0;
        sourceTime          = default;
        socketReceivingTime = default;
        processedTime       = default;
        dispatchedTime      = default;
        clientReceivedTime  = default;
        LastPublicationTime = default;
        FeedSyncStatus      = FeedSyncStatus.OutOfSync;
        IsReplay            = false;
        UpdatedFlags        = QuoteFieldUpdatedFlags.None;
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
            yield return new PQFieldUpdate(PQFieldKeys.SingleTickValue, SingleTickValue,
                                           PQSourceTickerInfo?.PriceScalingPrecision ?? 1);
        if (!updatedOnly || IsSourceTimeDateUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.SourceSentDateTime, sourceTime.GetHoursFromUnixEpoch());
        if (!updatedOnly || IsSourceTimeSubHourUpdated)
        {
            var fifthByte = sourceTime.GetSubHourComponent().BreakLongToByteAndUint(out var lower4Bytes);
            yield return new PQFieldUpdate(PQFieldKeys.SourceSentSubHourTime, lower4Bytes, fifthByte);
        }
        if (!updatedOnly || IsBooleanFlagsChanged())
        {
            var booleanFields = GenerateBooleanFlags(!updatedOnly);
            yield return new PQFieldUpdate(PQFieldKeys.QuoteBooleanFlags, (uint)booleanFields);
        }

        if (!updatedOnly || IsFeedSyncStatusUpdated) yield return new PQFieldUpdate(PQFieldKeys.PQSyncStatus, (byte)FeedSyncStatus);

        var includeReceiverTimes = (messageFlags & StorageFlags.IncludeReceiverTimes) > 0;
        if (includeReceiverTimes)
        {
            if (!updatedOnly || IsSocketReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFieldKeys.SocketReceivingDateTime
                                             , SocketReceivingTime.GetHoursFromUnixEpoch());

            if (!updatedOnly || IsSocketReceivedTimeSubHourUpdated)
            {
                var fifthByte = SocketReceivingTime.GetSubHourComponent().BreakLongToByteAndUint(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFieldKeys.SocketReceivingSubHourTime, lower4Bytes, fifthByte);
            }

            if (!updatedOnly || IsProcessedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFieldKeys.ProcessedDateTime, ProcessedTime.GetHoursFromUnixEpoch());

            if (!updatedOnly || IsProcessedTimeSubHourUpdated)
            {
                var fifthByte = ProcessedTime.GetSubHourComponent().BreakLongToByteAndUint(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFieldKeys.ProcessedSubHourTime, lower4Bytes, fifthByte);
            }

            if (!updatedOnly || IsDispatchedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFieldKeys.DispatchedDateTime, DispatchedTime.GetHoursFromUnixEpoch());

            if (!updatedOnly || IsDispatchedTimeSubHourUpdated)
            {
                var fifthByte = DispatchedTime.GetSubHourComponent().BreakLongToByteAndUint(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFieldKeys.DispatchedSubHourTime, lower4Bytes, fifthByte);
            }

            if (!updatedOnly || IsClientReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFieldKeys.ClientReceivedDateTime
                                             , ClientReceivedTime.GetHoursFromUnixEpoch());

            if (!updatedOnly || IsClientReceivedTimeSubHourUpdated)
            {
                var fifthByte = ClientReceivedTime.GetSubHourComponent().BreakLongToByteAndUint(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFieldKeys.ClientReceivedSubHourTime, lower4Bytes, fifthByte);
            }
        }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        var infoResult = PQSourceTickerInfo!.UpdateField(pqFieldUpdate);
        if (infoResult > 0) return infoResult;
        switch (pqFieldUpdate.Id)
        {
            case PQFieldKeys.SingleTickValue:
                IsSingleValueUpdated = true;
                SingleTickValue      = PQScaling.Unscale(pqFieldUpdate.Value, pqFieldUpdate.Flag);
                return 0;
            case PQFieldKeys.SourceSentDateTime:
                IsSourceTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceTime, pqFieldUpdate.Value);
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQFieldKeys.SourceSentSubHourTime:
                IsSourceTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref sourceTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQFieldKeys.QuoteBooleanFlags:
                SetBooleanFields((PQBooleanValues)pqFieldUpdate.Value);
                return 0;
            case PQFieldKeys.PQSyncStatus:
                FeedSyncStatus = (FeedSyncStatus)pqFieldUpdate.Value;
                return 0;
            case PQFieldKeys.SocketReceivingDateTime:
                IsSocketReceivedTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref socketReceivingTime, pqFieldUpdate.Value);
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
                return 0;
            case PQFieldKeys.SocketReceivingSubHourTime:
                IsSocketReceivedTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref socketReceivingTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
                return 0;
            case PQFieldKeys.ProcessedDateTime:
                IsProcessedTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref processedTime, pqFieldUpdate.Value);
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
                return 0;
            case PQFieldKeys.ProcessedSubHourTime:
                IsProcessedTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref processedTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
                return 0;
            case PQFieldKeys.DispatchedDateTime:
                IsDispatchedTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref dispatchedTime, pqFieldUpdate.Value);
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
                return 0;
            case PQFieldKeys.DispatchedSubHourTime:
                IsDispatchedTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref dispatchedTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
                return 0;
            case PQFieldKeys.ClientReceivedDateTime:
                IsClientReceivedTimeDateUpdated = true;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref clientReceivedTime, pqFieldUpdate.Value);
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
                return 0;
            case PQFieldKeys.ClientReceivedSubHourTime:
                IsClientReceivedTimeSubHourUpdated = true;
                PQFieldConverters.UpdateSubHourComponent(ref clientReceivedTime,
                                                         pqFieldUpdate.Flag.AppendUintToMakeLong(pqFieldUpdate.Value));
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
                return 0;
        }

        return -1;
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        if (PQSourceTickerInfo != null)
            foreach (var field in PQSourceTickerInfo.GetStringUpdates(snapShotTime, messageFlags))
                yield return field;
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate) =>
        PQSourceTickerInfo != null && PQSourceTickerInfo.UpdateFieldString(stringUpdate);


    public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref sourceTime, ipq0.SourceTime.GetHoursFromUnixEpoch());
                IsSourceTimeDateUpdated = originalSourceTime != sourceTime;
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
            }
            if (ipq0.IsSourceTimeSubHourUpdated || isFullReplace)
            {
                var originalSourceTime = sourceTime;
                PQFieldConverters.UpdateSubHourComponent(ref sourceTime, ipq0.SourceTime.GetSubHourComponent());
                IsSourceTimeSubHourUpdated = originalSourceTime != sourceTime;
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
            }
            if (ipq0.IsSocketReceivedTimeDateUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = socketReceivingTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref socketReceivingTime, ipq0.SocketReceivingTime.GetHoursFromUnixEpoch());
                IsSocketReceivedTimeDateUpdated = originalSocketReceivingTime != socketReceivingTime;
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
            }
            if (ipq0.IsSocketReceivedTimeSubHourUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = socketReceivingTime;
                PQFieldConverters.UpdateSubHourComponent(ref socketReceivingTime, ipq0.SocketReceivingTime.GetSubHourComponent());
                IsSocketReceivedTimeSubHourUpdated = originalSocketReceivingTime != socketReceivingTime;
                if (socketReceivingTime == DateTime.UnixEpoch) socketReceivingTime = default;
            }
            if (ipq0.IsProcessedTimeDateUpdated || isFullReplace)
            {
                var originalProcessedTime = processedTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref processedTime, ipq0.ProcessedTime.GetHoursFromUnixEpoch());
                IsProcessedTimeDateUpdated = originalProcessedTime != processedTime;
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
            }
            if (ipq0.IsProcessedTimeSubHourUpdated || isFullReplace)
            {
                var originalProcessedTime = processedTime;
                PQFieldConverters.UpdateSubHourComponent(ref processedTime, ipq0.ProcessedTime.GetSubHourComponent());
                IsProcessedTimeSubHourUpdated = originalProcessedTime != processedTime;
                if (processedTime == DateTime.UnixEpoch) processedTime = default;
            }
            if (ipq0.IsDispatchedTimeDateUpdated || isFullReplace)
            {
                var originalDispatchedTime = dispatchedTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref dispatchedTime, ipq0.DispatchedTime.GetHoursFromUnixEpoch());
                IsDispatchedTimeDateUpdated = originalDispatchedTime != dispatchedTime;
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
            }
            if (ipq0.IsDispatchedTimeSubHourUpdated || isFullReplace)
            {
                var originalDispatchedTime = dispatchedTime;
                PQFieldConverters.UpdateSubHourComponent(ref dispatchedTime, ipq0.DispatchedTime.GetSubHourComponent());
                IsDispatchedTimeSubHourUpdated = originalDispatchedTime != dispatchedTime;
                if (dispatchedTime == DateTime.UnixEpoch) dispatchedTime = default;
            }
            if (ipq0.IsClientReceivedTimeDateUpdated || isFullReplace)
            {
                var originalClientReceivedTime = clientReceivedTime;
                PQFieldConverters.UpdateHoursFromUnixEpoch(ref clientReceivedTime, ipq0.ClientReceivedTime.GetHoursFromUnixEpoch());
                IsClientReceivedTimeDateUpdated = originalClientReceivedTime != clientReceivedTime;
                if (clientReceivedTime == DateTime.UnixEpoch) clientReceivedTime = default;
            }
            if (ipq0.IsClientReceivedTimeSubHourUpdated || isFullReplace)
            {
                var originalClientReceivedTime = clientReceivedTime;
                PQFieldConverters.UpdateSubHourComponent(ref clientReceivedTime, ipq0.ClientReceivedTime.GetSubHourComponent());
                IsClientReceivedTimeSubHourUpdated = originalClientReceivedTime != clientReceivedTime;
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

    IVersionedMessage IStoreState<IVersionedMessage>.CopyFrom(IVersionedMessage source, CopyMergeFlags copyMergeFlags) =>
        (IVersionedMessage)CopyFrom((ITickInstant)source, copyMergeFlags);

    public IReusableObject<IVersionedMessage> CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IVersionedMessage)CopyFrom((ITickInstant)source, copyMergeFlags);

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

    public override string ToString() => $"{GetType().Name}({TickInstantToStringMembers})";
}
