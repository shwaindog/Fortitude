// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeBusRules.Connectivity.Cluster.Messages;

public interface IRemoteTopicSubscribeResponse : IBusRulesMessage
{
    int SubscriberSubscriptionId { get; }

    bool Succeeded { get; }

    MutableString? FailMessage { get; }
}

public class RemoteTopicSubscribeResponse : BusRulesMessage, IRemoteTopicSubscribeResponse
{
    public RemoteTopicSubscribeResponse() { }

    public RemoteTopicSubscribeResponse(IRemoteTopicSubscribeResponse toClone) : base(toClone)
    {
        SubscriberSubscriptionId = toClone.SubscriberSubscriptionId;

        Succeeded   = toClone.Succeeded;
        FailMessage = toClone.FailMessage?.Clone();
    }

    public override uint MessageId => (uint)InterClusterMessageIds.RemoteTopicSubscribeResponse;

    [OrxMandatoryField(10)] public int  SubscriberSubscriptionId { get; set; }
    [OrxMandatoryField(11)] public bool Succeeded                { get; set; }

    [OrxOptionalField(10)] public MutableString? FailMessage { get; set; }

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<RemoteTopicSubscribeResponse>().CopyFrom(this) ?? new RemoteTopicSubscribeResponse(this);


    public override IVersionedMessage CopyFrom
    (IVersionedMessage versionedMessage
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(versionedMessage, copyMergeFlags);
        if (versionedMessage is IRemoteTopicSubscribeResponse remoteTopicSubscribeResponse)
        {
            SubscriberSubscriptionId = remoteTopicSubscribeResponse.SubscriberSubscriptionId;

            Succeeded   = remoteTopicSubscribeResponse.Succeeded;
            FailMessage = remoteTopicSubscribeResponse.FailMessage.CopyOrClone(FailMessage);
        }

        return this;
    }

    public override void StateReset()
    {
        SubscriberSubscriptionId = 0;

        Succeeded = false;

        FailMessage?.DecrementRefCount();
        FailMessage = null;
        base.StateReset();
    }
}
