using System.Text.Json.Serialization;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages;

public interface IPQMessage : IFeedEventStatusUpdate, IVersionedMessage, IDoublyLinkedListNode<IPQMessage>, ICloneable<IPQMessage>
  , IPQSupportsFieldUpdates<IFeedEventStatusUpdate>, IPQSupportsStringUpdates<IFeedEventStatusUpdate>, IEmptyaable
{
    PQMessageFlags? OverrideSerializationFlags { get; }

    uint      StreamId            { get; }
    uint      PQSequenceId        { get; }
    ISyncLock Lock                { get; }
    DateTime  LastPublicationTime { get; }
    DateTime  SocketReceivingTime { get; }
    DateTime  ProcessedTime       { get; }
    DateTime  DispatchedTime      { get; }

    new IPQMessage Clone();
}

public interface IPQMutableMessage : IPQMessage, IMutableFeedEventStatusUpdate, IStateResetable,
    IRelatedItems<IPQMessage>, IDoublyLinkedListNode<IPQMutableMessage>, IInterfacesComparable<IPQMutableMessage>,
    ITransferState<IPQMutableMessage>
{
    new PQMessageFlags? OverrideSerializationFlags { get; set; }

    new uint     PQSequenceId        { get; set; }
    new DateTime LastPublicationTime { get; set; }
    new DateTime SocketReceivingTime { get; set; }
    new DateTime ProcessedTime       { get; set; }
    new DateTime DispatchedTime      { get; set; }

    bool IsSocketReceivedTimeDateUpdated    { get; set; }
    bool IsSocketReceivedTimeSub2MinUpdated { get; set; }
    bool IsProcessedTimeDateUpdated         { get; set; }
    bool IsProcessedTimeSub2MinUpdated      { get; set; }
    bool IsDispatchedTimeDateUpdated        { get; set; }
    bool IsDispatchedTimeSub2MinUpdated     { get; set; }
    bool IsClientReceivedTimeDateUpdated    { get; set; }
    bool IsClientReceivedTimeSub2MinUpdated { get; set; }
    bool IsFeedSyncStatusUpdated            { get; set; }

    new IPQMutableMessage? Previous { get; set; }
    new IPQMutableMessage? Next     { get; set; }

    new IPQMutableMessage CopyFrom(IPQMutableMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    new IPQMutableMessage Clone();
}

public abstract class PQReusableMessage : ReusableObject<IFeedEventStatusUpdate>, IPQMutableMessage
{
    private FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good;

    protected uint NumOfUpdates = uint.MaxValue;

    protected readonly ISyncLock SyncLock = new SpinLockLight();

    protected DateTime ProcessedTimeField;
    protected DateTime SocketReceivingTimeField;
    protected DateTime ClientReceivedTimeField;
    protected DateTime DispatchedTimeField;

    public abstract bool HasUpdates { get; set; }

    public PQMessageFlags? OverrideSerializationFlags { get; set; }

    public abstract uint MessageId { get; }

    public abstract uint StreamId { get; }

    public uint PQSequenceId { get; set; }

    public ISyncLock Lock => SyncLock;

    public DateTime LastPublicationTime { get; set; }

    public virtual byte Version => 1;

    public uint UpdateCount => NumOfUpdates;

    public virtual bool IsEmpty
    {
        get =>
            ClientReceivedTimeField == DateTime.MinValue
         && DispatchedTimeField == DateTime.MinValue
         && ProcessedTimeField == DateTime.MinValue
         && SocketReceivingTime == DateTime.MinValue
         && OverrideSerializationFlags == null
         && FeedSyncStatus == FeedSyncStatus.Good
         && PQSequenceId == 0;
        set
        {
            if (!value) return;
            OverrideSerializationFlags = null;

            NumOfUpdates = 0;

            ClientReceivedTimeField  = default;
            LastPublicationTime      = default;
            SocketReceivingTimeField = default;

            PQSequenceId        = 0;
            ProcessedTimeField  = default;
            DispatchedTimeField = default;
            feedSyncStatus      = FeedSyncStatus.Good;
        }
    }

    public DateTime DispatchedTime
    {
        get => DispatchedTimeField;
        set
        {
            if (DispatchedTimeField == value) return;
            IsDispatchedTimeDateUpdated
                |= DispatchedTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsDispatchedTimeSub2MinUpdated |= DispatchedTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            DispatchedTimeField            =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    public DateTime ProcessedTime
    {
        get => ProcessedTimeField;
        set
        {
            if (ProcessedTimeField == value) return;
            IsProcessedTimeDateUpdated
                |= ProcessedTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsProcessedTimeSub2MinUpdated |= ProcessedTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            ProcessedTimeField            =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    public DateTime SocketReceivingTime
    {
        get => SocketReceivingTimeField;
        set
        {
            IsSocketReceivedTimeDateUpdated
                |= SocketReceivingTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsSocketReceivedTimeSub2MinUpdated
                |= SocketReceivingTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            SocketReceivingTimeField = value == DateTime.UnixEpoch ? default : value;
        }
    }

    public DateTime ClientReceivedTime
    {
        get => ClientReceivedTimeField;
        set
        {
            IsClientReceivedTimeDateUpdated
                |= ClientReceivedTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsClientReceivedTimeSub2MinUpdated
                |= ClientReceivedTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            ClientReceivedTimeField = value == DateTime.UnixEpoch ? default : value;
        }
    }
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

    public abstract bool IsSocketReceivedTimeDateUpdated    { get; set; }
    public abstract bool IsSocketReceivedTimeSub2MinUpdated { get; set; }
    public abstract bool IsProcessedTimeDateUpdated         { get; set; }
    public abstract bool IsProcessedTimeSub2MinUpdated      { get; set; }
    public abstract bool IsDispatchedTimeDateUpdated        { get; set; }
    public abstract bool IsDispatchedTimeSub2MinUpdated     { get; set; }
    public abstract bool IsFeedSyncStatusUpdated            { get; set; }
    public abstract bool IsClientReceivedTimeDateUpdated    { get; set; }
    public abstract bool IsClientReceivedTimeSub2MinUpdated { get; set; }

    [JsonIgnore] IPQMessage? IDoublyLinkedListNode<IPQMessage>.Previous { get; set; }
    [JsonIgnore] IPQMessage? IDoublyLinkedListNode<IPQMessage>.Next     { get; set; }

    [JsonIgnore]
    public IPQMutableMessage? Previous
    {
        get => GetPrevious<IPQMutableMessage?>();
        set => SetPrevious(value);
    }
    [JsonIgnore]
    public IPQMutableMessage? Next
    {
        get => GetNext<IPQMutableMessage?>();
        set => SetNext(value);
    }

    [JsonIgnore]
    IPQMutableMessage? IDoublyLinkedListNode<IPQMutableMessage>.Previous
    {
        get => GetPrevious<IPQMutableMessage?>();
        set => SetPrevious(value);
    }
    [JsonIgnore]
    IPQMutableMessage? IDoublyLinkedListNode<IPQMutableMessage>.Next
    {
        get => GetNext<IPQMutableMessage?>();
        set => SetNext(value);
    }

    protected T? GetPrevious<T>()
    {
        return (T?)(((IDoublyLinkedListNode<IPQMessage>)this).Previous);
    }

    protected void SetPrevious<T>(T value)
    {
        ((IDoublyLinkedListNode<IPQMessage>)this).Previous = (IPQMessage?)value;
    }

    protected T? GetNext<T>()
    {
        return (T?)(((IDoublyLinkedListNode<IPQMessage>)this).Next);
    }

    protected void SetNext<T>(T value)
    {
        ((IDoublyLinkedListNode<IPQMessage>)this).Next = (IPQMessage?)value;
    }

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IPQMutableMessage IPQMutableMessage.Clone() => (IPQMutableMessage)Clone();

    IPQMessage ICloneable<IPQMessage>.Clone() => Clone();

    IPQMessage IPQMessage.Clone() => Clone();

    public abstract override IPQMessage Clone();

    IFeedEventStatusUpdate ITransferState<IFeedEventStatusUpdate>.CopyFrom
        (IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IVersionedMessage ITransferState<IVersionedMessage>.CopyFrom
        (IVersionedMessage? source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IFeedEventStatusUpdate?)source!, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IFeedEventStatusUpdate?)source, copyMergeFlags);

    IPQMutableMessage ITransferState<IPQMutableMessage>.CopyFrom(IPQMutableMessage source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IPQMutableMessage IPQMutableMessage.CopyFrom(IPQMutableMessage source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);


    public override IPQMutableMessage CopyFrom(IFeedEventStatusUpdate? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        // only copy if changed
        if (source is IPQMutableMessage pqMm)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pqMm.IsSocketReceivedTimeDateUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = SocketReceivingTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref SocketReceivingTimeField
                                                                    , pqMm.SocketReceivingTime.Get2MinIntervalsFromUnixEpoch());
                IsSocketReceivedTimeDateUpdated = originalSocketReceivingTime != SocketReceivingTimeField;
                if (SocketReceivingTimeField == DateTime.UnixEpoch) SocketReceivingTimeField = default;
            }
            if (pqMm.IsSocketReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = SocketReceivingTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref SocketReceivingTimeField, pqMm.SocketReceivingTime.GetSub2MinComponent());
                IsSocketReceivedTimeSub2MinUpdated = originalSocketReceivingTime != SocketReceivingTimeField;
                if (SocketReceivingTimeField == DateTime.UnixEpoch) SocketReceivingTimeField = default;
            }
            if (pqMm.IsProcessedTimeDateUpdated || isFullReplace)
            {
                var originalProcessedTime = ProcessedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref ProcessedTimeField, pqMm.ProcessedTime.Get2MinIntervalsFromUnixEpoch());
                IsProcessedTimeDateUpdated = originalProcessedTime != ProcessedTimeField;
                if (ProcessedTimeField == DateTime.UnixEpoch) ProcessedTimeField = default;
            }
            if (pqMm.IsProcessedTimeSub2MinUpdated || isFullReplace)
            {
                var originalProcessedTime = ProcessedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref ProcessedTimeField, pqMm.ProcessedTime.GetSub2MinComponent());
                IsProcessedTimeSub2MinUpdated = originalProcessedTime != ProcessedTimeField;
                if (ProcessedTimeField == DateTime.UnixEpoch) ProcessedTimeField = default;
            }
            if (pqMm.IsDispatchedTimeDateUpdated || isFullReplace)
            {
                var originalDispatchedTime = DispatchedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref DispatchedTimeField
                                                                    , pqMm.DispatchedTime.Get2MinIntervalsFromUnixEpoch());
                IsDispatchedTimeDateUpdated = originalDispatchedTime != DispatchedTimeField;
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
            }
            if (pqMm.IsDispatchedTimeSub2MinUpdated || isFullReplace)
            {
                var originalDispatchedTime = DispatchedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref DispatchedTimeField, pqMm.DispatchedTime.GetSub2MinComponent());
                IsDispatchedTimeSub2MinUpdated = originalDispatchedTime != DispatchedTimeField;
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
            }
            if (pqMm.IsClientReceivedTimeDateUpdated || isFullReplace)
            {
                var originalClientReceivedTime = ClientReceivedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref ClientReceivedTimeField
                                                                    , pqMm.ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());
                IsClientReceivedTimeDateUpdated = originalClientReceivedTime != ClientReceivedTimeField;
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
            }
            if (pqMm.IsClientReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalClientReceivedTime = ClientReceivedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref ClientReceivedTimeField, pqMm.ClientReceivedTime.GetSub2MinComponent());
                IsClientReceivedTimeSub2MinUpdated = originalClientReceivedTime != ClientReceivedTimeField;
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
            }
            if (pqMm.IsFeedSyncStatusUpdated || isFullReplace) FeedSyncStatus = pqMm.FeedSyncStatus;

            OverrideSerializationFlags = pqMm.OverrideSerializationFlags;

            LastPublicationTime = pqMm.LastPublicationTime;
            PQSequenceId        = pqMm.PQSequenceId;
        }
        else if (source is IPQMessage pqMessage)
        {
            OverrideSerializationFlags = null;
            SocketReceivingTime        = pqMessage.SocketReceivingTime;
            DispatchedTime             = pqMessage.DispatchedTime;
            ProcessedTime              = pqMessage.ProcessedTime;
            ClientReceivedTime         = pqMessage.ClientReceivedTime;
            FeedSyncStatus             = pqMessage.FeedSyncStatus;
        }
        else if (source is not null)
        {
            ClientReceivedTime = source.ClientReceivedTime;
            FeedSyncStatus     = source.FeedSyncStatus;
        }

        return this;
    }

    public abstract void EnsureRelatedItemsAreConfigured(IPQMessage? item);


    public abstract IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags);

    public abstract bool UpdateFieldString(PQFieldStringUpdate stringUpdate);

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, StorageFlags messageFlags)
    {
        var updatedOnly          = (messageFlags & StorageFlags.Complete) == 0;
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
        if (!updatedOnly || IsFeedSyncStatusUpdated) yield return new PQFieldUpdate(PQFeedFields.PQSyncStatus, (byte)FeedSyncStatus);
    }


    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.PQSyncStatus:
                IsFeedSyncStatusUpdated = true; // incase of reset and sending 0;
                FeedSyncStatus          = (FeedSyncStatus)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.ClientSocketReceivingDateTime:
                IsSocketReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref SocketReceivingTimeField, pqFieldUpdate.Payload);
                if (SocketReceivingTimeField == DateTime.UnixEpoch) SocketReceivingTimeField = default;
                return 0;
            case PQFeedFields.ClientSocketReceivingSub2MinTime:
                IsSocketReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref SocketReceivingTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (SocketReceivingTimeField == DateTime.UnixEpoch) SocketReceivingTimeField = default;
                return 0;
            case PQFeedFields.ClientProcessedDateTime:
                IsProcessedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref ProcessedTimeField, pqFieldUpdate.Payload);
                if (ProcessedTimeField == DateTime.UnixEpoch) ProcessedTimeField = default;
                return 0;
            case PQFeedFields.ClientProcessedSub2MinTime:
                IsProcessedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref ProcessedTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (ProcessedTimeField == DateTime.UnixEpoch) ProcessedTimeField = default;
                return 0;
            case PQFeedFields.ClientDispatchedDateTime:
                IsDispatchedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref DispatchedTimeField, pqFieldUpdate.Payload);
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
                return 0;
            case PQFeedFields.ClientDispatchedSub2MinTime:
                IsDispatchedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref DispatchedTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
                return 0;
            case PQFeedFields.ClientReceivedDateTime:
                IsClientReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref ClientReceivedTimeField, pqFieldUpdate.Payload);
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
                return 0;
            case PQFeedFields.ClientReceivedSub2MinTime:
                IsClientReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref ClientReceivedTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
                return 0;
        }

        return -1;
    }

    public virtual bool AreEquivalent(IFeedEventStatusUpdate? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var publicationStatusSame = FeedSyncStatus == other.FeedSyncStatus;
        if (!exactTypes)
        {
            return publicationStatusSame;
        }
        if (other is not PQReusableMessage pqMessage) return false;
        
        var clientReceivedSame      = ClientReceivedTime == other.ClientReceivedTime;
        var sequenceIdSame          = PQSequenceId == pqMessage.PQSequenceId;
        var socketReceivingTimeSame = SocketReceivingTime == (pqMessage.SocketReceivingTime);
        var lastPubTimeSame         = LastPublicationTime == (pqMessage.LastPublicationTime);
        var processingTimeSame      = ProcessedTime == pqMessage.ProcessedTime;
        var dispatchTimeSame        = DispatchedTime == pqMessage.DispatchedTime;

        var allAreSame = clientReceivedSame && dispatchTimeSame && processingTimeSame
                      && lastPubTimeSame && socketReceivingTimeSame && sequenceIdSame && publicationStatusSame;
        return allAreSame;
    }

    bool IInterfacesComparable<IPQMutableMessage>.AreEquivalent(IPQMutableMessage? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public virtual void UpdateComplete()
    {
        if (HasUpdates) NumOfUpdates++;
    }

    public virtual void ResetFields()
    {
        OverrideSerializationFlags = null;

        NumOfUpdates = 0;

        ClientReceivedTimeField  = default;
        LastPublicationTime      = default;
        SocketReceivingTimeField = default;

        PQSequenceId        = 0;
        ProcessedTimeField  = default;
        DispatchedTimeField = default;
        feedSyncStatus      = FeedSyncStatus.Good;
    }
}
