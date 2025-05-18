// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Serialization;
using FortitudeCommon.AsyncProcessing;
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

[Flags]
public enum PQMessageUpdatedFlags : uint
{
    None                                  = 0x00_00_00
  , FeedSyncStatusFlag                    = 0x00_00_01
  , FeedConnectivityStatusUpdatedFlag     = 0x00_00_02
  , SocketReceivedDateUpdatedFlag         = 0x00_00_04
  , SocketReceivedSub2MinUpdatedFlag      = 0x00_00_08
  , ProcessedDateUpdatedFlag              = 0x00_00_10
  , ProcessedSub2MinUpdatedFlag           = 0x00_00_20
  , DispatchedDateUpdatedFlag             = 0x00_00_40
  , DispatchedSub2MinUpdatedFlag          = 0x00_00_80
  , ClientReceivedDateUpdatedFlag         = 0x00_01_00
  , ClientReceivedSub2MinUpdatedFlag      = 0x00_02_00
  , AdapterReceivedTimeDateUpdatedFlag    = 0x00_40_00
  , AdapterReceivedTimeSub2MinUpdatedFlag = 0x00_80_00
  , AdapterSentTimeDateUpdatedFlag        = 0x01_00_00
  , AdapterSentTimeSub2MinUpdatedFlag     = 0x02_00_00
}

public abstract class PQReusableMessage : ReusableObject<IFeedEventStatusUpdate>, IPQMessage
{
    private FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good;

    protected uint NumOfUpdates = uint.MaxValue;

    protected readonly ISyncLock SyncLock = new SpinLockLight();

    protected DateTime InboundProcessedTimeField;
    protected DateTime InboundSocketReceivingTimeField;
    protected DateTime ClientReceivedTimeField;
    protected DateTime DispatchedTimeField;
    protected DateTime AdapterReceivedTimeField;
    protected DateTime AdapterSentTimeField;

    protected PQMessageUpdatedFlags       MessageUpdatedFlags;
    private   FeedConnectivityStatusFlags feedMarketConnectivityStatus;

    protected PQReusableMessage() { }

    protected PQReusableMessage(IFeedEventStatusUpdate? toClone)
    {
        if (toClone == null) return;
        FeedSyncStatus = toClone.FeedSyncStatus;

        SubscriberDispatchedTime   = toClone.SubscriberDispatchedTime;
        InboundProcessedTime       = toClone.InboundProcessedTime;
        InboundSocketReceivingTime = toClone.InboundSocketReceivingTime;
        ClientReceivedTime         = toClone.ClientReceivedTime;
        AdapterSentTime            = toClone.AdapterSentTime;
        AdapterReceivedTime        = toClone.AdapterReceivedTime;
    }

    protected PQReusableMessage(IPQMessage? toClone)
    {
        if (toClone == null) return;
        PQSequenceId   = toClone.PQSequenceId;
        FeedSyncStatus = toClone.FeedSyncStatus;

        LastPublicationTime        = toClone.LastPublicationTime;
        SubscriberDispatchedTime   = toClone.SubscriberDispatchedTime;
        InboundProcessedTime       = toClone.InboundProcessedTime;
        InboundSocketReceivingTime = toClone.InboundSocketReceivingTime;
        ClientReceivedTime         = toClone.ClientReceivedTime;
        AdapterSentTime            = toClone.AdapterSentTime;
        AdapterReceivedTime        = toClone.AdapterReceivedTime;

        OverrideSerializationFlags = toClone.OverrideSerializationFlags;
        SetFlagsSame(toClone);
    }


    public PQMessageFlags? OverrideSerializationFlags { get; set; }

    public abstract uint MessageId { get; }

    public abstract uint   StreamId   { get; }
    public abstract string StreamName { get; }

    public uint PQSequenceId { get; set; }

    public ISyncLock Lock => SyncLock;

    public DateTime LastPublicationTime { get; set; }

    public virtual byte Version => 1;

    public uint UpdateCount => NumOfUpdates;

    public FeedConnectivityStatusFlags FeedMarketConnectivityStatus
    {
        get => feedMarketConnectivityStatus;
        set
        {
            IsFeedConnectivityStatusUpdated |= value != feedMarketConnectivityStatus || NumOfUpdates == 0;
            feedMarketConnectivityStatus    =  value;
        }
    }

    public DateTime SubscriberDispatchedTime
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

    public DateTime InboundProcessedTime
    {
        get => InboundProcessedTimeField;
        set
        {
            if (InboundProcessedTimeField == value) return;
            IsProcessedTimeDateUpdated
                |= InboundProcessedTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsProcessedTimeSub2MinUpdated |= InboundProcessedTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            InboundProcessedTimeField     =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    public DateTime InboundSocketReceivingTime
    {
        get => InboundSocketReceivingTimeField;
        set
        {
            IsSocketReceivedTimeDateUpdated
                |= InboundSocketReceivingTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsSocketReceivedTimeSub2MinUpdated
                |= InboundSocketReceivingTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || NumOfUpdates == 0;
            InboundSocketReceivingTimeField = value == DateTime.UnixEpoch ? default : value;
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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime AdapterSentTime
    {
        get => AdapterSentTimeField;
        set
        {
            IsAdapterSentTimeDateUpdated |= AdapterSentTimeField.Get2MinIntervalsFromUnixEpoch()
             != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsAdapterSentTimeSub2MinUpdated |= AdapterSentTimeField.GetSub2MinComponent()
             != value.GetSub2MinComponent() || NumOfUpdates == 0;
            AdapterSentTimeField = value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime AdapterReceivedTime
    {
        get => AdapterReceivedTimeField;
        set
        {
            IsAdapterReceivedTimeDateUpdated |= AdapterReceivedTimeField.Get2MinIntervalsFromUnixEpoch()
             != value.Get2MinIntervalsFromUnixEpoch() || NumOfUpdates == 0;
            IsAdapterReceivedTimeSub2MinUpdated |= AdapterReceivedTimeField.GetSub2MinComponent()
             != value.GetSub2MinComponent() || NumOfUpdates == 0;
            AdapterReceivedTimeField = value == DateTime.UnixEpoch ? default : value;
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

    public bool IsFeedConnectivityStatusUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.FeedConnectivityStatusUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.FeedConnectivityStatusUpdatedFlag;

            else if (IsFeedConnectivityStatusUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.FeedConnectivityStatusUpdatedFlag;
        }
    }


    [JsonIgnore]
    public bool IsSocketReceivedTimeDateUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.SocketReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.SocketReceivedDateUpdatedFlag;

            else if (IsSocketReceivedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.SocketReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeSub2MinUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.SocketReceivedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.SocketReceivedSub2MinUpdatedFlag;

            else if (IsSocketReceivedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.SocketReceivedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeDateUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.ProcessedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ProcessedDateUpdatedFlag;

            else if (IsProcessedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ProcessedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeSub2MinUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.ProcessedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ProcessedSub2MinUpdatedFlag;

            else if (IsProcessedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ProcessedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeDateUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.DispatchedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.DispatchedDateUpdatedFlag;

            else if (IsDispatchedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.DispatchedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeSub2MinUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.DispatchedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.DispatchedSub2MinUpdatedFlag;

            else if (IsDispatchedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.DispatchedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeDateUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.ClientReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ClientReceivedDateUpdatedFlag;

            else if (IsClientReceivedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ClientReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeSub2MinUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.ClientReceivedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ClientReceivedSub2MinUpdatedFlag;

            else if (IsClientReceivedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ClientReceivedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterSentTimeDateUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterSentTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterSentTimeDateUpdatedFlag;

            else if (IsAdapterSentTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterSentTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterSentTimeSub2MinUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag;

            else if (IsAdapterSentTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedTimeDateUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;

            else if (IsAdapterReceivedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedTimeSub2MinUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterReceivedTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterReceivedTimeSub2MinUpdatedFlag;

            else if (IsAdapterReceivedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterReceivedTimeSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsFeedSyncStatusUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.FeedSyncStatusFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.FeedSyncStatusFlag;

            else if (IsFeedSyncStatusUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.FeedSyncStatusFlag;
        }
    }

    [JsonIgnore] IPQMessage? IDoublyLinkedListNode<IPQMessage>.Previous { get; set; }
    [JsonIgnore] IPQMessage? IDoublyLinkedListNode<IPQMessage>.Next     { get; set; }

    [JsonIgnore]
    public IPQMessage? Previous
    {
        get => GetPrevious<IPQMessage?>();
        set => SetPrevious(value);
    }
    [JsonIgnore]
    public IPQMessage? Next
    {
        get => GetNext<IPQMessage?>();
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

    public virtual bool HasUpdates
    {
        get => MessageUpdatedFlags != PQMessageUpdatedFlags.None;
        set
        {
            if (value) return;
            MessageUpdatedFlags = PQMessageUpdatedFlags.None;
        }
    }

    public virtual bool IsEmpty
    {
        get =>
            ClientReceivedTimeField == DateTime.MinValue
         && DispatchedTimeField == DateTime.MinValue
         && InboundProcessedTimeField == DateTime.MinValue
         && InboundSocketReceivingTimeField == DateTime.MinValue
         && AdapterSentTimeField == DateTime.MinValue
         && AdapterReceivedTimeField == DateTime.MinValue
         && OverrideSerializationFlags == null
         && FeedSyncStatus == FeedSyncStatus.NotStarted
         && FeedMarketConnectivityStatus == FeedConnectivityStatusFlags.None
         && PQSequenceId == 0;
        set
        {
            if (!value) return;
            OverrideSerializationFlags = null;

            NumOfUpdates = 0;

            ClientReceivedTimeField         = default;
            DispatchedTimeField             = default;
            InboundProcessedTimeField       = default;
            LastPublicationTime             = default;
            InboundSocketReceivingTimeField = default;
            AdapterSentTimeField            = default;
            AdapterReceivedTimeField        = default;

            FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.None;

            PQSequenceId   = 0;
            feedSyncStatus = FeedSyncStatus.NotStarted;
        }
    }

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => (IVersionedMessage)Clone();

    IPQMessage IPQMessage.Clone() => (IPQMessage)Clone();

    IPQMessage ICloneable<IPQMessage>.Clone() => (IPQMessage)Clone();

    IMutableFeedEventStatusUpdate ICloneable<IMutableFeedEventStatusUpdate>.Clone() => (IMutableFeedEventStatusUpdate)Clone();

    IMutableFeedEventStatusUpdate IMutableFeedEventStatusUpdate.Clone() => (IMutableFeedEventStatusUpdate)Clone();

    public abstract override IFeedEventStatusUpdate Clone();

    IFeedEventStatusUpdate ITransferState<IFeedEventStatusUpdate>.CopyFrom
        (IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IVersionedMessage ITransferState<IVersionedMessage>.CopyFrom
        (IVersionedMessage? source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IFeedEventStatusUpdate?)source!, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IFeedEventStatusUpdate?)source, copyMergeFlags);

    IPQMessage ITransferState<IPQMessage>.CopyFrom(IPQMessage source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    IPQMessage IPQMessage.CopyFrom(IPQMessage source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);


    public override IPQMessage CopyFrom(IFeedEventStatusUpdate? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQMessage pqMesg)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pqMesg.IsFeedConnectivityStatusUpdated || isFullReplace)
            {
                IsFeedConnectivityStatusUpdated = true;
                FeedMarketConnectivityStatus    = pqMesg.FeedMarketConnectivityStatus;
            }
            if (pqMesg.IsAdapterSentTimeDateUpdated || isFullReplace)
            {
                var originalAdapterSentTime = AdapterSentTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref AdapterSentTimeField,
                                                                      pqMesg.AdapterSentTime.Get2MinIntervalsFromUnixEpoch());
                IsAdapterSentTimeDateUpdated |= originalAdapterSentTime != AdapterSentTimeField;
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
            }
            if (pqMesg.IsAdapterSentTimeSub2MinUpdated || isFullReplace)
            {
                var originalAdapterSentTime = AdapterSentTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref AdapterSentTimeField,
                                                         pqMesg.AdapterSentTime.GetSub2MinComponent());
                IsAdapterSentTimeSub2MinUpdated |= originalAdapterSentTime != AdapterSentTimeField;
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
            }
            if (pqMesg.IsAdapterReceivedTimeDateUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = AdapterReceivedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref AdapterReceivedTimeField,
                                                                      pqMesg.AdapterReceivedTime.Get2MinIntervalsFromUnixEpoch());
                IsAdapterReceivedTimeDateUpdated |= originalAdapterReceivedTime != AdapterReceivedTimeField;
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
            }
            if (pqMesg.IsAdapterReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = AdapterReceivedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref AdapterReceivedTimeField,
                                                         pqMesg.AdapterReceivedTime.GetSub2MinComponent());
                IsAdapterReceivedTimeSub2MinUpdated |= originalAdapterReceivedTime != AdapterReceivedTimeField;
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
            }
            if (pqMesg.IsSocketReceivedTimeDateUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = InboundSocketReceivingTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref InboundSocketReceivingTimeField
                                                                    , pqMesg.InboundSocketReceivingTime.Get2MinIntervalsFromUnixEpoch());
                IsSocketReceivedTimeDateUpdated = originalSocketReceivingTime != InboundSocketReceivingTimeField;
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
            }
            if (pqMesg.IsSocketReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = InboundSocketReceivingTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref InboundSocketReceivingTimeField
                                                       , pqMesg.InboundSocketReceivingTime.GetSub2MinComponent());
                IsSocketReceivedTimeSub2MinUpdated = originalSocketReceivingTime != InboundSocketReceivingTimeField;
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
            }
            if (pqMesg.IsProcessedTimeDateUpdated || isFullReplace)
            {
                var originalProcessedTime = InboundProcessedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref InboundProcessedTimeField
                                                                    , pqMesg.InboundProcessedTime.Get2MinIntervalsFromUnixEpoch());
                IsProcessedTimeDateUpdated = originalProcessedTime != InboundProcessedTimeField;
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
            }
            if (pqMesg.IsProcessedTimeSub2MinUpdated || isFullReplace)
            {
                var originalProcessedTime = InboundProcessedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref InboundProcessedTimeField, pqMesg.InboundProcessedTime.GetSub2MinComponent());
                IsProcessedTimeSub2MinUpdated = originalProcessedTime != InboundProcessedTimeField;
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
            }
            if (pqMesg.IsDispatchedTimeDateUpdated || isFullReplace)
            {
                var originalDispatchedTime = DispatchedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref DispatchedTimeField
                                                                    , pqMesg.SubscriberDispatchedTime.Get2MinIntervalsFromUnixEpoch());
                IsDispatchedTimeDateUpdated = originalDispatchedTime != DispatchedTimeField;
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
            }
            if (pqMesg.IsDispatchedTimeSub2MinUpdated || isFullReplace)
            {
                var originalDispatchedTime = DispatchedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref DispatchedTimeField, pqMesg.SubscriberDispatchedTime.GetSub2MinComponent());
                IsDispatchedTimeSub2MinUpdated = originalDispatchedTime != DispatchedTimeField;
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
            }
            if (pqMesg.IsClientReceivedTimeDateUpdated || isFullReplace)
            {
                var originalClientReceivedTime = ClientReceivedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref ClientReceivedTimeField
                                                                    , pqMesg.ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());
                IsClientReceivedTimeDateUpdated = originalClientReceivedTime != ClientReceivedTimeField;
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
            }
            if (pqMesg.IsClientReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalClientReceivedTime = ClientReceivedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref ClientReceivedTimeField, pqMesg.ClientReceivedTime.GetSub2MinComponent());
                IsClientReceivedTimeSub2MinUpdated = originalClientReceivedTime != ClientReceivedTimeField;
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
            }
            if (pqMesg.IsFeedSyncStatusUpdated || isFullReplace) FeedSyncStatus = pqMesg.FeedSyncStatus;

            OverrideSerializationFlags = pqMesg.OverrideSerializationFlags;

            LastPublicationTime = pqMesg.LastPublicationTime;
            PQSequenceId        = pqMesg.PQSequenceId;
        }
        else if (source is not null)
        {
            OverrideSerializationFlags = null;

            FeedMarketConnectivityStatus = source.FeedMarketConnectivityStatus;
            FeedSyncStatus               = source.FeedSyncStatus;

            ClientReceivedTime         = source.ClientReceivedTime;
            AdapterSentTime            = source.AdapterSentTime;
            AdapterReceivedTime        = source.AdapterReceivedTime;
            InboundSocketReceivingTime = source.InboundSocketReceivingTime;
            SubscriberDispatchedTime   = source.SubscriberDispatchedTime;
            InboundProcessedTime       = source.InboundProcessedTime;
        }

        return this;
    }

    public virtual void SetPublisherStateToConnectivityStatus(PublisherStates publisherStates, DateTime atDateTime)
    {
        // Todo map publisherStates to feed states
    }

    public abstract void EnsureRelatedItemsAreConfigured(IPQMessage? item);

    public abstract IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags);

    public abstract bool UpdateFieldString(PQFieldStringUpdate stringUpdate);

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, StorageFlags messageFlags)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        // only copy if changed

        if (!updatedOnly || IsFeedConnectivityStatusUpdated)
            yield return new PQFieldUpdate(PQFeedFields.FeedMarketConnectivityStatus, (uint)FeedMarketConnectivityStatus);
        if (!FeedMarketConnectivityStatus.HasIsAdapterReplay()) AdapterSentTime = snapShotTime;
        if (!updatedOnly || IsAdapterSentTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.AdapterSentDateTime, AdapterSentTimeField.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsAdapterSentTimeSub2MinUpdated)
        {
            var extended = AdapterSentTimeField.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.AdapterSentSub2MinTime, value, extended);
        }
        if (!updatedOnly || IsAdapterReceivedTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.AdapterReceivedDateTime, AdapterReceivedTimeField.Get2MinIntervalsFromUnixEpoch());
        if (!updatedOnly || IsAdapterReceivedTimeSub2MinUpdated)
        {
            var extended = AdapterReceivedTimeField.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.AdapterReceivedSub2MinTime, value, extended);
        }
        var includeReceiverTimes = (messageFlags & StorageFlags.IncludeReceiverTimes) > 0;
        if (includeReceiverTimes)
        {
            if (!updatedOnly || IsFeedSyncStatusUpdated) yield return new PQFieldUpdate(PQFeedFields.PQSyncStatus, (byte)FeedSyncStatus);
            if (!updatedOnly || IsSocketReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientSocketReceivingDateTime
                                             , InboundSocketReceivingTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsSocketReceivedTimeSub2MinUpdated)
            {
                var extended = InboundSocketReceivingTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientSocketReceivingSub2MinTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsProcessedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientProcessedDateTime, InboundProcessedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsProcessedTimeSub2MinUpdated)
            {
                var extended = InboundProcessedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientProcessedSub2MinTime, lower4Bytes, extended);
            }

            if (!updatedOnly || IsDispatchedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientDispatchedDateTime, SubscriberDispatchedTime.Get2MinIntervalsFromUnixEpoch());

            if (!updatedOnly || IsDispatchedTimeSub2MinUpdated)
            {
                var extended = SubscriberDispatchedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
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
    }


    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.PQSyncStatus:
                IsFeedSyncStatusUpdated = true; // incase of reset and sending 0;
                FeedSyncStatus          = (FeedSyncStatus)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.FeedMarketConnectivityStatus:
                IsFeedConnectivityStatusUpdated = true; // incase of reset and sending 0;
                FeedMarketConnectivityStatus    = (FeedConnectivityStatusFlags)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.AdapterSentDateTime:
                IsAdapterSentTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref AdapterSentTimeField, pqFieldUpdate.Payload);
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
                return 0;
            case PQFeedFields.AdapterSentSub2MinTime:
                IsAdapterSentTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref AdapterSentTimeField, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
                return 0;
            case PQFeedFields.AdapterReceivedDateTime:
                IsAdapterReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref AdapterReceivedTimeField, pqFieldUpdate.Payload);
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
                return 0;
            case PQFeedFields.AdapterReceivedSub2MinTime:
                IsAdapterReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref AdapterReceivedTimeField, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
                return 0;
            case PQFeedFields.ClientSocketReceivingDateTime:
                IsSocketReceivedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref InboundSocketReceivingTimeField, pqFieldUpdate.Payload);
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
                return 0;
            case PQFeedFields.ClientSocketReceivingSub2MinTime:
                IsSocketReceivedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref InboundSocketReceivingTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
                return 0;
            case PQFeedFields.ClientProcessedDateTime:
                IsProcessedTimeDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref InboundProcessedTimeField, pqFieldUpdate.Payload);
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
                return 0;
            case PQFeedFields.ClientProcessedSub2MinTime:
                IsProcessedTimeSub2MinUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref InboundProcessedTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
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

        var feedConnectivityStatus = FeedMarketConnectivityStatus == other.FeedMarketConnectivityStatus;
        var feedSyncStatusSame     = FeedSyncStatus == other.FeedSyncStatus;

        if (!exactTypes)
        {
            return feedSyncStatusSame && feedConnectivityStatus;
        }
        if (other is not PQReusableMessage pqMessage) return false;


        var adapterReceivedSame     = AdapterReceivedTimeField.Equals(pqMessage.AdapterReceivedTime);
        var adapterSentSame         = AdapterSentTimeField.Equals(pqMessage.AdapterSentTime);
        var clientReceivedSame      = ClientReceivedTime == other.ClientReceivedTime;
        var socketReceivingTimeSame = InboundSocketReceivingTime == (pqMessage.InboundSocketReceivingTime);
        var lastPubTimeSame         = LastPublicationTime == (pqMessage.LastPublicationTime);
        var processingTimeSame      = InboundProcessedTime == pqMessage.InboundProcessedTime;
        var dispatchTimeSame        = SubscriberDispatchedTime == pqMessage.SubscriberDispatchedTime;
        var pqSequenceIdSame        = PQSequenceId == pqMessage.PQSequenceId;

        var allAreSame = adapterReceivedSame && adapterSentSame && clientReceivedSame && dispatchTimeSame && processingTimeSame && lastPubTimeSame
                      && socketReceivingTimeSame && pqSequenceIdSame && feedSyncStatusSame && feedConnectivityStatus;
        return allAreSame;
    }

    bool IInterfacesComparable<IPQMessage>.AreEquivalent(IPQMessage? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    public virtual void UpdateComplete()
    {
        if (HasUpdates) NumOfUpdates++;
    }

    public virtual IPQMessage ResetWithTracking()
    {
        OverrideSerializationFlags = null;

        NumOfUpdates = 0;

        ClientReceivedTimeField         = default;
        LastPublicationTime             = default;
        InboundSocketReceivingTimeField = default;
        InboundProcessedTimeField       = default;
        AdapterReceivedTimeField        = default;
        AdapterSentTimeField            = default;
        DispatchedTimeField             = default;

        FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.None;

        PQSequenceId   = 0;
        feedSyncStatus = FeedSyncStatus.Good;
        HasUpdates     = false;
        return this;
    }

    protected void SetFlagsSame(IPQMessage toCopyFlags)
    {
        if (toCopyFlags is PQReusableMessage pqToClone) MessageUpdatedFlags = pqToClone.MessageUpdatedFlags;
    }
}
